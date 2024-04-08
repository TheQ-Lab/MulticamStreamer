using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandPanelSpawner : MonoBehaviour
{
    List<CommandVisRisingPanel> listCommandPanels = new();

    public static CommandPanelSpawner Instance;
    private void Awake()
    {
        TooManyFuncts.Singletonize(ref Instance, this, false);
    }

    Vector2 panelStartPos;
    // Start is called before the first frame update
    void Start()
    {
        var blueprint = TooManyFuncts.GetComponentInChildrenParametric<CommandVisRisingPanel>(transform, null, null, null);
        panelStartPos = new Vector2(blueprint.transform.position.x, blueprint.transform.position.y);
        Debug.Log(blueprint);
        listCommandPanels.Add(blueprint);
        for (int i = 0; i < Constants.I.commandPanelsPoolSize; i++)
        {
            var instance = Instantiate(blueprint.gameObject, transform, false);

            instance.TryGetComponent(out CommandVisRisingPanel panel);
            listCommandPanels.Add(panel);

            panel.ToInitialize();
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public CommandVisRisingPanel OnSpawnNewPanel()
    {
        foreach(CommandVisRisingPanel panel in listCommandPanels)
        {
            if(panel.isFree)
            {
                //panel.gameObject.SetActive(true);
                panel.transform.position = (Vector3) panelStartPos;

                panel.moving = true;

                panel.isFree = false;
                return panel;
            }
        }
        Debug.LogWarning("No free panel found! Consider increasing pool size.");
        return null;
    }

}
