using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    [SerializeField] private InputAction MenuToggle;
    [SerializeField] private InputAction SoftShutdown;
    [SerializeField] private InputAction ShowCam1;
    [SerializeField] private InputAction ShowCam2;
    [SerializeField] private InputAction ShowCam3;
    [SerializeField] private InputAction ShowCam4;

    public Flag inSwapAnim = new();
    public int currentSwapAnimTgt;

    //public bool menuActive;

    private void Awake()
    {
        TooManyFuncts.Singletonize(ref Instance, this, false);

        MenuToggle.performed += ctx => ToggleMenu(ctx);
        SoftShutdown.performed += ctx => PerformSoftShutdown(ctx);
        ShowCam1.performed += ctx => OnPressNum(1, ctx);
        ShowCam2.performed += ctx => OnPressNum(2, ctx);
        ShowCam3.performed += ctx => OnPressNum(3, ctx);
        ShowCam4.performed += ctx => OnPressNum(4, ctx);
    }

    private void OnEnable()
    {
        MenuToggle.Enable();
        SoftShutdown.Enable();
        ShowCam1.Enable();
        ShowCam2.Enable();
        ShowCam3.Enable();
        ShowCam4.Enable();
    }

    private void ToggleMenu(InputAction.CallbackContext context)
    {
        CamAssignHandler.Instance.menuActive = !CamAssignHandler.Instance.menuActive;
    }

    private void PerformSoftShutdown(CallbackContext ctx)
    {
        GetComponent<InputSimulatorScript>().ShutownProgram();
    }

    private void OnPressNum(int n, InputAction.CallbackContext context)
    {
        //Debug.Log("InSwap " + (bool)inSwapAnim + " voteOpen " + (bool)VoteManager.voteOpen);
        if (inSwapAnim) return;
        if ((bool) VoteManager.voteOpen is false) return;

        //currentSwapAnimTgt = n;
        //CamGridHandler.Instance.SwapTriggered(n);
        VoteManager.AddVotes(n, +1);
    }
    /*
    private void ShowCamN(int n, InputAction.CallbackContext context)
    {
        //Debug.LogError("hello?");
        if (inSwapAnim) return;

        currentSwapAnimTgt = n;
        CamGridHandler.Instance.SwapTriggered(n);
    }*/

    private void OnDisable()
    {
        MenuToggle.Disable();
        SoftShutdown.Disable();
        ShowCam1.Disable();
        ShowCam2.Disable();
        ShowCam3.Disable();
        ShowCam4.Disable();
    }
}
