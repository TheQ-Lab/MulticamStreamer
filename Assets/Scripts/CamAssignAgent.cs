using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MBExtensions;

public class CamAssignAgent: MonoBehaviour
{
    //private WebCamDevice wcDevice;
    [SerializeField]
    public WebCamTexture tx;
    [SerializeField]
    private Material mat;

    [SerializeField]
    private Vector3 txResolution;
    public void AssignTx(WebCamTexture newTexture)
    {
        mat = GetComponent<Renderer>().material;
        //mat = TooManyFuncts2.GetComponentInChildrenParametric<MeshRenderer>(transform, null, null, null, true);

        /*if(tx != null)
            tx.Stop();*/
        // --- Get Tx
        tx = newTexture;
        
        // --- Assign Texture
        //GetComponent<Renderer>().material.mainTexture = tx;   // Without instantiating
        mat.mainTexture = tx;                                   // With instantiating

        tx.Play();
        txResolution = new(tx.width, tx.height, 0);
        //Debug.Log(tx.width + ", " + tx.height);

        StartCoroutine(this.DelayedExecution(MeasureFPS, 3f));
        void MeasureFPS()
        {
            txResolution.z = tx.updateCount / 3f;
        }
    }


    public void ScaleWidthToHeight()
    {
        //Debug.Log(tx.width + ", " + tx.height);
        float aspectRatio = Mathf.Abs(TooManyFuncts.Remap(tx.width, 0, tx.height, 0, transform.localScale.z)); //for height = 1, width of cam is <--
        // when camHeight is planeHeight, camWidth is ?
        Vector3 scale = transform.localScale;
        if (scale.x < 0f) aspectRatio *= -1;
        scale.x = aspectRatio;
        transform.localScale = scale;
        //Debug.Log(wcDevice.availableResolutions);


       // IMPORTANT: Both X and Z are normally mirrored, else flip in scale in inspector
    }

    public void CropToSize()
    {
        // ---fixed arguments:---
        //Vector3 frameSize = transform.parent.localScale;
        Vector2 frameSize = new(transform.localScale.x, transform.localScale.z);
        var mesh = GetComponent<MeshFilter>().mesh;
        Vector2 meshSize = new(mesh.bounds.size.x, mesh.bounds.size.z);
        //Debug.LogWarning("REEE - " + transform.name + " " + meshSize.ToString());
        // --------------------------------


        //float aspectRatioTx = Mathf.Abs(TooManyFuncts.Remap(tx.width, 0, tx.height, 0, 1)); //for height = 1, width of cam is <--
        //Debug.Log(aspectRatioTx);
        float aspectRatioTx = Mathf.Abs(TooManyFuncts.Remap(tx.height, 0, tx.width, 0, 1)); //for width = 1, height of cam is <-- // alias: 1 to x

        //Debug.Log(aspectRatioTx);

        float aspectRatioFrame = Mathf.Abs(TooManyFuncts.Remap(frameSize.y, 0, frameSize.x, 0, 1));
        aspectRatioFrame *= Mathf.Abs(TooManyFuncts.Remap(meshSize.y, 0, meshSize.x, 0, 1));

        //Debug.Log(aspectRatioFrame);

        // aspectRaio of Tx < aspectRatio of Frame => crop to Height | crop of right and left edges

        Vector2 txTiling = new(1f, 1);
        Vector2 txOffset = new(0f, 0f);
        if (aspectRatioTx < aspectRatioFrame)
        {
            float aspectRatioRel = Mathf.Abs(TooManyFuncts.Remap(tx.height, 0, tx.width, 0, 1/ aspectRatioFrame));

            txTiling.x = aspectRatioRel;
            txOffset.x = (1 - aspectRatioRel) / 2;

            // Old versions / Specific to 1:1 Frame-Aspect
            //txTiling.x = aspectRatioTx;
            //txOffset.x = (1 - aspectRatioTx) / 2;
        }
        else if (aspectRatioTx > aspectRatioFrame)
        {
            float aspectRatioRel = Mathf.Abs(TooManyFuncts.Remap(tx.width, 0, tx.height, 0, aspectRatioFrame));

            txTiling.y = aspectRatioRel;
            txOffset.y = (1 - aspectRatioRel) / 2;
        }

        mat.mainTextureScale = txTiling;
        mat.mainTextureOffset = txOffset;
        //mat.SetTextureScale("_MainTex", txTiling);
    }
}
