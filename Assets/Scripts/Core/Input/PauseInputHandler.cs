using Assets.Scripts.Core.Events;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Core.Input
{
    public class PauseInputHandler : MonoBehaviour
    {
        public GameEvent onPause;
        public bool isPauseButtonPressed;

        public void OnPause(InputAction.CallbackContext context)
        {
            if (context.action.WasPressedThisFrame()){ isPauseButtonPressed = true;}
            if (!context.action.WasReleasedThisFrame()) return;
            if (!isPauseButtonPressed) return;
            isPauseButtonPressed = false;
            onPause.Raise();
            Debug.LogWarning($"On Pause");
        }
    }
}