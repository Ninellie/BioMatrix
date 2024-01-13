using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Core.Input
{
    public class LevelUpInputHandler : MonoBehaviour
    {
        public UnityEvent _onNextDeck;
        public UnityEvent _onPreviousDeck;
        public UnityEvent _onDeckAccept;

        public void OnNextDeck(InputAction.CallbackContext context)
        {
            if (!context.action.WasReleasedThisFrame()) return;
            Debug.LogWarning($"On next deck");
            _onNextDeck.Invoke();
        }

        public void OnPreviousDeck(InputAction.CallbackContext context)
        {
            if (!context.action.WasReleasedThisFrame()) return;
            Debug.LogWarning($"On previous deck");
            _onPreviousDeck.Invoke();
        }

        public void OnDeckAccept(InputAction.CallbackContext context)
        {
            if (!context.action.WasReleasedThisFrame()) return;
            Debug.LogWarning($"On deck accept");
            _onDeckAccept.Invoke();
        }
    }
}