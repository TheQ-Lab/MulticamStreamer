using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamGridHandler : MonoBehaviour
{
    [Serializable]
    public class CamFrameSlot
    {
        public Vector3 position;
        public Vector3 scale;
        public CamGridAgent currentlyOccupiedBy;

        public CamFrameSlot(Vector3 position, Vector3 scale, CamGridAgent currentlyOccupiedBy)
        {
            this.position = position;
            this.scale = scale;
            this.currentlyOccupiedBy = currentlyOccupiedBy;
        }
    }
    public static CamGridHandler Instance;
    public List<CamFrameSlot> CamFrameSlots = new ();
    public List<CamGridAgent> CamGridAgents = new ();
    //public Dictionary<CamGridAgent, CamFrameTransform> SlotList;
    public CamGridAgent currentFullScreenCam;

    private void Awake()
    {
        TooManyFuncts.Singletonize(ref Instance, this, false);

        List<Transform> list = TooManyFuncts.GetChildrenParametric(transform, null, null, true);
        for (int i = 0; i < list.Count; i++)
        {
            Transform t = list[i];
            if (t.TryGetComponent<CamGridAgent>(out CamGridAgent agent))
            {
                CamFrameSlot cft = new(t.position, t.localScale, agent);
                //agent.CamFrameSlot = cft;
                agent.CamFrameSlotIndex = i;
                CamFrameSlots.Add(cft);
                CamGridAgents.Add(agent);

                // Final one is the Full screen
                currentFullScreenCam = agent;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //InputManager.Instance.inSwapAnim.setEvent += SwapTriggered;

        
    }

    public void SwapTriggered(int tgtCamNo)
    {
        //Debug.LogWarning("boop");

        //int swapTgt = InputManager.Instance.currentSwapAnimTgt;
        CamGridAgent swapTgtAgent = CamGridAgents[tgtCamNo-1];
        if (swapTgtAgent == currentFullScreenCam)
            return;

        InputManager.Instance.inSwapAnim.Set();

        StartCoroutine(SwapTriggeredRoutine(swapTgtAgent));
        IEnumerator SwapTriggeredRoutine(CamGridAgent swapTgtAgent)
        {
            SfxManager.Instance.PlaySfx(SfxManager.Sfx.CameraSwap);

            swapTgtAgent.StartCoroutine(swapTgtAgent.LaunchAnim(currentFullScreenCam.CamFrameSlotIndex));
            yield return currentFullScreenCam.StartCoroutine(currentFullScreenCam.LaunchAnim(swapTgtAgent.CamFrameSlotIndex));
/*
            List<CamAssignAgent> lst = new();
            lst.Add(swapTgtAgent.assignAgent);
            lst.Add(currentFullScreenCam.assignAgent);
            CamAssignHandler.Instance.ApplyCameraResizes(lst); // necessary? the last 4 lines?
*/
            SfxManager.Instance.PlaySfx(SfxManager.Sfx.CameraSwapCompleted);
            CamFrameSlots[currentFullScreenCam.CamFrameSlotIndex].currentlyOccupiedBy = currentFullScreenCam;
            CamFrameSlots[swapTgtAgent.CamFrameSlotIndex].currentlyOccupiedBy = swapTgtAgent;

            currentFullScreenCam = swapTgtAgent;
            InputManager.Instance.inSwapAnim.Reset();
        }
    }

    public CamGridAgent GetCam(int camNo, int votesDelta)
    {
        CamGridAgent voteTgt = CamGridAgents[camNo - 1];
        return voteTgt;
    }

}
