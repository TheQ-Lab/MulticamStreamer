using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBExtensions;
using TMPro;

public class ViewportHeader : MonoBehaviour
{
    //public CamGridAgent CamGridAgent;

    private AnimEngine animEngineHeader, animEngineKeySymbol;
    private TextMeshProUGUI tmpKey, tmpKeyShadow, tmpText;
    // Start is called before the first frame update
    void Start()
    {
        TryGetComponent(out animEngineHeader);
        StartCoroutine(this.DelayedExecution(() =>
        {
            animEngineKeySymbol = TooManyFuncts.GetComponentInChildrenParametric<AnimEngine>(transform, null, null, null);
            tmpKeyShadow = TooManyFuncts.GetComponentInChildrenParametric<TextMeshProUGUI>(tmpKey.transform.parent, "KEY", null, null);
            Debug.Log("namane " + tmpKey.transform.name);
        }, new WaitForEndOfFrame()));
        //animEngineKeySymbol = TooManyFuncts.GetComponentInChildrenParametric<AnimEngine>(transform, "KEY", null, null);
        if (animEngineHeader is null)
            throw new System.EntryPointNotFoundException();
        animEngineHeader.AnimFadeDuration = 0.3f;

        tmpKey = TooManyFuncts.GetComponentInChildrenParametric<TextMeshProUGUI>(transform, "KEY", null, null);
        //tmpKeyShadow = TooManyFuncts.GetComponentInChildrenParametric<TextMeshProUGUI>(tmpKey.transform, "KEY", null, null);
        tmpText = TooManyFuncts.GetComponentInChildrenParametric<TextMeshProUGUI>(transform, "TEXT", null, null);

        //animEngineHeader.TriggerAnimFade(true);

        /*CamGridAgent.swapEvent += (start, camName) => 
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
        };*/

        CamGridHandler.Instance.CamFrameSlots[transform.GetSiblingIndex()].swapEvent += swapEvent;
        //CamGridAgent.swapEvent += swapEvent;

        void swapEvent(bool start, string camName)
        {
            animEngineHeader.TriggerAnimFade(start);
            if (start)
            {
                animEngineKeySymbol.CancelInvoke();
                animEngineKeySymbol.stopAllAnimations = true;
                //animEngineKeySymbol.StopAllCoroutines();
            }
            else if (!start)
            {
                animEngineKeySymbol.CancelInvoke();
                animEngineKeySymbol.stopAllAnimations = true;

                animEngineKeySymbol?.SchedulePulse();
                tmpKey.text = camName.Substring(0,1);
                tmpKeyShadow.text = camName.Substring(0,1);
                tmpText.text = camName.Substring(1);
            }
            Debug.Log("REEEE" + camName);
            Debug.Log("me " + name);

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
