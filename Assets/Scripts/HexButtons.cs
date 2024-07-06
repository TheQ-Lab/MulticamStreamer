using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexButtons : MonoBehaviour
{
    public static HexButtons Instance;
    private void Awake()
    {
        TooManyFuncts.Singletonize(ref Instance, this, false);
    }

    public GameObject HexBtnR, HexBtnG, HexBtnB;
    Dictionary<InputSimulatorScript.GravitraxConnex.cmds, ColorBtn> buttonDict = new();

    private void Start()
    {
        var btn = new ColorBtn(HexBtnR);
        buttonDict.Add(InputSimulatorScript.GravitraxConnex.cmds.red, btn);
        btn = new ColorBtn(HexBtnG);
        buttonDict.Add(InputSimulatorScript.GravitraxConnex.cmds.green, btn);
        btn = new ColorBtn(HexBtnB);
        buttonDict.Add(InputSimulatorScript.GravitraxConnex.cmds.blue, btn);

    }

    public ColorBtn GetCorrespondingButton(InputSimulatorScript.GravitraxConnex.cmds color)
    {
        return buttonDict[color];
    }

    public void IterateBtnProg(InputSimulatorScript.GravitraxConnex.cmds color)
    {
        ColorBtn btn = buttonDict[color];
        btn.currentValue = Mathf.Clamp(btn.currentValue + 1, 0, Constants.I.votesNeededBoth);
        var perc = (float) btn.currentValue / Constants.I.votesNeededBoth;
        btn.rim.fillAmount = perc;
        // Debug.Log("currentAmount: " + btn.currentValue + " fill perc: " + perc);
    }

    public class ColorBtn
    {
        public GameObject gameObj;
        public Image rim;
        public int currentValue;
        public ColorBtn(GameObject obj)
        {
            gameObj = obj;
            rim = TooManyFuncts.GetComponentInChildrenParametric<Image>(obj.transform, "rim", null, null);
            currentValue = 0;
        }
    }
}
