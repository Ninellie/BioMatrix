using Assets.Scripts.GameSession.UIScripts.SessionModel;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents
{
    public class CardsAndDecksSwitcher : MonoBehaviour
    {
        [SerializeField] private ComplexLevelUpDisplay _complexLevelUpDisplay;
        [SerializeField] private GameSessionController _gameSessionController;
        [SerializeField] private LevelUpController _levelUp;

        private Vector2 _side = Vector2.zero;

        private void Awake()
        {
            _complexLevelUpDisplay = FindObjectOfType<ComplexLevelUpDisplay>(true);
            _gameSessionController = FindObjectOfType<GameSessionController>(true);
            _levelUp = FindObjectOfType<LevelUpController>(true);
        }

        public bool _isInProcess = false;

        public void OnLevelUp()
        {
            _gameSessionController.Resume();
            _levelUp.LevelUp();
        }

        //public void OnNextCard()
        //{
        //    _complexLevelUpDisplay.SelectNextCard();
        //}

        //public void OnPreviousCard()
        //{
        //    _complexLevelUpDisplay.SelectPreviousCard();
        //}

        public void OnNextDeck()
        {
            _complexLevelUpDisplay.ActivateNextDeck();
        }

        public void OnPreviousDeck()
        {
            _complexLevelUpDisplay.ActivatePreviousDeck();
        }

        public void OnSwipeEnd()
        {
            if (_side.Equals(Vector2.up))
            {
                _complexLevelUpDisplay.ActivateNextDeck();
            }
            if (_side.Equals(Vector2.down))
            {
                _complexLevelUpDisplay.ActivatePreviousDeck();
            }

            _isInProcess = false;
            _side = Vector2.zero;
        }

        //public void OnNextCard_M()
        //{
        //    if (_isInProcess) return;
        //        _side = Side.Right;
        //    _isInProcess = true;
        //}

        //public void OnPreviousCard_M()
        //{
        //    if (_isInProcess) return;
        //        _side = Side.Left;
        //    _isInProcess = true;
        //}

        public void OnNextDeck_M()
        {
            if (_isInProcess) return;
            _side = Vector2.up;
            _isInProcess = true;
        }

        public void OnPreviousDeck_M()
        {
            if (_isInProcess) return;
            _side = Vector2.down;
            _isInProcess = true;
        }
    }
}