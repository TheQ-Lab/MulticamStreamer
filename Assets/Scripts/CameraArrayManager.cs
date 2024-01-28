using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraArrayManager : MonoBehaviour
{
    //public enum Type { BlueSpace, RedSpace, StarSpace, Fork, MinigameSpace };
    //public Type type = Type.BlueSpace;

    private int[] toolbarIndices = { 1, 1, 1, 1 };
    private string[] toolbarStrings = { "" };
    void OnGUI()
    {
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

    WebCamDevice[] wcDeviceLst;
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
        wcDeviceLst = WebCamTexture.devices;
        foreach (WebCamDevice dev in wcDeviceLst)
            Debug.Log(dev.name);

        // Update UI with new List
        List<string> names = new List<string>();
        foreach (WebCamDevice dev in wcDeviceLst)
        { names.Add(dev.name); }
        toolbarStrings = names.ToArray();

        // Creates Textures
        wcTextureLst = new();
        foreach (WebCamDevice wcDevice in wcDeviceLst)
            wcTextureLst.Add(new WebCamTexture(wcDevice.name));
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
