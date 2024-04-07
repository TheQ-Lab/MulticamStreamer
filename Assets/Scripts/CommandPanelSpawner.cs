using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandPanelSpawner : MonoBehaviour
{
    public GameObject HexBtnR, HexBtnG, HexBtnB;

    List<CommandVisRisingPanel> listCommandPanels = new();

    // Start is called before the first frame update
    void Start()
    {
        var blueprint = GetComponentInChildren<CommandVisRisingPanel>();
        listCommandPanels.Add(blueprint);
        for (int i = 0; i < Constants.I.commandPanelsPoolSize; i++)
        {
            var instance = Instantiate(blueprint.gameObject, transform, false);
            listCommandPanels.Add(instance.GetComponent<CommandVisRisingPanel>());
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSpawnNewPanel()
    {

    }

    private void IterateBtnProg(ColorBtn btn)
    {
        btn.currentValue = Mathf.Clamp(btn.currentValue +1, 0, Constants.I.votesNeededGtrxColor);
        btn.rim.fillAmount = btn.currentValue / Constants.I.votesNeededGtrxColor;
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
