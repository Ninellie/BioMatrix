using Assets.Scripts.Core.Events;
using Assets.Scripts.Core.Variables;
using Assets.Scripts.Core.Variables.References;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputEventsHandler : MonoBehaviour
{
    [Header("Game events")]
    public GameEvent onFire;
    public GameEvent onRun;
    public GameEvent onIdle;
    [Space]
    [Header("Variables")]
    public Vector2Variable playerMovementDirection;
    public Vector2Variable playerAimDirection;
    public Vector2Reference playerPosition;

    private bool _isFireButtonPressed = false;

    private void FixedUpdate()
    {
        if (_isFireButtonPressed)
        {
            onFire.Raise();
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        var value = context.ReadValue<Vector2>();
        playerMovementDirection.SetValue(value);
        if (value != Vector2.zero) onRun.Raise();
        else onIdle.Raise();
    }

    public void OnFireStart()
    {
        _isFireButtonPressed = true;
    }

    public void OnFireEnd()
    {
        _isFireButtonPressed = false;
    }

    public void OnMouseAiming(InputAction.CallbackContext context)
    {
        var mousePosition = context.ReadValue<Vector2>();
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        var directionToMousePos = mousePosition - playerPosition;
        directionToMousePos.Normalize();
        playerAimDirection.SetValue(directionToMousePos);
    }

    public void OnJoystickAimingFire(InputAction.CallbackContext context)
    {
        var value = context.ReadValue<Vector2>();
        _isFireButtonPressed = !value.Equals(Vector2.zero);
        value.Normalize();
        playerAimDirection.SetValue(value);
    }
}