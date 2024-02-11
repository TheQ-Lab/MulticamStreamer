using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamAssignHandler : MonoBehaviour
{
    public static CamAssignHandler Instance;

    [SerializeField] int[] defaultToolbarIndices = { 2, 3, 1, 0 };
    private List<int> toolbarIndices = new(4);

    #region OnScreenMenu
    private string[] toolbarStrings = { "" };
    void OnGUI()
    {
        if (!menuActive)
            return;

        //GUIStyle style = new GUIStyle();
        //style.normal.background

        GUI.skin.label.fontSize = 24;
        //GUI.skin.label.
        GUI.skin.button.fontSize = 22;
        for (int i = 0; i < camAgents.Count; i++)
        {
            CamAssignAgent camFrame = camAgents[i];
            GUI.Label(new Rect(25,100+(i*95),150, 30), camFrame.name);
            //GUILayout.Label(
            toolbarIndices[i] = GUI.Toolbar(new Rect(25, 135+(i*95), 800, 50), toolbarIndices[i], toolbarStrings);
        }
        /*
        foreach (WebCamDevice dev in wcDeviceLst)
            GUILayout.Label(dev.name);
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

    private void Awake()
    {
        TooManyFuncts.Singletonize(ref Instance, this, false);
    }

    public bool menuActive;

    List<WebCamDevice> wcDeviceLst;
    List<WebCamTexture> wcTextureLst;

    private List<CamAssignAgent> camAgents = new();

    private void Start()
    {
        //camFrames = new(GetComponentsInChildren<WCamTx>());
        // camFrames = TooManyFuncts.GetComponentsInChildrenParametric<CamAssignAgent>(transform, null, null, true);
         camAgents = TooManyFuncts2.GetComponentsInChildrenParametric<CamAssignAgent>(transform, null, null, true, true);
        //foreach(Transform c1 in transform)
        //    camFrames.Add(c1.GetComponentInChildren<WCamTx>());

        ScanForCameras();

        // Startup sanity check wether preffered cameras even are available - to get assigned
        foreach(int ind in defaultToolbarIndices)
        {
            //int nuInd = Mathf.Min(ind, wcDeviceLst.Count - 1);
            int nuInd = ind;
            if(nuInd >= wcDeviceLst.Count)
                nuInd = 0;
            toolbarIndices.Add(nuInd);
        }

        ApplyCameraAssigns();


        InvokeRepeating(nameof(ApplyCameraResizes), 2f, .2f);
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

        foreach(CamAssignAgent camAgnt in camAgents)
        {
            int i = camAgents.IndexOf(camAgnt);
            camAgnt.AssignTx(wcTextureLst[toolbarIndices[i]]);
            //camTx.ScaleWidthToHeight();
            //camAgnt.CropToSize();
        }

        ApplyCameraResizes();
    }

    private void ApplyCameraResizes()
    {
        ApplyCameraResizes(null);
    }

#nullable enable
    private void ApplyCameraResizes(List<CamAssignAgent>? resizedCamAgents)
    {
        // null means resize all!
        if (resizedCamAgents is null)
            resizedCamAgents = camAgents;

        foreach(CamAssignAgent agent in resizedCamAgents)
        {
            agent.CropToSize();
        }
    }
#nullable disable
}
