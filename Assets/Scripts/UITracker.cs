using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITracker : MonoBehaviour
{
    public Transform trackedObj;

    private void Update()
    {
        this.transform.position = Camera.main.WorldToScreenPoint(trackedObj.position);
    }

}
