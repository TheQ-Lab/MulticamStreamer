using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Label : MonoBehaviour
{
    WorldSpTracker tracker;
    TextMeshPro tmp;

    private void Awake()
    {
        tracker = GetComponent<WorldSpTracker>();
        tmp = GetComponentInChildren<TextMeshPro>();
    }

    public void SwapStarted()
    {
        tracker.enabled = false;
        StartCoroutine(Animation(false));
    }

    public void SwapEnded()
    {
        tracker.enabled = true;
        StartCoroutine(Animation(true));
    }

    IEnumerator Animation(bool show)
    {
        float duration = 0.5f;
        if (show is true)
            duration = 0.4f;
        var t = 0f;
        Color c = tmp.color;

        float progress, smoothed;
        while (t < duration)
        {
            progress = TooManyFuncts.Remap(t, 0f, duration, 0f, 1f);
                
            smoothed = Mathf.Pow(progress, 3f);
            if (show is false)
                smoothed = -(smoothed-1);
            c.a = smoothed;
            tmp.color = c;

            yield return null;
            t += Time.deltaTime;
        }

        if (show is true)
            c.a = 1f;
        if (show is false)
            c.a = 0f;
        tmp.color = c;

        Debug.Log("DoneAnimatin");
    }
}
