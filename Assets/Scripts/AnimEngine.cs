using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AnimEngine : MonoBehaviour
{
    //private List<float> AlphaStorage;
    public List<Image> StorageImg;
    public List<Color> StorageImgColors;
    public List<TextMeshProUGUI> StorageTMP;
    public List<Color> StorageTMPColors;
    [Tooltip("You know what this one is for. It's safer this way. Trust me.")]
    public int recursionSanityCounter = 0;

    public bool stopAllAnimations = false;

    // Start is called before the first frame update
    void Start()
    {
        SetupAnimFade();

        
        //StartCoroutine(AnimFade(false, AnimFadeDuration));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SchedulePulse()
    {
        InvokeRepeating(nameof(TriggerPulse), 1f, 1f);
    }

    public void TriggerPulse()
    {
        if(isActiveAndEnabled)
            StartCoroutine(AnimPulse(0.7f));
    }

#nullable enable
#nullable disable
    [Tooltip("Set the factor for the scale at the end of the anim | Used only in AnimPulse")]
    public float AnimPulseEndScale = 1.2f;
    private IEnumerator AnimPulse(float duration)
    {
        float startTime = Time.time;
        float endTime = Time.time + duration;

        Vector2 startScl = (Vector2) transform.localScale;
        Vector2 endScl = (Vector2) transform.localScale * AnimPulseEndScale;
        Vector2 currentScl = new();

        StartCoroutine(AnimFade(true, duration));

        yield return null;
        stopAllAnimations = false;
        float animPercentage = 0f;
        while (Time.time < endTime && !stopAllAnimations)
        {
            animPercentage = TooManyFuncts.Remap(Time.time, startTime, endTime, 0f, 1f);
            currentScl.x = TooManyFuncts.Remap(animPercentage, 0f, 1f, startScl.x, endScl.x);
            currentScl.y = TooManyFuncts.Remap(animPercentage, 0f, 1f, startScl.y, endScl.y);

            transform.localScale = currentScl;
            yield return null;
        }
        /*
        animPercentage = 1f;
        currentScl.x = TooManyFuncts.Remap(animPercentage, 0f, 1f, startScl.x, endScl.x);
        currentScl.y = TooManyFuncts.Remap(animPercentage, 0f, 1f, startScl.y, endScl.y);
        transform.localScale = currentScl;
        yield return null;
        */

        transform.localScale = startScl;
    }

    public float AnimFadeDuration = 0.7f;
    private IEnumerator AnimFade(bool isOut, float duration)
    {
        float startTime = Time.time;
        float endTime = Time.time + duration;

        yield return null;
        stopAllAnimations = false;
        float animPercentage = 0f;
        while(Time.time < endTime && !stopAllAnimations)
        {
            if (isOut is true)
                animPercentage = TooManyFuncts.Remap(Time.time, startTime, endTime, 1f, 0f);
            else if (isOut is false)
                animPercentage = TooManyFuncts.Remap(Time.time, startTime, endTime, 0f, 1f);
            SetAllComponentsToAlphaPercentage(animPercentage);
            yield return null;
        }
        if(isOut)   animPercentage = 0f;
        else        animPercentage = 1f;
        SetAllComponentsToAlphaPercentage(animPercentage);

        void SetAllComponentsToAlphaPercentage(float alphaPercentage)
        {
            for (int i = 0; i < StorageImg.Count; i++)
            {
                Color col = StorageImg[i].color;
                var newAlpha = TooManyFuncts.Remap(alphaPercentage, 0f, 1f, 0f, StorageImgColors[i].a);
                StorageImg[i].color = new(col.r, col.g, col.b, newAlpha);
            }
            for (int i = 0; i < StorageTMP.Count; i++)
            {
                Color col = StorageTMP[i].color;
                var newAlpha = TooManyFuncts.Remap(alphaPercentage, 0f, 1f, 0f, StorageTMPColors[i].a);
                StorageTMP[i].color = new(col.r, col.g, col.b, newAlpha);
            }
        }
    }

    public void TriggerAnimFade(bool isOut)
    {
        if (!isActiveAndEnabled)
            return;
        CancelInvoke();
        StopAllCoroutines();
        StartCoroutine(AnimFade(isOut, AnimFadeDuration));
    }

    private void SetupAnimFade()
    {
        CrawlForColoredComponents(out StorageImg, out StorageTMP);
        SaveComponentColors();
    }

    private void CrawlForColoredComponents(out List<Image> _StorageImg, out List<TextMeshProUGUI> _StorageTMP)
    {
        //AlphaStorage = new();
        List<Image> __StorageImg = new();
        List<TextMeshProUGUI> __StorageTMP = new();

        Recurse(transform);

        _StorageImg = __StorageImg;
        _StorageTMP = __StorageTMP;

        void Recurse(Transform t)
        {
            IdentifyComponentMatches(t);
            recursionSanityCounter++;
            foreach (Transform c in t)
            {
                Recurse(c);
            }
        }

        void IdentifyComponentMatches(Transform t)
        {
            t.TryGetComponent<Image>(out var image);
            if (image is not null)
                __StorageImg.Add(image);
            t.TryGetComponent<TextMeshProUGUI>(out var tmpUI);
            if (tmpUI is not null)
                __StorageTMP.Add(tmpUI);
        }
    }

    private void SaveComponentColors()
    {
        StorageImgColors = new();
        StorageTMPColors = new();

        foreach (var c in StorageImg)
            StorageImgColors.Add(c.color);

        foreach (var c in StorageTMP)
            StorageTMPColors.Add(c.color);
    }

}
