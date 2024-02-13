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
    public static CamGridHandler Instance;
    public List<CamFrameTransform> CamFrameTransforms = new ();
    public List<CamGridAgent> CamGridAgents = new ();
    //public Dictionary<CamGridAgent, CamFrameTransform> SlotList;
    public CamGridAgent currentFullScreenCam;

    private void Awake()
    {
        TooManyFuncts.Singletonize(ref Instance, this, false);
    }

    // Start is called before the first frame update
    void Start()
    {
        //InputManager.Instance.inSwapAnim.setEvent += SwapTriggered;

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

    public void SwapTriggered()
    {
        //Debug.LogWarning("boop");

        int swapTgt = InputManager.Instance.currentSwapAnimTgt;
        CamGridAgent swapTgtAgent = CamGridAgents[swapTgt-1];
        if (swapTgtAgent == currentFullScreenCam)
            return;

        InputManager.Instance.inSwapAnim.Set();

        StartCoroutine(SwapTriggeredRoutine(swapTgtAgent));
        IEnumerator SwapTriggeredRoutine(CamGridAgent swapTgtAgent)
        {

            swapTgtAgent.StartCoroutine(swapTgtAgent.LaunchAnim(currentFullScreenCam.CamFrameTransform));
            yield return currentFullScreenCam.StartCoroutine(currentFullScreenCam.LaunchAnim(swapTgtAgent.CamFrameTransform));

            List<CamAssignAgent> lst = new();
            lst.Add(swapTgtAgent.assignAgent);
            lst.Add(currentFullScreenCam.assignAgent);
            CamAssignHandler.Instance.ApplyCameraResizes(lst);

            currentFullScreenCam = swapTgtAgent;
            InputManager.Instance.inSwapAnim.Reset();
        }
    }

}
