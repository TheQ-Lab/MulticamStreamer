using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        /*
        var keyboard = InputSystem.AddDevice<Keyboard>();
        //SetKeyboardLayout("QWERTY", keyboard);
        string text = "qwerty";

        //ACT
        foreach (char c in text)
        {
            Key key = (Key)System.Enum.Parse(typeof(Key), c.ToString().ToUpper());
            //Press(keyboard[key]);
            var state = new KeyboardState(key);
            state.Press(key);
            InputSystem.QueueStateEvent(keyboard, );

            InputSystem.Update();
            //yield return null;
            //Release(keyboard[key]);
            //yield return null;
        }
        */
        //InvokeRepeating(nameof(StartTestPress), 0.1f, 2f);
    }
    void StartTestPress()
    {
        StartCoroutine(TestPress());
    }

    private IEnumerator TestPress()
    {
        //var keyboard = InputSystem.AddDevice<Keyboard>();
        var keyboard = InputSystem.GetDevice<Keyboard>();
        char c = 'm';


        Key key = (Key)System.Enum.Parse(typeof(Key), c.ToString().ToUpper());
        //Press(keyboard[key]);
        Press(key, keyboard);
        //InputSystem.Update();
        Debug.Log("beep");
        Debug.Log(key.ToString());

        //yield return null;

        Release(key, keyboard);
        //InputSystem.Update();
        yield return null;
    }

    private IEnumerator TestPress2()
    {
        //var keyboard = InputSystem.AddDevice<Keyboard>();
        var keyboard = InputSystem.GetDevice<Keyboard>();
        char c = 'm';


        Key key = (Key)System.Enum.Parse(typeof(Key), c.ToString().ToUpper());
        //Press(keyboard[key]);
        Press(key, keyboard);
        //System.Windows.Forms.SendKeys.Send("{ENTER}");
        //InputSystem.Update();
        Debug.Log("beep");
        Debug.Log(key.ToString());

        //yield return null;

        Release(key, keyboard);
        //InputSystem.Update();
        yield return null;
    }

    void Press(Key key, Keyboard keyboard)
    {
        var press = new KeyboardState();
        press.Press(key);
        InputSystem.QueueStateEvent(keyboard, press);
    }

    void Release(Key key, Keyboard keyboard)
    {
        var release = new KeyboardState();
        release.Release(key);
        InputSystem.QueueStateEvent(keyboard, release);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
