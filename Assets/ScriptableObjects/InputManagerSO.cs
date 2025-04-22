using System;
using UnityEngine;
using UnityEngine.InputSystem;
[CreateAssetMenu(menuName = "InputaManager")]

public class InputManagerSO : ScriptableObject
{
    InputSystem_Actions myActions;

    public event Action OnJump;
    public event Action<Vector2> OnMove;

    private void OnEnable()
    {
        myActions = new InputSystem_Actions();
        myActions.Player.Enable();
        myActions.Player.Jump.started += Jump;
        myActions.Player.Move.performed += Move;
        myActions.Player.Move.canceled += Move;
    }

    private void Move(InputAction.CallbackContext context)
    {
        OnMove?.Invoke(context.ReadValue<Vector2>());
    }

    private void Jump(InputAction.CallbackContext context)
    {
        OnJump?.Invoke();
    }

    private void OnDisable()
    {
        myActions.Player.Disable();
    }

}
