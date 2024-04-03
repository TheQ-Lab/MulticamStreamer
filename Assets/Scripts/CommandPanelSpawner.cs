using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandPanelSpawner : MonoBehaviour
{
    List<CommandVisRisingPanel> listCommandPanels = new();

    // Start is called before the first frame update
    void Start()
    {
        var blueprint = GetComponentInChildren<CommandVisRisingPanel>();
        listCommandPanels.Add(blueprint);
        for (int i = 0; i < 10; i++)
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
}
