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
    Image colorBlob;
    Image colorParticleBlob;
    ColorParticleBlob colorParticleScript;
    TMPro.TextMeshProUGUI username;

    public bool isFree;
    public bool moving;

    double time;
    Vector2 panelStartPos;
    InputSimulatorScript.GravitraxConnex.cmds colorCmd;

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
    }

    private void FixedUpdate()
    {
        if (moving)
        {
            Moving();
        }
    }

    public void ToInitialize()
    {
        gameObject.SetActive(true);

        colorBlob = TooManyFuncts.GetComponentInChildrenParametric<Image>(transform, "ColorBlob", null, null);
        colorParticleScript = TooManyFuncts.GetComponentInChildrenParametric<ColorParticleBlob>(transform, null, null, null);
        colorParticleScript.ToInit();
        colorParticleBlob = colorParticleScript.GetComponent<Image>();
        username = TooManyFuncts.GetComponentInChildrenParametric<TextMeshProUGUI>(transform, "Username", null, null);

        panelStartPos = new Vector2(this.transform.position.x, this.transform.position.y);
        isFree = true;
    }

    public void ToActivate()
    {
        transform.position = (Vector3)panelStartPos;

        moving = true;

        isFree = false;
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
        Color c;
        if (cmd == InputSimulatorScript.GravitraxConnex.cmds.red)
            c = Constants.I.red;
        else if (cmd == InputSimulatorScript.GravitraxConnex.cmds.green)
            c = Constants.I.green;
        else if (cmd == InputSimulatorScript.GravitraxConnex.cmds.blue)
            c = Constants.I.blue;
        else
            return;
        colorBlob.color = c;
        colorParticleBlob.color = c;

        c.a = 0.4f;
        colorBlob.color = c;

        colorCmd = cmd;

        username.text = string.Empty;
    }

    public void DisplayUsername(string userString)
    {
        username.text = userString + ":" + @"\u0009" + @"\u0009";
        time = durationShown;
    }


    public void FinishUsernameTransmission()
    {
        colorParticleScript.ToActivate(colorCmd);
    }
}
