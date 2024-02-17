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
        StartCoroutine(this.DelayedExecution(delegate { InitializeVote(5); }, 1.0f));
        //StartCoroutine(this.DelayedExecution(delegate { AddVotes(1, 2); }, 5.0f));
    }

    public static void InitializeVote(int maxVotes)
    {
        foreach(UIVoteButton scr in Instance.voteButtons)
        {
            var gridAgent = CamGridHandler.Instance.CamGridAgents[Instance.voteButtons.IndexOf(scr)];
            scr.InitializeVote(gridAgent, maxVotes);
        }
        //VoteManager.maxVotes = maxVotes;

        voteOpen.Set();
    }

    public static void AddVotes(int camNo, int votesDelta)
    {
        CamGridHandler.PassOnVoteUpdate(camNo, votesDelta);

        var voteButton = Instance.voteButtons[camNo - 1];

        int updatedVotes = Mathf.Clamp(votesDelta + voteButton.currentVotes, 0, voteButton.maxVotes);
        voteButton.UpdateVote(updatedVotes);

        Debug.Log(updatedVotes + " of " + voteButton.maxVotes);

        if (updatedVotes == voteButton.maxVotes)
        {
            //CamGridHandler.Instance.SwapTriggered(camNo);
            Instance.StartCoroutine(Instance.DelayedExecution(delegate { CamGridHandler.Instance.SwapTriggered(camNo); }, 1.0f));
        }
    }
}
