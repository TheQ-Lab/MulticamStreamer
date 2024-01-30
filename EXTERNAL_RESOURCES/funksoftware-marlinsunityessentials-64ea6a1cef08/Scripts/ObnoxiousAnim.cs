using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObnoxiousAnim : MonoBehaviour
{
    Transform tr;
    private void Awake()
    {
        var img = TooManyFuncts.GetComponentInChildrenParametric<UnityEngine.UI.Image>(transform, null, null, null);
        if (img is not null)
            tr = img.transform;
        else
            tr = transform;
    }

    void Update()
    {
        tr.Rotate(new(0, 0, 1f));
    }
}
