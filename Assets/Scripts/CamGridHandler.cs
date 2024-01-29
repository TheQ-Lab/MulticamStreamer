using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamGridHandler : MonoBehaviour
{
    [Serializable]
    public class CamFrameTransform
    {
        public Vector3 position;
        public Vector3 scale;

        public CamFrameTransform(Vector3 position, Vector3 scale)
        {
            this.position = position;
            this.scale = scale;
        }
    }

    public List<CamFrameTransform> CamFrameTransforms = new ();
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform t in transform)
        {
            CamFrameTransforms.Add(new(t.position, t.localScale));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
