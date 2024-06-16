using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [Header("Input Action Asset")]
    [SerializeField] private InputActionAsset playerControls;

    [Header("Action Map Name References")]
    [SerializeField] private string actionMapName = "PlayerInput";

    [Header("Action Name References")]
    [SerializeField] private string move = "Move";
    [SerializeField] private string jump = "Jump";
    [SerializeField] private string fire = "Fire";
    [SerializeField] private string changeWeapon = "ChangeWeapon";
    [SerializeField] private string reload = "Reload";
    [SerializeField] private string slowWalk = "SlowWalk";
    [SerializeField] private string pickup = "Pickup";

    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction fireAction;
    private InputAction changeWeaponAction;
    private InputAction reloadAction;
    private InputAction slowWalkAction;
    private InputAction pickupAction;

    public Vector2 MoveInput { get; private set; }
    public bool UseJumpTriggered { get; private set; }
    public bool UseFireTriggered { get; private set; }
    public bool UseChangeWeaponTriggered { get; private set; }
    public bool UseReloadTriggered { get; private set; }
    public bool UseSlowWalkTriggered { get; private set; }
    public bool UsePickupTriggered { get; private set; }


    public static PlayerInputHandler Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        { 
            Destroy(gameObject);
        }

        moveAction = playerControls.FindActionMap(actionMapName).FindAction(move);
        jumpAction = playerControls.FindActionMap(actionMapName).FindAction(jump);
        fireAction = playerControls.FindActionMap(actionMapName).FindAction(fire);
        changeWeaponAction = playerControls.FindActionMap(actionMapName).FindAction(changeWeapon);
        reloadAction = playerControls.FindActionMap(actionMapName).FindAction(reload);
        slowWalkAction = playerControls.FindActionMap(actionMapName).FindAction(slowWalk);
        pickupAction = playerControls.FindActionMap(actionMapName).FindAction(pickup);

        RegisterInputActions();

    }

    void RegisterInputActions()
    {
        moveAction.performed += context => MoveInput = context.ReadValue<Vector2>();
        moveAction.canceled += context => MoveInput = Vector2.zero;

        jumpAction.performed += context => UseJumpTriggered = true;
        jumpAction.canceled += context => UseJumpTriggered = false;

        fireAction.performed += context => UseFireTriggered = true;
        fireAction.canceled += context => UseFireTriggered = false;

        changeWeaponAction.performed += context => UseChangeWeaponTriggered = true;
        changeWeaponAction.canceled += context => UseChangeWeaponTriggered = false;

        reloadAction.performed += context => UseReloadTriggered = true;
        reloadAction.canceled += context => UseReloadTriggered = false;

        slowWalkAction.performed += context => UseSlowWalkTriggered = true;
        slowWalkAction.canceled += context => UseSlowWalkTriggered = false;

        pickupAction.performed += context => UsePickupTriggered = true;
        pickupAction.canceled += context => UsePickupTriggered = false;
    }


    private void OnEnable()
    {
        moveAction.Enable();
        jumpAction.Enable();
        fireAction.Enable();
        changeWeaponAction.Enable();
        reloadAction.Enable();
        slowWalkAction.Enable();
        pickupAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        jumpAction.Disable();
        fireAction.Disable();
        changeWeaponAction.Disable();
        reloadAction.Disable();
        slowWalkAction.Disable();
        pickupAction.Disable();
    }

}
