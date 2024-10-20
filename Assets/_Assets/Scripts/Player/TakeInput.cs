using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TakeInput : MonoBehaviour
{
    private const string PLAYER_PREFS_INPUT = "inputBindings";
    
    private PlayerInput playerInputActions;
    public event Action onInteractAction;
    public event Action onCuttingAction;
    public event Action onPauseGame;
    
    public static TakeInput Instance { get; private set; }

    public enum Bindings
    {
        MoveUp,
        MoveDown,
        MoveRight,
        MoveLeft,
        Interact,
        CutInteract,
        Pause
    }
    
    private void Awake()
    {
        Instance = this;
        playerInputActions = new PlayerInput();
        if (PlayerPrefs.HasKey(PLAYER_PREFS_INPUT))
        {
            playerInputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_INPUT));
        }
        playerInputActions.Player.Enable();
        playerInputActions.Player.Interact.performed += InteractOnperformed;
        playerInputActions.Player.CutObject.performed += CutObjectOnperformed;
        playerInputActions.Player.Pause.performed += PauseOnperformed;
    }

    private void OnDestroy()
    {
        playerInputActions.Player.Interact.performed -= InteractOnperformed;
        playerInputActions.Player.CutObject.performed -= CutObjectOnperformed;
        playerInputActions.Player.Pause.performed -= PauseOnperformed;
        playerInputActions.Dispose();
    }

    private void PauseOnperformed(InputAction.CallbackContext obj)
    {
        onPauseGame?.Invoke();
    }

    private void CutObjectOnperformed(InputAction.CallbackContext obj)
    {
        onCuttingAction?.Invoke();
    }

    private void InteractOnperformed(InputAction.CallbackContext obj)
    {
        onInteractAction?.Invoke();
    }


    public Vector2 GetDirectionInputNormalized()
    {
        Vector2 direction = playerInputActions.Player.Move.ReadValue<Vector2>();
        return direction.normalized;
    }

    public string GetKeyBinding(Bindings state)
    {
        switch (state)
        {
            case Bindings.MoveUp:
                return playerInputActions.Player.Move.bindings[1].ToDisplayString();
            case Bindings.MoveDown:
                return playerInputActions.Player.Move.bindings[2].ToDisplayString();
            case Bindings.MoveRight:
                return playerInputActions.Player.Move.bindings[4].ToDisplayString();
            case Bindings.MoveLeft:
                return playerInputActions.Player.Move.bindings[3].ToDisplayString();
            case Bindings.Interact:
                return playerInputActions.Player.Interact.bindings[0].ToDisplayString();
            case Bindings.CutInteract:
                return playerInputActions.Player.CutObject.bindings[0].ToDisplayString();
            case Bindings.Pause:
                return playerInputActions.Player.Pause.bindings[0].ToDisplayString();
        }

        return "";
    }


    public void Rebinding(Bindings key, Action onActionRebound)
    {
        InputAction inputAction;
        int bindingIndex;
        switch (key)
        {
            default:
            case Bindings.MoveUp:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 1;
                break;
            case Bindings.MoveDown:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 2;
                break;
                case Bindings.MoveRight:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 4;
                break;
                case Bindings.MoveLeft:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 3;
                break;
                case Bindings.Interact:
                inputAction = playerInputActions.Player.Interact;
                bindingIndex = 0;
                break;
                case Bindings.CutInteract:
                inputAction = playerInputActions.Player.CutObject;
                bindingIndex = 0;
                break;
                case Bindings.Pause:
                inputAction = playerInputActions.Player.Pause;
                bindingIndex = 0;
                break;
        }
        playerInputActions.Player.Disable();
        
        inputAction.PerformInteractiveRebinding(bindingIndex)
            .OnComplete(callback =>
            {
                callback.Dispose();
                playerInputActions.Player.Enable();
                onActionRebound();
                PlayerPrefs.SetString(PLAYER_PREFS_INPUT,playerInputActions.SaveBindingOverridesAsJson());
                PlayerPrefs.Save();
            }).Start();
        
        
    }
    
}
