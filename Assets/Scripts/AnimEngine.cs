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
    public int counter = 0;

    // Start is called before the first frame update
    void Start()
    {
        SetupAnimFade();

        
        //StartCoroutine(AnimFade(false, FadeDuration));
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

    private IEnumerator AnimPulse(float duration)
    {
        float startTime = Time.time;
        float endTime = Time.time + duration;

        Vector2 startScl = (Vector2) transform.localScale;
        Vector2 endScl = (Vector2) transform.localScale * 1.2f;
        Vector2 currentScl = new();

        StartCoroutine(AnimFade(true, duration));

        float animPercentage = 0f;
        while (Time.time < endTime)
        {
            animPercentage = TooManyFuncts.Remap(Time.time, startTime, endTime, 0f, 1f);
            currentScl.x = TooManyFuncts.Remap(animPercentage, 0f, 1f, startScl.x, endScl.x);
            currentScl.y = TooManyFuncts.Remap(animPercentage, 0f, 1f, startScl.y, endScl.y);

            transform.localScale = currentScl;
            yield return null;
        }
        animPercentage = 1f;
        currentScl.x = TooManyFuncts.Remap(animPercentage, 0f, 1f, startScl.x, endScl.x);
        currentScl.y = TooManyFuncts.Remap(animPercentage, 0f, 1f, startScl.y, endScl.y);
        transform.localScale = currentScl;

        yield return null;

        transform.localScale = startScl;
    }

    const float FadeDuration = 0.7f;
    private IEnumerator AnimFade(bool isOut, float duration)
    {
        float startTime = Time.time;
        float endTime = Time.time + duration;

        float animPercentage = 0f;
        while(Time.time < endTime)
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

    private void SetupAnimFade()
    {
        CrawlForColoredComponents();
        SaveComponentColors();
    }

    private void CrawlForColoredComponents()
    {
        //AlphaStorage = new();
        StorageImg = new();
        StorageTMP = new();

        Recurse(transform);

        void Recurse(Transform t)
        {
            IdentifyComponentMatches(t);
            counter++;
            foreach (Transform c in t)
            {
                Recurse(c);
            }
        }

        void IdentifyComponentMatches(Transform t)
        {
            t.TryGetComponent<Image>(out var image);
            if (image is not null)
                StorageImg.Add(image);
            t.TryGetComponent<TextMeshProUGUI>(out var tmpUI);
            if (tmpUI is not null)
                StorageTMP.Add(tmpUI);
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
