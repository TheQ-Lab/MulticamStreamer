using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBExtensions;

public class ViewportHeader : MonoBehaviour
{
    public CamGridAgent CamGridAgent;

    private AnimEngine animEngineHeader, animEngineKeySymbol;
    // Start is called before the first frame update
    void Start()
    {
        TryGetComponent(out animEngineHeader);
        StartCoroutine(this.DelayedExecution(() =>
        {
            animEngineKeySymbol = TooManyFuncts.GetComponentInChildrenParametric<AnimEngine>(transform, null, null, null);
        }, new WaitForEndOfFrame()));
        //animEngineKeySymbol = TooManyFuncts.GetComponentInChildrenParametric<AnimEngine>(transform, "KEY", null, null);
        if (animEngineHeader is null)
            throw new System.EntryPointNotFoundException();
        animEngineHeader.AnimFadeDuration = 0.3f;

        //animEngineHeader.TriggerAnimFade(true);
        CamGridAgent.swapEvent += (start, camName) => 
        { 
            animEngineHeader.TriggerAnimFade(start);
            if (start)
            {
                animEngineKeySymbol.CancelInvoke();
                animEngineKeySymbol.stopAllAnimations = true;
                //animEngineKeySymbol.StopAllCoroutines();
            }
            else if (!start)
                animEngineKeySymbol.SchedulePulse();
            Debug.Log("REEEE" + camName); 
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
