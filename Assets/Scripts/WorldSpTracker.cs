using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSpTracker : MonoBehaviour
{
    public Transform trackedObj;

    private void Update()
    {
        transform.position = new(trackedObj.position.x, trackedObj.position.y, transform.position.z);
    }
}
