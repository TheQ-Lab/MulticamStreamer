using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjTracker : MonoBehaviour
{
    public enum Mode { WorldSpace, ScreenSpace };

    [Tooltip("Is tracking Target in World or Screen Space")]
    public Mode mode;
    public Transform trackedObj;

    private void Update()
    {
        switch (mode)
        {
            case Mode.WorldSpace:
                transform.position = new(trackedObj.position.x, trackedObj.position.y, transform.position.z);
                break;
            case Mode.ScreenSpace:
                transform.position = Camera.main.WorldToScreenPoint(trackedObj.position);
                break;
        }
    }
}
