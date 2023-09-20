using System;
using Assets.Scripts.GameSession.UIScripts.SessionModel;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents
{
    public enum Side
    {
        Left, Right, Up, Down
    }

    public class CardsAndDecksSwitcher : MonoBehaviour
    {
        [SerializeField] private DeckDisplay _deckDisplay;
        [SerializeField] private GameSessionController _gameSessionController;
        [SerializeField] private LevelUpControllerScreen _levelUp;

        private Side _side;

        private void Awake()
        {
            _deckDisplay = FindObjectOfType<DeckDisplay>(true);
            _gameSessionController = FindObjectOfType<GameSessionController>(true);
            _levelUp = FindObjectOfType<LevelUpControllerScreen>(true);
        }

        public bool _isInProcess = false;

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

        public void OnSwipeEnd()
        {
            switch (_side)
            {
                case Side.Left:
                    _deckDisplay.SelectPreviousCard();
                    break;
                case Side.Right:
                    _deckDisplay.SelectNextCard();
                    break;
                case Side.Up:
                    _deckDisplay.ActivateNextDeck();
                    break;
                case Side.Down:
                    _deckDisplay.ActivatePreviousDeck();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            _isInProcess = false;
        }

        public void OnNextCard_M()
        {
            if (_isInProcess) return;
                _side = Side.Right;
            _isInProcess = true;
        }

        public void OnPreviousCard_M()
        {
            if (_isInProcess) return;
            _side = Side.Left;
            _isInProcess = true;
        }

        public void OnNextDeck_M()
        {
            if (_isInProcess) return;
            _side = Side.Up;
            _isInProcess = true;
        }

        public void OnPreviousDeck_M()
        {
            if (_isInProcess) return;
            _side = Side.Down;
            _isInProcess = true;
        }
    }
}