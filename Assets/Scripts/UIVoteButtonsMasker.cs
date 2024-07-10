using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIVoteButtonsMasker : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        if(Constants.I.turnAllVoteButtonsInvisible)
            Invoke(nameof(DoTurnAllVoteButtonsInvisible), 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DoTurnAllVoteButtonsInvisible()
    {
        //var animEngines = GetComponentsInChildren<AnimEngine>();
        KillAll<AnimPulseInstantiator>();
        KillAll<AnimEngine>();

        CrawlForColoredComponents(out var StorageImg, out var StorageTMP);

        foreach (var v in StorageImg)
            v.enabled = false;
        foreach (var v in StorageTMP)
            v.enabled = false;

        
        foreach(var v in StorageImg)
        {
            Color col = v.color;
            v.color = new(col.r, col.g, col.b, 0f);
        }
        foreach (var v in StorageTMP)
        {
            Color col = v.color;
            v.color = new(col.r, col.g, col.b, 0f);
        }
    }

    void KillAll<T>() where T : UnityEngine.Component
    {
        var componentsArray = GetComponentsInChildren<T>(true);
        for (int i=0; i<componentsArray.Length; i++)
        {
            Destroy(componentsArray[i]);
        }
    }

    [SerializeField] private long recursionSanityCounter = 0;
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
    /*
    void SetAllComponentsToAlphaPercentage(float alphaPercentage, List<Image> _StorageImg, List<TextMeshProUGUI> _StorageTMP)
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
    */
}
