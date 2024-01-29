using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    [SerializeField] private InputAction MenuToggle;

    //public bool menuActive;

    private void Awake()
    {
        TooManyFuncts.Singletonize(ref Instance, this, false);
        MenuToggle.performed += ctx => ToggleMenu(ctx);
    }

    private void OnEnable()
    {
        MenuToggle.Enable();
    }

    private void ToggleMenu(InputAction.CallbackContext context)
    {
        CameraAssignManager.Instance.menuActive = !CameraAssignManager.Instance.menuActive;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
