using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAssignManager : MonoBehaviour
{
    public static CameraAssignManager Instance;
    private void Awake()
    {
        TooManyFuncts.Singletonize(ref Instance, this, false);
    }

    private int[] toolbarIndices = { 1, 1, 1, 1 };
    private string[] toolbarStrings = { "" };


    void OnGUI()
    {
        if (!menuActive)
            return;

        for (int i = 0; i < camFrames.Count; i++)
        {
            WCamTx camFrame = camFrames[i];
            GUI.Label(new Rect(25,105+(i*50),150, 30), camFrame.name);
            //GUILayout.Label(
            toolbarIndices[i] = GUI.Toolbar(new Rect(25, 125+(i*50), 400, 30), toolbarIndices[i], toolbarStrings);
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

    public bool menuActive;

    List<WebCamDevice> wcDeviceLst;
    List<WebCamTexture> wcTextureLst;

    private List<WCamTx> camFrames;

    private void Start()
    {
        camFrames = new(GetComponentsInChildren<WCamTx>());
        ScanForCameras();

        ApplyCameraAssigns();
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
            WCamTx camTx = camFrames[i];
            camTx.SetupAssignTx(wcTextureLst[toolbarIndices[i]]);
            camTx.SetupAdaptToAspectRatio();
        }
    }
}
