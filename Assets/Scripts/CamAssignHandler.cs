using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamAssignHandler : MonoBehaviour
{

    [SerializeField] int[] defaultToolbarIndices = { 2, 3, 0, 1 };
    List<int> toolbarIndices = new(4);

    #region OnScreenMenu

    private string[] toolbarStrings = { "" };
    void OnGUI()
    {
        if (!menuActive)
            return;

        for (int i = 0; i < camFrames.Count; i++)
        {
            CamTxAgent camFrame = camFrames[i];
            GUI.Label(new Rect(25,105+(i*50),150, 30), camFrame.name);
            //GUILayout.Label(
            toolbarIndices[i] = GUI.Toolbar(new Rect(25, 125+(i*50), 800, 30), toolbarIndices[i], toolbarStrings);
        }
        /*
        foreach (WebCamDevice dev in wcDeviceLst)
        {
            GUILayout.Label(dev.name);
        }
        */

        if (GUILayout.Button("Apply changes"))
        {
            ApplyCameraAssigns();
            Debug.Log("Applied!");
        }
        if (GUILayout.Button("Scan for new Cameras"))
        {
            ScanForCameras();
            Debug.Log("Scanned!");
        }

        /*
        if(GUI.changed)
        {
            ApplyCameraAssigns();
            Debug.Log("Applied!");
        }
        */
    }
    #endregion

    public static CamAssignHandler Instance;
    private void Awake()
    {
        TooManyFuncts.Singletonize(ref Instance, this, false);
    }

    public bool menuActive;

    List<WebCamDevice> wcDeviceLst;
    List<WebCamTexture> wcTextureLst;

    private List<CamTxAgent> camFrames = new();

    private void Start()
    {
        //camFrames = new(GetComponentsInChildren<WCamTx>());
        // camFrames = TooManyFuncts.GetComponentsInChildrenParametric<CamTxAgent>(transform, null, null, true);
         camFrames = TooManyFuncts2.GetComponentsInChildrenParametric<CamTxAgent>(transform, null, null, true, true);
        //foreach(Transform c1 in transform)
        //    camFrames.Add(c1.GetComponentInChildren<WCamTx>());

        ScanForCameras();

        // Startup sanity check wether preffered cameras even are available - to get assigned
        foreach(int ind in defaultToolbarIndices)
        {
            int nuInd = Mathf.Min(ind, wcDeviceLst.Count - 1);
            toolbarIndices.Add(nuInd);
        }

        ApplyCameraAssigns();


        InvokeRepeating(nameof(ApplyCameraAssigns), 2f, 2f);
    }

    private void ScanForCameras()
    {
        // Scan for new Devices 
        if (wcDeviceLst == null) // initial
        {
            wcDeviceLst = new(WebCamTexture.devices);
            // Creates Textures
            wcTextureLst = new();
            foreach (WebCamDevice wcDevice in wcDeviceLst)
                wcTextureLst.Add(new WebCamTexture(wcDevice.name));
        }
        else // add more [WCDevice cant be just dumped and a new Device created - without stopping() it before]
        {
            foreach (WebCamDevice wcDevice in WebCamTexture.devices)
            {
                if (!wcDeviceLst.Contains(wcDevice))
                {
                    wcTextureLst.Add(new WebCamTexture(wcDevice.name));
                }
            }
            wcDeviceLst = new(WebCamTexture.devices);
        }
        foreach (WebCamDevice dev in wcDeviceLst)
            Debug.Log(dev.name);

        // Update UI with new List
        List<string> names = new List<string>();
        foreach (WebCamDevice dev in wcDeviceLst)
        { names.Add(dev.name); }
        toolbarStrings = names.ToArray();

    }

    private void ApplyCameraAssigns()
    {
        foreach(WebCamTexture tx in wcTextureLst)
            tx.Pause();

        for (int i = 0; i < camFrames.Count; i++)
        {
            CamTxAgent camTx = camFrames[i];
            camTx.SetupAssignTx(wcTextureLst[toolbarIndices[i]]);
            //camTx.ScaleWidthToHeight();
            camTx.CropToSize();
        }
    }
}
