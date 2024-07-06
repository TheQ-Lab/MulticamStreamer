using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBExtensions;

public class VoteManager : MonoBehaviour
{
    public static Flag voteOpen = new();
    //public static int maxVotes;

    public List<UIVoteButton> voteButtons;

    public static VoteManager Instance;
    private void Awake()
    {
        TooManyFuncts.Singletonize(ref Instance, this, false);
        var canvasUI = GameObject.FindGameObjectWithTag("CanvasUI").transform;
        voteButtons = TooManyFuncts.GetComponentsInChildrenParametric<UIVoteButton>(canvasUI, "Vote", null, null); //technically gets 2nd children
    }

    private void Start()
    {
        /*
        foreach (UIVoteButton scr in Instance.voteButtons)
        {
            Debug.Log("beep");
            var gridAgent = CamGridHandler.Instance.CamGridAgents[Instance.voteButtons.IndexOf(scr)];
            Debug.Log("woop");
            scr.SetupVoteBtn(gridAgent);
        }*/
        // TEMP
        StartCoroutine(this.DelayedExecution(delegate { InitializeVote(Constants.I.votesNeededBoth); }, Constants.I.votesDelay));
        //StartCoroutine(this.DelayedExecution(delegate { AddVotes(1, 2); }, 5.0f));
    }

    public static void InitializeVote(int maxVotes)
    {
        int index = 0;
        foreach(UIVoteButton scr in Instance.voteButtons)
        {
            var gridAgent = CamGridHandler.Instance.CamGridAgents[index];
            if(CamGridHandler.Instance.currentFullScreenCam == gridAgent)
                gridAgent = CamGridHandler.Instance.CamGridAgents[++index];
            //var gridAgent = CamGridHandler.Instance.CamGridAgents[Instance.voteButtons.IndexOf(scr)];
            gridAgent.voteBtn = scr;
            scr.InitializeVote(gridAgent, maxVotes);
            index++;
        }
        //VoteManager.maxVotes = maxVotes;

        voteOpen.Set();
    }

    public static void AddVotes(int camNo, int votesDelta)
    {
        var tgtCam = CamGridHandler.Instance.GetCam(camNo, votesDelta);

        //var voteButton = Instance.voteButtons[camNo - 1];
        var voteButton = tgtCam.voteBtn;

        // This would mean, that the vote requested is for the cam in Fullscreen position - which has no button assigned
        if (voteButton == null)
            return;

        int updatedVotes = Mathf.Clamp(votesDelta + voteButton.currentVotes, 0, voteButton.maxVotes);
        voteButton.UpdateVote(updatedVotes);

        Debug.Log(updatedVotes + " of " + voteButton.maxVotes + " votes cast");

        if (updatedVotes == voteButton.maxVotes)
        {
            //CamGridHandler.Instance.SwapTriggered(camNo);
            /*foreach(UIVoteButton v in Instance.voteButtons)
            {
                if (v.Equals(voteButton)) continue;
                v.DismissPanel();
            }*/

            foreach (CamGridAgent cam in CamGridHandler.Instance.CamGridAgents)
                cam.voteBtn = null;

            Instance.StartCoroutine(Instance.DelayedExecution(delegate { CamGridHandler.Instance.SwapTriggered(camNo); }, 1.0f));
            Instance.StartCoroutine(Instance.DelayedExecution(delegate { InitializeVote(Constants.I.votesNeededBoth); }, Constants.I.votesDelay));
        }
    }

    public static void DismissAllPanels()
    {
        foreach (UIVoteButton v in Instance.voteButtons)
        {
            //if (v.Equals(voteButton)) continue;
            v.DismissPanel();
        }
    }
}
