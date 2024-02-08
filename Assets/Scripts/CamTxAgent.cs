using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CamTxAgent: MonoBehaviour
{
    //private WebCamDevice wcDevice;
    [SerializeField]
    private WebCamTexture tx;
    [SerializeField]
    private Material mat;

    [SerializeField]
    private Vector2Int txResolution;
    public void SetupAssignTx(WebCamTexture newTexture)
    {
        /*if(tx != null)
            tx.Stop();*/
        // --- Get Tx
        tx = newTexture;
        
        //Debug.Log(newTexture.name + " selected");

        //tx = new WebCamTexture(wcDevice.name);
        // --- Assign Texture
        //GetComponent<Renderer>().material.mainTexture = tx;  // Without instantiating
        mat = GetComponent<Renderer>().material;
        mat.mainTexture = tx;

        tx.Play();
        txResolution = new(tx.width, tx.height);

        //Debug.Log(tx.width + ", " + tx.height);

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
        float aspectRatio = Mathf.Abs(TooManyFuncts.Remap(tx.width, 0, tx.height, 0, 1)); //for height = 1, width of cam is <--
        Debug.Log(aspectRatio);

    }
}
