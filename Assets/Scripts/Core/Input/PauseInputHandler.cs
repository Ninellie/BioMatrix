using Core.Events;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Input
{
    public class PauseInputHandler : MonoBehaviour
    {
        public GameEvent onPause;

        public void OnPause(InputAction.CallbackContext context)
        {
            if (!context.action.WasReleasedThisFrame()) return;
            onPause.Raise();
            Debug.LogWarning($"On Pause");
        }
    }
}