using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents
{
    public class LevelUpSwitcher : MonoBehaviour
    {
        [SerializeField] private DeckDisplay _deckDisplay;
        private void Awake()
        {
            _deckDisplay = FindObjectOfType<DeckDisplay>(true);
        }

        public void OnNextCard()
        {
            _deckDisplay.SelectNextCard();
        }

        public void OnPreviousCard()
        {
            _deckDisplay.SelectPreviousCard();
        }

        public void OnNextDeck()
        {
            _deckDisplay.ActivateNextDeck();
        }

        public void OnPreviousDeck()
        {
            _deckDisplay.ActivatePreviousDeck();
        }
    }
}