using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using MBExtensions;

public class UIForkPanel : MonoBehaviour//, IPointerDownHandler, IPointerUpHandler
{
    /*
    private Vector3 trackedPosition;
    //private bool interactable = false;
    private List<Coroutine> coroutines = new();

    private Animator animator;
    private Button button;
    private Image circleBar, arrow;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        button = GetComponent<Button>();

        circleBar = TooManyFuncts.GetComponentInChildrenParametric<Image>(transform, "Bar", null, null);
        arrow = TooManyFuncts.GetComponentInChildrenParametric<Image>(transform, "Arrow", null, null);
    }

    private void Start()
    {
        KillPanel();
    }

    private void OnDisable()
    {
        circleBar.fillAmount = 0f;
        foreach (var c in coroutines)
            StopCoroutine(c);
        coroutines.Clear();
    }

    void Update()
    {
        Track();
        //Debug.Log(coroutines.Count);
    }

    private void Track()
    {
        transform.position = Camera.main.WorldToScreenPoint(trackedPosition);
        //Debug.Log(trackedPosition);
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
        *//*
        // new version with Stack instead of List
        #endregion
        while (lateUpdate.Count > 0)
            lateUpdate.Pop().Invoke();
    }

    public void CallPanel(Space potentialNext, in List<Sprite> arrowSprites)
    {
        var nextPos = potentialNext.transform.position;
        var currentPos = potentialNext.prev[0].transform.position;

        // choos correct arrow
        var angle = Vector3.SignedAngle(Vector3.right, nextPos - currentPos, Vector3.up);
        if (angle < 0f)
            angle = 360f + angle;
        var ckAng = -45f / 2;
        foreach(var dir in arrowSprites)
        {
            ckAng += 45f;
            if (angle <= ckAng)
            {
                //Debug.LogWarning(dir.name);
                arrow.sprite = dir;
                break;
            }
        }

        foreach (var i in TooManyFuncts.GetComponentsInChildrenParametric<Image>(transform.parent, null, null, null))
            i.enabled = true;

        gameObject.SetActive(true);
        button.interactable = false;
        animator.Play("Pop in", 1);

        //lateUpdate.Push(readOutAnimLength);
        StartCoroutine(this.DelayedExecution(readOutAnimLength, new WaitForEndOfFrame()));
        void readOutAnimLength()
        {
            var info = animator.GetCurrentAnimatorStateInfo(1); // this needs to be checked delayed
                                                                // (after setting a new Animation to play right before)
            //Invoke(nameof(RemoveSafetyPin), info.length);
            StartCoroutine(PopInTransform(info.length, nextPos, currentPos));
        }
    }

    IEnumerator PopInTransform(float length, Vector3 nextPos, Vector3 currentPos)
    {
        float progress;
        //length *= 3 / 4;
        for (float t = 0f; t < length; t += Time.deltaTime)
        {
            progress = TooManyFuncts.Remap(t, 0f, length, Mathf.PI, Mathf.PI*1.5f) ;
            progress = Mathf.Cos(progress)+1f;
            progress /= 2f;
            trackedPosition = Vector3.Lerp(currentPos, nextPos, progress);
            trackedPosition += 0.8f * Vector3.up;
            yield return null;
        }
        trackedPosition = Vector3.Lerp(currentPos, nextPos, 0.5f);
        trackedPosition += 0.8f * Vector3.up;

        RemoveSafetyPin();
    }

    private void RemoveSafetyPin()
    {
        button.interactable = true;
        animator.Play("Idle", 1);
    }

    void DisplayInfo()
    {
        var info = animator.GetCurrentAnimatorStateInfo(1).length;
        Debug.Log("Frame " + Time.frameCount + ": " + info);
    }


    public void DeactivatePanel()
    {
        gameObject.SetActive(false);
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        animator.Play("Fill", 0);
        coroutines.Add(StartCoroutine(CircleBarAnim(0.4f, 0.2f)));
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        animator.Play("Idle", 0);

        foreach (var c in coroutines)
            StopCoroutine(c);
        coroutines.Clear();
        circleBar.fillAmount = 0f;
    }

    private IEnumerator CircleBarAnim(float duration, float afterDelay)
    {
        var t = 0f;
        yield return new WaitForSeconds(afterDelay);

        while (t < duration)
        {
            float x = TooManyFuncts.Remap(t, 0f, duration, 0, Mathf.PI*.5f);

            circleBar.fillAmount = Mathf.Sin(x);
            // alternative with ease in&out
            //float x = TooManyFuncts.Remap(t, 0f, duration, Mathf.PI, Mathf.PI * 2f);
            //circleBar.fillAmount = (Mathf.Cos(x) + 1f) * 0.5f;

            yield return null;
            t += Time.deltaTime;
        }

        circleBar.fillAmount = 1f;

        button.interactable = false;
        DismissPanel();
    }

    private void DismissPanel()
    {
        animator.Play("Pop out", 1);

        lateUpdate.Push(scheduleKill);
        void scheduleKill()
        {
            var info = animator.GetCurrentAnimatorStateInfo(1); // this needs to be checked delayed
                                                                // (after setting a new Animation to play right before)
            Invoke(nameof(KillPanel), info.length);
            UIOverlayInterface.Instance.uIFork.ReturnFork(this);
        }
    }

    public void KillPanel()
    {
        animator.Play("Idle", 0);
        animator.Play("Idle", 1);
        //animator.StopPlayback();

        foreach (Image i in TooManyFuncts.GetComponentsInChildrenParametric<Image>(transform.parent, null, null, null))
            i.enabled = false;

        Invoke(nameof(DeactivatePanel), animator.GetCurrentAnimatorStateInfo(1).length);
    }
*/
}
