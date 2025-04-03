using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour, IInput
{
    PlayerActionsMap inputActions;

    public Vector2 MovementInput => inputActions.Player.Movement.ReadValue<Vector2>();

    public event Action OnActionInput, OnPauseInput;
    private void OnEnable()
    {
        inputActions = new PlayerActionsMap();
        inputActions.Enable();
        inputActions.Player.Action.performed += HandleAction;
        inputActions.Player.Pause.performed += HandlePause;
    }

    private void HandlePause(InputAction.CallbackContext context)
    {
        OnPauseInput?.Invoke();
    }

    private void HandleAction(InputAction.CallbackContext context)
    {
        OnActionInput?.Invoke();
    }

    private void OnDisable()
    {
        inputActions.Player.Action.performed -= HandleAction;
        inputActions.Disable();

    }
}
