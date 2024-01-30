using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class FrameRateDisplay : MonoBehaviour
{
    Text text;
    static FrameRateDisplay Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            UnityEngine.Object.DontDestroyOnLoad(transform.parent.gameObject);
        }
        else if (Instance != this)
        {
            UnityEngine.Object.Destroy(transform.parent.gameObject);
        }
    }

    void Start()
    {
        text = GetComponentInChildren<Text>();
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += UltraModeRecalibrate;
        UnityEngine.SceneManagement.SceneManager.sceneUnloaded += UltraModeRecalibrate;
    }

    [HideInInspector]
    public int smoothing = 1;

    Queue<float> buffer = new();
    short bufferLength = 0;
    float probeStart = 0f;
    
    void Update()
    {

        if (smoothing is 0)
            text.text = (1 / Time.deltaTime).ToString("F0") + "fps";
        else if (smoothing is 1)
            text.text = (1 / Time.smoothDeltaTime).ToString("F0") + "fps";
        else if (smoothing is 2)
        {
            if(bufferLength <= 0)
            {
                
                if (probeStart == 0f)
                {
                    text.text = "......";
                    if (Time.timeSinceLevelLoad > .5f) probeStart = Time.time;
                    return;
                }
                text.text = ".....";
                bufferLength--;
                if (Time.time >= probeStart + .5f)
                {
                    bufferLength *= -1;
                }
                return;
            }

            buffer.Enqueue(1 / Time.deltaTime);
            if (buffer.Count < bufferLength)
            {
                text.text = "...";
            }
            else
            {
                float sum = 0f;
                foreach (float f in buffer)
                    sum += f;
                text.text = (sum / buffer.Count).ToString("F0") + "fps";
                _ = buffer.Dequeue();
            }

        }
    }
    
    public void UltraModeRecalibrate(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {   if ( mode is UnityEngine.SceneManagement.LoadSceneMode.Additive ) UltraModeRecalibrate();  }
    public void UltraModeRecalibrate(UnityEngine.SceneManagement.Scene scene)
    { UltraModeRecalibrate(); }
    public void UltraModeRecalibrate()
    {
        //Debug.Log("redo: " + Time.time + "---" + Time.timeSinceLevelLoad);
        bufferLength = 0;
        probeStart = 0f;
        buffer.Clear();
    }

    #if UNITY_EDITOR
    [CustomEditor(typeof(FrameRateDisplay))]
    public class ConversationEditor : Editor
    {
        private string[] smoothingLabels = { "Raw", "Smoothed", "Ultra" };
        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();
            serializedObject.Update();

            FrameRateDisplay tgt = (FrameRateDisplay)target;
            int smoothing = tgt.smoothing;

            GUILayout.Space(12);
            GUILayout.BeginHorizontal();
            EditorGUIUtility.labelWidth = 15;
            EditorGUILayout.LabelField("Frame count smoothing:");
            //GUILayout.Space(7);
            smoothing = GUILayout.Toolbar(smoothing, smoothingLabels);
            GUILayout.EndHorizontal();
            GUILayout.Space(8);

            if (smoothing != tgt.smoothing)
            {
                tgt.smoothing = smoothing;
                if (smoothing == 2)
                    tgt.UltraModeRecalibrate();
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(tgt);
            }
        }
    }
    #endif
}
