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

        Invoke(nameof(RunGravitraxConnectCliScript), 1.0f);
        Invoke(nameof(RunTwitchPlaysScript), 1.1f);

        //Invoke(nameof(CloseGravitraxConnectCliScript), 15f);
        //Invoke(nameof(CloseTwitchPlaysScript), 16f);

        //inputSimulator.Keyboard.KeyDown(VirtualKeyCode.SHIFT);
        GravitraxConnex.Instance = new GravitraxConnex(this);
    }
    /*
    private void OnDestroy()
    {
        //Invoke(nameof(CloseGravitraxConnectCliScript), 15f);
        CloseTwitchPlaysScript();
    }
    */
    void PressBtn()
    {
        UnityEngine.Debug.Log("Press");
        inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_R);
        //inputSimulator.Keyboard.TextEntry("Pimmelö");
    }

    void RunTwitchPlaysScript()
    {
        string path = Application.dataPath + "/Tools/TwitchPlays/TwitchPlays_TEMPLATE.py";
        //string command = "cd " + path;
        UnityEngine.Debug.Log(path);
        System.Diagnostics.Process process = System.Diagnostics.Process.Start(path);
    }

    void RunGravitraxConnectCliScript()
    {
        string path = Application.dataPath + "/Tools/GraviTrax-Connect/Applications/CLI_Application/gravitrax_cli.py";
        //string command = "cd " + path;
        UnityEngine.Debug.Log(path);
        System.Diagnostics.Process process = System.Diagnostics.Process.Start(path);
    }

    void CloseTwitchPlaysScript()
    {
        //inputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.RSHIFT, VirtualKeyCode.BACK);
        //inputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.SHIFT, VirtualKeyCode.BACK);
        StartCoroutine(CloseTwitchPlaysScriptCoroutine());

        IEnumerator CloseTwitchPlaysScriptCoroutine()
        {
            inputSimulator.Keyboard.KeyDown(VirtualKeyCode.RSHIFT);
            inputSimulator.Keyboard.KeyDown(VirtualKeyCode.BACK);
            yield return new WaitForSeconds(0.1f);
            inputSimulator.Keyboard.KeyUp(VirtualKeyCode.RSHIFT);
            inputSimulator.Keyboard.KeyUp(VirtualKeyCode.BACK);
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
