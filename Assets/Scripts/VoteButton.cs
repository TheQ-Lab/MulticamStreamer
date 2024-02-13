using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MBExtensions;
using System;

public class VoteButton : MonoBehaviour
{
    public int currentVotes = 0;
    public int maxVotes = 5;

    private List<Coroutine> coroutines = new();

    private Animator animator;
    //private Button button;
    private Image circleBar; //arrow;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        //button = GetComponent<Button>();

        circleBar = TooManyFuncts.GetComponentInChildrenParametric<Image>(transform, "Bar", null, null);
        //arrow = TooManyFuncts.GetComponentInChildrenParametric<Image>(transform, "Arrow", null, null);
    }

    public void InitializeVote(int maxVotes)
    {
        currentVotes = 0;

        //Spawn Btn as Square initially
    }

    public void UpdateVote(int updatedVotes)
    {
        updatedVotes = Mathf.Clamp(updatedVotes, 0, maxVotes);

        if (currentVotes == 0 && updatedVotes > 0)
        {
            //Transform Btn to circle at first vote cast for it
            animator.Play("Fill", 0);

            //float GetAnimLength(int layer) => animator.GetCurrentAnimatorStateInfo(layer).length;
            void DelayedAnimation() 
            {
                var len = animator.GetCurrentAnimatorStateInfo(0).length;
                StartCoroutine(this.DelayedExecution(StartAnimation, new WaitForSeconds(len))); 
            }
            StartCoroutine(this.DelayedExecution(DelayedAnimation, new WaitForEndOfFrame()));
        }
        else
        {
            StartAnimation();
        }

        // Update circle around accordingly soft
        void StartAnimation() => StartCoroutine(Animation()); 
        IEnumerator Animation()
        {
            float duration = .5f;
            var t = 0f;

            Debug.Log("StartAnimatin");
            while (t < duration)
            {
                //float x = TooManyFuncts.Remap(t, 0f, duration, 0f, Mathf.PI * .5f);
                //float segmentProgPerc = Mathf.Sin(x);
                float x = TooManyFuncts.Remap(t, 0f, duration, Mathf.PI, Mathf.PI * 2f);
                float segmentProgPerc = (Mathf.Cos(x) + 1f) * 0.5f;

                float segmentProgAbs = TooManyFuncts.Remap(segmentProgPerc, 0f, 1f, currentVotes, updatedVotes);
                float circleProg = TooManyFuncts.Remap(segmentProgAbs, 0f, maxVotes, 0f, 1f);
                Debug.Log(circleProg);
                circleBar.fillAmount = circleProg;
                //float x = TooManyFuncts.Remap(t, 0f, duration, 0, Mathf.PI * .5f);
                //circleBar.fillAmount = Mathf.Sin(x);
                // alternative with ease in&out
                //float x = TooManyFuncts.Remap(t, 0f, duration, Mathf.PI, Mathf.PI * 2f);
                //circleBar.fillAmount = (Mathf.Cos(x) + 1f) * 0.5f;

                yield return null;
                t += Time.deltaTime;
            }
            circleBar.fillAmount = (float)updatedVotes / (float)maxVotes;

            currentVotes = updatedVotes;
            if(updatedVotes == maxVotes)
            {
                yield return new WaitForSeconds(.5f);
                DismissPanel();
            }

            Debug.Log("DoneAnimatin");
        }
    }




    Stack<Action> lateUpdate = new();
    private void LateUpdate()
    {
        #region --old List replaced by Stack
        /*
        if (delayedExecution.Count > 0)
        {
            foreach (var a in delayedExecution)
                a.Invoke();
            delayedExecution.Clear();
        }
        */
        // new version with Stack instead of List
        #endregion
        while (lateUpdate.Count > 0)
            lateUpdate.Pop().Invoke();
    }

    private void Start()
    {
        //StartCoroutine(this.DelayedExecution(on, 1.5f));
        StartCoroutine(this.DelayedExecution(CallPanel, 1f));
        StartCoroutine(this.DelayedExecution(delegate { UpdateVote(1); }, 2.5f));
        StartCoroutine(this.DelayedExecution(delegate { UpdateVote(2); }, 4f));
        StartCoroutine(this.DelayedExecution(delegate { UpdateVote(3); }, 5.2f));
        StartCoroutine(this.DelayedExecution(delegate { UpdateVote(4); }, 6.4f));
        StartCoroutine(this.DelayedExecution(delegate { UpdateVote(5); }, 7.6f));


        KillPanel();

        /*void on()
        {
            Debug.Log("boop");
            gameObject.SetActive(true);
            animator.Play("Pop in", 1);
        }*/
    }




    public void CallPanel()
    {
        foreach (var i in TooManyFuncts.GetComponentsInChildrenParametric<Image>(transform.parent, null, null, null))
            i.enabled = true;

        gameObject.SetActive(true);
        //button.interactable = false;
        animator.Play("Pop in", 1);

        //lateUpdate.Push(readOutAnimLength);
        StartCoroutine(this.DelayedExecution(readOutAnimLength, new WaitForEndOfFrame()));
        void readOutAnimLength()
        {
            var info = animator.GetCurrentAnimatorStateInfo(1); // this needs to be checked delayed
                                                                // (after setting a new Animation to play right before)
            ////Invoke(nameof(RemoveSafetyPin), info.length);
            //StartCoroutine(PopInTransform(info.length, nextPos, currentPos));
        }
    }

    private void DismissPanel()
    {
        animator.Play("Pop out", 1);

        lateUpdate.Push(scheduleKill);
        //coroutines.Add(
        //StartCoroutine(this.DelayedExecution(scheduleKill, new WaitForEndOfFrame()));
        void scheduleKill()
        {
            var info = animator.GetCurrentAnimatorStateInfo(1); // this needs to be checked delayed
                                                                // (after setting a new Animation to play right before)
            Invoke(nameof(KillPanel), info.length);
            //UIOverlayInterface.Instance.uIFork.ReturnFork(this);
        }
    }

    public void KillPanel()
    {
        animator.Play("Idle", 0);
        animator.Play("Idle", 1);
        //animator.StopPlayback();

        foreach (Image i in TooManyFuncts.GetComponentsInChildrenParametric<Image>(transform.parent, null, null, null))
            i.enabled = false;

        //Invoke(nameof(DeactivatePanel), animator.GetCurrentAnimatorStateInfo(1).length);
        Deactivate();
    }

    private void OnEnable()
    {
        Debug.Log("enable");
        //animator.Play("Pop in", 1);
    }
    
    public void Deactivate()
    {
        //gameObject.SetActive(false);
        enabled = false;
    }
    private void OnDisable()
    {
        circleBar.fillAmount = 0f;
        foreach (var c in coroutines)
            StopCoroutine(c);
        coroutines.Clear();
    }
}
