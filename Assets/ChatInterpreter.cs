using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChatInterpreter : MonoBehaviour
{
    string text = string.Empty;
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
        text += ch;
        if(ch == 'Q')
        {
            Debug.Log("Output: " + text);
            text = string.Empty;
        }
    }
}
