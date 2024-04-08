using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CommandVisRisingPanel : MonoBehaviour
{
    [SerializeField] float durationShown = 4.0f;
    [SerializeField] Vector2 velocity = new(0f, +1f);

    //public Color red, green, blue;
    Image color;
    TMPro.TextMeshProUGUI username;

    public bool isFree;
    public bool moving;
    double time;

    //public static CommandVisRisingPanel Instance;
    private void Awake()
    {
        //TooManyFuncts.Singletonize(ref Instance, this, false);

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
                ToDeactivate();
        }
        
        if(moving)
        {
            Moving();
        }
    }

    public void ToInitialize()
    {
        gameObject.SetActive(true);

        color = TooManyFuncts.GetComponentInChildrenParametric<Image>(transform, "Color", null, null);
        username = TooManyFuncts.GetComponentInChildrenParametric<TextMeshProUGUI>(transform, "Username", null, null);
        isFree = true;
    }

    public void ToActivate()
    {

    }

    public void ToDeactivate()
    {
        gameObject.SetActive(false);
        isFree = true;
    }

    void Moving()
    {
        //Debug.LogWarning("MOVE!");
        //transform.position = transform.position + velocity;
        var rBody = GetComponent<Rigidbody2D>();
        rBody.position = rBody.position + velocity;
    }

    public void DisplayCmd(InputSimulatorScript.GravitraxConnex.cmds cmd)
    {
        if (cmd == InputSimulatorScript.GravitraxConnex.cmds.red)
            color.color = Constants.I.red;
        else if (cmd == InputSimulatorScript.GravitraxConnex.cmds.green)
            color.color = Constants.I.green;
        else if (cmd == InputSimulatorScript.GravitraxConnex.cmds.blue)
            color.color = Constants.I.blue;
        else
            return;

        username.text = string.Empty;
    }

    public void DisplayUsername(string userString)
    {
        username.text = userString + ":" + @"\u0009" + @"\u0009";
        time = durationShown;
    }

}
