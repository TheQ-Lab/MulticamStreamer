using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimPulseInstantiator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var obj = Instantiate(gameObject, transform);
        Destroy(obj.GetComponent<AnimPulseInstantiator>());     // prevent recursive instantiating
        obj.transform.localScale = Vector3.one;                 // resize child to be as big as parent

        var animEngine = obj.AddComponent<AnimEngine>();
        //animEngine.InvokeRepeating(nameof(animEngine.TriggerPulse), 1f, 1f);
        animEngine.SchedulePulse();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
