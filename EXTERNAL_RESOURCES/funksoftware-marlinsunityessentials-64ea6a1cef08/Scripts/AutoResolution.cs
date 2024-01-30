using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class AutoResolution : MonoBehaviour
{
    public bool debug = false;
    #region Singleton
    public static AutoResolution Instance;
    private void Awake()
    {
        TooManyFuncts.Singletonize(ref Instance, this, true);
    }
    #endregion

    public enum ScreenMode { Borderless, Exclusive };
    [SerializeField] public ScreenMode screenMode;
    public enum FrameRateOverride { Auto, Hz15, Hz30, Hz40, Hz60, Hz90, Hz120, Hz144, Hz240, Unlimited };
    [SerializeField] public FrameRateOverride frameRateOverride;
    public enum VSyncOverride { Auto, Off, Full, Half, Third };
    [SerializeField] public VSyncOverride vSyncOverride;

    private string currentDisplay;

    private void Start()
    {
        SetBestResolution();
    }

    private void SetBestResolution()
    {
        if (debug) Debug.Log("Base Res: " + Screen.currentResolution);

        FullScreenMode mode = FullScreenMode.FullScreenWindow;
        if (screenMode is ScreenMode.Borderless)
            mode = FullScreenMode.FullScreenWindow;
        else if (screenMode is ScreenMode.Exclusive)
            mode = FullScreenMode.ExclusiveFullScreen;

        int refreshRate = 60;
        if (frameRateOverride is FrameRateOverride.Auto)
            foreach (Resolution r in Screen.resolutions)
            {
                if (r.refreshRate > refreshRate) refreshRate = r.refreshRate;
            }
        else
            refreshRate = GetFrameRateValue(frameRateOverride);

        Resolution maxRes = Screen.resolutions[Screen.resolutions.Length - 1];
        if (debug) Debug.Log("Highest res: " + maxRes + " with max Hz: " + refreshRate);
        if (maxRes.height >= 2160 && maxRes.width >= 3840)
            Screen.SetResolution(3840, 2160, mode, refreshRate);
        else if (maxRes.height >= 1440 && maxRes.width >= 2560)
            Screen.SetResolution(2560, 1440, mode, refreshRate);
        else if (maxRes.height >= 1080 && maxRes.width >= 1920)
            Screen.SetResolution(1920, 1080, mode, refreshRate);
        else
            Screen.SetResolution(1280, 720, mode, 60);

        currentDisplay = Screen.mainWindowDisplayInfo.name;

        if (vSyncOverride is not VSyncOverride.Auto)
            QualitySettings.vSyncCount = GetVSyncValue(vSyncOverride);

        //https://docs.unity3d.com/ScriptReference/Application-targetFrameRate.html
        /*
        QualitySettings.vSyncCount = 1;
        Screen.SetResolution(2560, 1440, FullScreenMode.ExclusiveFullScreen, 30);
        Application.targetFrameRate = 30;
        Debug.Log(QualitySettings.vSyncCount);
        */

        if (!IsInvoking(nameof(CheckRes))) InvokeRepeating(nameof(CheckRes), 0.5f, 4f);
        else if (debug) Debug.Log("Invoke CheckRes() already running...");

        if (debug) Debug.Log("Check... Currently " + Screen.currentResolution + " with mode " + Screen.fullScreenMode + ", VBlank-Count " + QualitySettings.vSyncCount);
    }

    private void CheckRes()
    {
        if (Screen.mainWindowDisplayInfo.name != currentDisplay)
        {
            if (debug) Debug.LogWarning("Display Changed!");
            SetBestResolution();
        }
    }

    public static int GetFrameRateValue(FrameRateOverride fr)
    {
        var res = 60;
        if (fr is FrameRateOverride.Auto)
            res = 999;
        else if (fr is FrameRateOverride.Hz15)
            res = 15;
        else if (fr is FrameRateOverride.Hz30)
            res = 30;
        else if (fr is FrameRateOverride.Hz40)
            res = 40;
        else if (fr is FrameRateOverride.Hz60)
            res = 60;
        else if (fr is FrameRateOverride.Hz90)
            res = 90;
        else if (fr is FrameRateOverride.Hz120)
            res = 120;
        else if (fr is FrameRateOverride.Hz144)
            res = 144;
        else if (fr is FrameRateOverride.Hz240)
            res = 240;
        else if (fr is FrameRateOverride.Unlimited)
            res = 999;
        return res;
    }

    public static int GetVSyncValue(VSyncOverride vsv)
    {
        var res = 1;
        if (vsv is VSyncOverride.Off)
            res = 0;
        else if (vsv is VSyncOverride.Full)
            res = 1;
        else if (vsv is VSyncOverride.Half)
            res = 2;
        else if (vsv is VSyncOverride.Third)
            res = 3;
        return res;
    }

    /*
#if UNITY_EDITOR
    [CustomEditor(typeof(AutoResolution))]
    public class AutoResolutionEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();

            AutoResolution tgt = (AutoResolution)target;

            GUILayout.BeginHorizontal();
            EditorGUIUtility.labelWidth = 15;
            EditorGUILayout.LabelField("Frame count smoothing:");
            //GUILayout.Space(7);
            smoothing = GUILayout.Toolbar(;
            GUILayout.EndHorizontal();
        }
    }
#endif
    */
}
