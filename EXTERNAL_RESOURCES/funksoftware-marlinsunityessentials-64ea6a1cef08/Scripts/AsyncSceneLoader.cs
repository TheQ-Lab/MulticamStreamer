using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AsyncSceneLoader : MonoBehaviour
{
    public bool debugMode = false;
    public static bool forceAssetCleanup = false;
    //public enum LoadingType { StandardAsync, AdditiveAsync };
    [SerializeField] protected LoadSceneMode loadingType;

    private static AsyncOperation sceneLoading;
    private static string sceneLoadingName;
    private static bool sleepGOInitialized = false;

    #region Everything needed for asynchronous/additive asynchronous loading/unloading regardless of scenes
    protected virtual void Start() { SceneAutoActivatorRegistration(); }

    private static bool sceneAutoActivatorListening = false;
    protected void SceneAutoActivatorRegistration()
    {
        if (sceneAutoActivatorListening) return;
        SceneManager.sceneLoaded += (Scene s, LoadSceneMode l) => { if (l == LoadSceneMode.Additive) SceneManager.SetActiveScene(s); };
        sceneAutoActivatorListening = true;
    }

    protected async Task LoadLevelAsync(string sceneNameOrPath)
    {
        #region Explanation
        // < 0.9 => loading (isDone == false) | 0.9 => loading done, waiting to activate scene (isDone == false) | 1.0 => new scene is loaded & activated (isDone == true)
        /*if (sceneLoading != null && !sceneLoading.isDone) // only when there is no scene load in progress
        {                                                 // (a logged sceneLoadingOperation that is not done)
            Debug.LogWarning("There is a Scene aleady loading and another load attempt was made");
            return;                                      
        }*/
        #endregion

        if (sceneLoadingName == sceneNameOrPath && sceneLoading?.progress <= .9f)
        {
            if (debugMode) Debug.LogWarning("There is a Scene aleady loading and another load attempt was made");
        }
        else //if (sceneLoading == null || sceneLoading.isDone)
        {
            if (debugMode) Debug.Log("Pre-Load Level " + sceneNameOrPath);

            if (loadingType == LoadSceneMode.Additive) IfPackSleeperObject();

            if (sceneLoading?.progress <= .9f)
                await CancelOldPreloadFallback();

            sceneLoading = SceneManager.LoadSceneAsync(sceneNameOrPath, loadingType);
            sceneLoading.allowSceneActivation = false;
            sceneLoadingName = sceneNameOrPath;
        }

        while (sceneLoading.progress < 0.9f)
        {
            //Debug.Log("Wait... Progress: " + sceneLoading.progress + ", isDone: " + sceneLoading.isDone);
            await Task.Yield();
        }
        //Debug.Log("Progress: " + sceneLoading.progress + ", isDone: " + sceneLoading.isDone);
    }
    protected async Task LaunchLevelAsync(string sceneNameOrPath)
    {
        //double time = Time.fixedUnscaledTimeAsDouble;

        if (sceneLoading == null || sceneLoadingName != sceneNameOrPath || sceneLoading.progress != 0.9f)       // if there isn't a level preloaded or???
            await LoadLevelAsync(sceneNameOrPath);                              // preload isn't done, do preload and wait

        if (loadingType == LoadSceneMode.Additive)
            SceneManager.GetActiveScene().GetRootGameObjects()[0].SetActive(false);

        sceneLoading.allowSceneActivation = true;                               // Do the actual loading

        if (debugMode) Debug.Log("Launch Level: " + sceneNameOrPath);

        while (sceneLoading.isDone != true)                                     // Let the level complete loading before giving back for initialization
            await Task.Yield();

        if (forceAssetCleanup) Resources.UnloadUnusedAssets();

        //Debug.Log("Load took " + (Time.fixedUnscaledTimeAsDouble - time));
    }

    protected async Task UnloadNRelaunchLevel(string unloadSceneNameOrPath)
    {
        //double time = Time.fixedUnscaledTimeAsDouble;
        if (sceneLoading == null || sceneLoading.isDone)
        {
            if (debugMode) Debug.Log("Unload Level " + unloadSceneNameOrPath);
            sceneLoading = SceneManager.UnloadSceneAsync(unloadSceneNameOrPath);
            //sceneLoading.allowSceneActivation = false;
            //Debug.Log("Progress: " + sceneLoading.progress + ", isDone: " + sceneLoading.isDone + ", allowSceneActivation: " + sceneLoading.allowSceneActivation);
        }
        else
        {
            if (debugMode) Debug.LogWarning("There is a Scene aleady loading and another load attempt was made");
        }

        while (sceneLoading.progress < 0.9f)
        {
            await Task.Yield();
            //Debug.Log("Progress: " + sceneLoading.progress + ", isDone: " + sceneLoading.isDone + ", allowSceneActivation: " + sceneLoading.allowSceneActivation);
        }

        SceneManager.GetSceneAt(0).GetRootGameObjects()[0].SetActive(true);
        SceneManager.SetActiveScene(SceneManager.GetSceneAt(0));
        if (forceAssetCleanup) Resources.UnloadUnusedAssets();

        //Debug.Log("Load took " + (Time.fixedUnscaledTimeAsDouble - time));
    }

    private void IfPackSleeperObject()
    {
        if (!sleepGOInitialized)
        {
            Transform parent = new GameObject().transform;
            parent.name = "SleeperObject";
            foreach (GameObject o in SceneManager.GetActiveScene().GetRootGameObjects())
                o.transform.SetParent(parent, true);
            sleepGOInitialized = true;
        }
    }

    // A fallback if a different scene than preloaded should be loaded, not recommended (Double event systems for a frame - quick load completion + unload
    private async Task CancelOldPreloadFallback()
    {
        Debug.LogWarning("Scene Loading mismatch: A different scene was preloaded and must now be unloaded!  (fallback)" +
                    "   vvv Errors will follow vvv");
        sceneLoading.allowSceneActivation = true;
        while (sceneLoading.isDone != true)
            await Task.Yield();
        SceneManager.UnloadSceneAsync(sceneLoadingName);
        //Resources.UnloadUnusedAssets();
    }
    #endregion
}
