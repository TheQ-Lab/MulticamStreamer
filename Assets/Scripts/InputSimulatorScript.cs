using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using WindowsInput;
using WindowsInput.Native;

public class InputSimulatorScript : MonoBehaviour
{
    public static InputSimulator inputSimulator;
    // Start is called before the first frame update
    void Start()
    {
        inputSimulator = new InputSimulator();
        //InvokeRepeating(nameof(PressBtn), 5f, 2f);

        UnityEngine.Debug.LogError(Application.dataPath);

        Invoke(nameof(RunGravitraxConnectCliScript), 1.0f);
        Invoke(nameof(RunTwitchPlaysScript), 1.1f);

        //Invoke(nameof(ShutownProgram), 8f);

        //Invoke(nameof(CloseGravitraxConnectCliScript), 15f);
        //Invoke(nameof(CloseTwitchPlaysScript), 16f);

        //inputSimulator.Keyboard.KeyDown(VirtualKeyCode.SHIFT);
        GravitraxConnex.Instance = new GravitraxConnex(this);

        Application.wantsToQuit += OnShutdown;
    }
    /*
    private void OnDestroy()
    {
        //Invoke(nameof(CloseGravitraxConnectCliScript), 15f);
        CloseTwitchPlaysScript();
    }
    */
    bool isShutdownRequested, isShutdownProcessesFinished;
    bool OnShutdown()
    {
        if (isShutdownProcessesFinished)
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #endif
            return true;
        }
        if (isShutdownRequested)
            return false;

        UnityEngine.Debug.LogError("Quit MSG received");

        CloseGravitraxConnectCliScript();
        CloseTwitchPlaysScript();

        InvokeRepeating(nameof(RetryQuit), 0.5f, 1.0f);
        isShutdownRequested = true;

        return false;
    }

    void RetryQuit()
    {
        UnityEngine.Debug.LogError("Retry Quit..");
        #if UNITY_EDITOR
            OnShutdown();
        #else
            Application.Quit();
        #endif
    }

    public void ShutownProgram()
    {
        //UnityEngine.Debug.LogError("Inducing Quit...");
        #if UNITY_EDITOR
            //UnityEditor.EditorApplication.isPlaying = false;
            OnShutdown();
        #else
            Application.Quit();
        #endif
    }

    void PressBtn()
    {
        UnityEngine.Debug.Log("Press");
        inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_R);
        //inputSimulator.Keyboard.TextEntry("Pimmelö");
    }

    float startTimeTwitch;
    void RunTwitchPlaysScript()
    {
        string path = Application.dataPath + "/Tools/TwitchPlays/TwitchPlays_TEMPLATE.py";
        //string command = "cd " + path;
        UnityEngine.Debug.Log(path);
        System.Diagnostics.Process process = System.Diagnostics.Process.Start(path);

        startTimeTwitch = Time.unscaledTime;
    }

    float startTimeGravitrax;
    void RunGravitraxConnectCliScript()
    {
        string path = Application.dataPath + "/Tools/GraviTrax-Connect/Applications/CLI_Application/gravitrax_cli.py";
        //string command = "cd " + path;
        UnityEngine.Debug.Log(path);
        System.Diagnostics.Process process = System.Diagnostics.Process.Start(path);

        startTimeGravitrax = Time.unscaledTime;
    }

    void CloseTwitchPlaysScript()
    {
        //UnityEngine.Debug.Log("now " + Time.unscaledTime);
        //UnityEngine.Debug.Log(startTimeTwitch + 8f);
        /*
        if (Time.unscaledTime < startTimeTwitch + 8f) // if not at least 5+3s since start of TwitchCli have passed, twitchcli is at risk of not registering Quit-Input
        {
            UnityEngine.Debug.LogWarning("Halt Stopp!");
            return;
            UnityEngine.Debug.LogWarning("Dem isch ja illegal!!!11!");
        }
        */
        StartCoroutine(CloseTwitchPlaysScriptCoroutine());

        IEnumerator CloseTwitchPlaysScriptCoroutine()
        {
            yield return new WaitWhile(() => (Time.unscaledTime < startTimeTwitch + 8f));

            //inputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.RSHIFT, VirtualKeyCode.BACK);
            //UnityEngine.Debug.Log("Press RShift");
            inputSimulator.Keyboard.KeyDown(VirtualKeyCode.RSHIFT);
            inputSimulator.Keyboard.KeyDown(VirtualKeyCode.BACK);
            yield return new WaitForSeconds(0.1f);
            //UnityEngine.Debug.Log("De-Press RShift");
            inputSimulator.Keyboard.KeyUp(VirtualKeyCode.RSHIFT);
            inputSimulator.Keyboard.KeyUp(VirtualKeyCode.BACK);
            //yield return new WaitForSeconds(0.05f);
            isShutdownProcessesFinished = true;
        }
    }

    void CloseGravitraxConnectCliScript()
    {
        inputSimulator.Keyboard.KeyPress(VirtualKeyCode.ESCAPE);
    }

    public class GravitraxConnex
    {
        public static GravitraxConnex Instance;
        public Dictionary<cmds, Action> Commands = new();
        public enum cmds { red, green, blue };
        public InputSimulator inputSimulator;

        public GravitraxConnex(InputSimulatorScript parent)
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
                return;

            inputSimulator = new InputSimulator();

            Commands.Add(cmds.red, 
                delegate { inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_R); }
                );
            Commands.Add(cmds.green,
                delegate { inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_G); }
                );
            Commands.Add(cmds.blue,
                delegate { inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_B); }
                );

            //parent.InvokeRepeating(nameof(KeepAwake), 10f, 30f);
        }

        public void RunCommand(cmds cmd)
        {
            Action a = Commands[cmd];
            a();
        }

        public void KeepAwake()
        {
            inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_R);
        }

    }
}
