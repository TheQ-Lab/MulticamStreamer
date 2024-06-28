using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EasInInator : MonoBehaviour
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
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Init()
    {
        CrawlForColoredComponents();
        SaveComponentColors();
        StartCoroutine(FadeOut(true));
    }

#nullable enable
#nullable disable

    private IEnumerator FadeOut(bool isOut)
    {
        float startTime = Time.time;
        float endTime = Time.time + 0.7f;

        while(Time.time < endTime)
        {
            float animPercentage = 0f;
            if (isOut is true)
                animPercentage = TooManyFuncts.Remap(Time.time, startTime, endTime, 1f, 0f);
            else if (isOut is false)
                animPercentage = TooManyFuncts.Remap(Time.time, startTime, endTime, 0f, 1f);
            SetAllComponentsToAlphaPercentage(animPercentage);
            yield return null;
        }
        SetAllComponentsToAlphaPercentage(0f);

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
