using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimPulseInstantiator : MonoBehaviour
{
    [Tooltip("This gets passed on to customize the instabntiated AnimEngine\n --- \n" +
        "Set the factor for the scale at the end of the anim | Used only in AnimPulse")]
    public float AnimPulseEndScale = 1.4f;

    // Start is called before the first frame update
    void Start()
    {
        var obj = Instantiate(gameObject, transform, true);
        Destroy(obj.GetComponent<AnimPulseInstantiator>());     // prevent recursive instantiating
        obj.transform.localScale = Vector3.one;                 // resize child to be as big as parent

        var animEngine = obj.AddComponent<AnimEngine>();
        //animEngine.InvokeRepeating(nameof(animEngine.TriggerPulse), 1f, 1f);
        animEngine.AnimPulseEndScale = this.AnimPulseEndScale;
        animEngine.SchedulePulse();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
