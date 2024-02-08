using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    [SerializeField] private InputAction MenuToggle;
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
        ShowCam1.performed += ctx => ShowCamN(1, ctx);
        ShowCam2.performed += ctx => ShowCamN(2, ctx);
        ShowCam3.performed += ctx => ShowCamN(3, ctx);
        ShowCam4.performed += ctx => ShowCamN(4, ctx);
    }

    private void OnEnable()
    {
        MenuToggle.Enable();
        ShowCam1.Enable();
        ShowCam2.Enable();
        ShowCam3.Enable();
        ShowCam4.Enable();
    }

    private void ToggleMenu(InputAction.CallbackContext context)
    {
        CamAssignHandler.Instance.menuActive = !CamAssignHandler.Instance.menuActive;
    }

    private void ShowCamN(int n, InputAction.CallbackContext context)
    {
        currentSwapAnimTgt = n;
        inSwapAnim.Set();
        //...
    }

    private void OnDisable()
    {
        MenuToggle.Disable();
        ShowCam1.Disable();
        ShowCam2.Disable();
        ShowCam3.Disable();
        ShowCam4.Disable();
    }
}
