using Assets.Scripts.Core.Events;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Core.Input
{
    public class LevelUpInputHandler : MonoBehaviour
    {
        [SerializeField] private GameEvent _onNextDeck;
        [SerializeField] private GameEvent _onPreviousDeck;
        [SerializeField] private GameEvent _onDeckAccept;

        public void OnNextDeck(InputAction.CallbackContext context)
        {
            if (!context.action.WasReleasedThisFrame()) return;
            Debug.LogWarning($"On next deck");
            _onNextDeck.Raise();
        }

        public void OnPreviousDeck(InputAction.CallbackContext context)
        {
            if (!context.action.WasReleasedThisFrame()) return;
            Debug.LogWarning($"On previous deck");
            _onPreviousDeck.Raise();
        }

        public void OnDeckAccept(InputAction.CallbackContext context)
        {
            if (!context.action.WasReleasedThisFrame()) return;
            Debug.LogWarning($"On deck accept");
            _onDeckAccept.Raise();
        }
    }
}