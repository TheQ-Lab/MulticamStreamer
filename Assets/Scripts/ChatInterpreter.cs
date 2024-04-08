using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChatInterpreter : MonoBehaviour
{
    CommandVisRisingPanel panel;
    string text = string.Empty;
    public enum Phase { Idle, Msg, Name }
    public Phase phase = Phase.Idle;

    private InputSimulatorScript InputSimulatorScript;

    private void Start()
    {
        InputSimulatorScript = GetComponent<InputSimulatorScript>();
    }

    private void OnEnable()
    {
        Keyboard.current.onTextInput += OnTextInput;
    }

    private void OnDisable()
    {
        Keyboard.current.onTextInput -= OnTextInput;
    }

    // ref:
    // https://docs.unity3d.com/Packages/com.unity.inputsystem@1.8/manual/Keyboard.html
    // https://docs.unity3d.com/Packages/com.unity.inputsystem@1.8/api/UnityEngine.InputSystem.Keyboard.html#UnityEngine_InputSystem_Keyboard_onTextInput

    private void OnTextInput(char ch)
    {
        if(ch == ':')
        {
            Debug.Log("Listening...");
            text = string.Empty;
            phase = Phase.Msg;
            return;
        }
        else if(ch == ',')
        {
            ReferMsg(text);
            text = string.Empty;
            phase = Phase.Name;
            return;
        }
        else if(ch == '.')
        {
            ReferUsername(text);
            text = string.Empty;
            phase = Phase.Idle;
            return;
        }

        if (phase == Phase.Msg && char.IsUpper(ch))
        {
            text += ch;
        }
        else if (phase == Phase.Name && char.IsUpper(ch))
        {
            if (text.Length == 0)
                ch = char.ToLower(ch);
            else
                ch = char.ToLower(ch);

            text += ch;

            CommandVisualizer.Instance.ReceiveUsernameUpdate(text);
            //panel.DisplayUsernameUpdate(text);
        }
    }

    private void ReferMsg(string dirtyMsg)
    {
        string cmd = dirtyMsg;

        Debug.Log("Msg: " + cmd);
        InputSimulatorScript.GravitraxConnex.cmds cmdEnum;
        if (cmd == "REDCMD")
            cmdEnum = InputSimulatorScript.GravitraxConnex.cmds.red;
        else if (cmd == "GREENCMD")
            cmdEnum = InputSimulatorScript.GravitraxConnex.cmds.green;
        else if (cmd == "BLUECMD")
            cmdEnum = InputSimulatorScript.GravitraxConnex.cmds.blue;
        else
            return;

        CommandVisualizer.Instance.DisplayCmd(cmdEnum);

        panel = CommandPanelSpawner.Instance.OnSpawnNewPanel();
        panel?.DisplayCmd(cmdEnum);
        HexButtons.Instance.IterateBtnProg(cmdEnum);

        InputSimulatorScript.GravitraxConnex.Instance.RunCommand(cmdEnum);
    }

    private void ReferUsername(string dirtyUsername)
    {
        string username = dirtyUsername.ToLower();
        var firstLetter = char.ToUpper(username[0]);
        username = username.Remove(0, 1);
        username = username.Insert(0, firstLetter.ToString());

        Debug.Log("Name: " + username);

        CommandVisualizer.Instance.ReceiveUsernameUpdate(username);
        panel?.DisplayUsername(username);
        panel?.gameObject.SetActive(true);
    }
}
