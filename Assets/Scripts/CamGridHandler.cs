using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamGridHandler : MonoBehaviour
{
    [Serializable]
    public class CamFrameTransform
    {
        public Vector3 position;
        public Vector3 scale;

        public CamFrameTransform(Vector3 position, Vector3 scale)
        {
            this.position = position;
            this.scale = scale;
        }
    }

    public List<CamFrameTransform> CamFrameTransforms = new ();
    public List<CamGridAgent> CamGridAgents = new ();
    //public Dictionary<CamGridAgent, CamFrameTransform> SlotList;
    public CamGridAgent currentFullScreenCam;

    // Start is called before the first frame update
    void Start()
    {
        InputManager.Instance.inSwapAnim.setEvent += SwapTriggered;

        foreach (Transform t in TooManyFuncts.GetChildrenParametric(transform, null, null, true))
        {
            if(t.TryGetComponent<CamGridAgent>(out CamGridAgent agent))
            {
                CamFrameTransform cft = new(t.position, t.localScale);
                agent.CamFrameTransform = cft;
                CamFrameTransforms.Add(cft);
                CamGridAgents.Add(agent);
            
                // Final one is the Full screen
                currentFullScreenCam = agent;
            }
        }
    }

    protected void SwapTriggered()
    {
        int swapTgt = InputManager.Instance.currentSwapAnimTgt;
        CamGridAgent swapTgtAgent = CamGridAgents[swapTgt-1];
        if (swapTgtAgent == currentFullScreenCam)
        {
            InputManager.Instance.inSwapAnim.Reset();
            return;
        }

        swapTgtAgent.StartCoroutine(swapTgtAgent.LaunchAnim(currentFullScreenCam.CamFrameTransform));
        currentFullScreenCam.StartCoroutine(currentFullScreenCam.LaunchAnim(swapTgtAgent.CamFrameTransform));

        // yield wait for treturn blabla ...

        currentFullScreenCam = swapTgtAgent;

        InputManager.Instance.inSwapAnim.Reset();
        return;
    }



    // Update is called once per frame
    void Update()
    {
        /*SlotList.TryGetValue(new GameObject(), out CamFrameTransform t);
        t = CamFrameTransforms[0];*/
    }
}
