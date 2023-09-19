using Assets.Scripts.GameSession.UIScripts.SessionModel;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents
{
    public class LevelUpSwitcher : MonoBehaviour
    {
        [SerializeField] private DeckDisplay _deckDisplay;
        [SerializeField] private GameSessionController _gameSessionController;
        [SerializeField] private LevelUpControllerScreen _levelUp;

        private void Awake()
        {
            _deckDisplay = FindObjectOfType<DeckDisplay>(true);
            _gameSessionController = FindObjectOfType<GameSessionController>(true);
            _levelUp = FindObjectOfType<LevelUpControllerScreen>(true);
        }

        public void OnLevelUp()
        {
            _gameSessionController.Resume();
            _levelUp.LevelUp();
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