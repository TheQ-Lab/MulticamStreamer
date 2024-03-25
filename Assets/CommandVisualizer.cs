using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CommandVisualizer : MonoBehaviour
{
    public Color red, green, blue;
    Image color;
    TMPro.TextMeshProUGUI username;

    double time;
    public static CommandVisualizer Instance;
    private void Awake()
    {
        TooManyFuncts.Singletonize(ref Instance, this, false);
        color = TooManyFuncts.GetComponentInChildrenParametric<Image>(transform, "Color", null, null);
        username = TooManyFuncts.GetComponentInChildrenParametric<TextMeshProUGUI>(transform, "Username", null, null);
        gameObject.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (time > 0d)
        {
            time -= Time.deltaTime;
            if (time <= 0)
                gameObject.SetActive(false);
        }
    }

    public void ReceiveUsernameUpdate(string userString)
    {
        username.text = userString + ":" + @"\u0009" + @"\u0009";
        time = 4.0f;
    }

    public void ReceiveCmd(InputSimulatorScript.GravitraxConnex.cmds cmd)
    {
        if (cmd == InputSimulatorScript.GravitraxConnex.cmds.red)
            color.color = red;
        else if (cmd == InputSimulatorScript.GravitraxConnex.cmds.green)
            color.color = green;
        else if (cmd == InputSimulatorScript.GravitraxConnex.cmds.blue)
            color.color = blue;
        else
            return;

        gameObject.SetActive(true);
        username.text = string.Empty;
    }
}
