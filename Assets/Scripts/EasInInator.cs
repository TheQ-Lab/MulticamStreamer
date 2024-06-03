using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EasInInator : MonoBehaviour
{
    //private List<float> AlphaStorage;
    public List<Image> StorageImg;
    public List<TextMeshProUGUI> StorageTmp;
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
        //AlphaStorage = new();
        StorageImg = new();
        StorageTmp = new();

        Recurse(transform);

        void Recurse(Transform t)
        {
            FindComponentMatches(t);
            counter++;
            foreach (Transform c in t)
            {
                Recurse(c);
            }
        }
    }

#nullable enable
#nullable disable

    private void FindComponentMatches(Transform t)
    {
        t.TryGetComponent<Image>(out var image);
        if (image is not null)
            StorageImg.Add(image);
        t.TryGetComponent<TextMeshProUGUI>(out var tmpUI);
        if (tmpUI is not null)
            StorageTmp.Add(tmpUI);

    } 

}
