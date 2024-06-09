using Core.Events;
using Core.Variables;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Input
{
    public class MovementInputHandler : MonoBehaviour
    {
        [Header("Game events")]
        public GameEvent onRun;
        public GameEvent onIdle;
        [Space] [Header("Variables")]
        public Vector2Variable playerMovementDirection;

        public void OnMove(InputAction.CallbackContext context)
        {
            Debug.LogWarning($"On Move");
            var value = context.ReadValue<Vector2>();
            playerMovementDirection.SetValue(value);
            if (value != Vector2.zero) onRun.Raise();
            else onIdle.Raise();
        }
    }
}