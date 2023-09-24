using Assets.Scripts.GameSession.UIScripts.SessionModel;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents
{
    public class CardsAndDecksSwitcher : MonoBehaviour
    {
        [SerializeField] private ComplexLevelUpDisplay _complexLevelUpDisplay;
        [SerializeField] private GameSessionController _gameSessionController;
        [SerializeField] private LevelUpController _levelUp;

        private void Awake()
        {
            _complexLevelUpDisplay = FindObjectOfType<ComplexLevelUpDisplay>(true);
            _gameSessionController = FindObjectOfType<GameSessionController>(true);
            _levelUp = FindObjectOfType<LevelUpController>(true);
        }

        public void OnLevelUp()
        {
            _gameSessionController.Resume();
            _levelUp.LevelUp();
        }

        public void OnNextDeck()
        {
            _complexLevelUpDisplay.ActivateNextDeck();
        }

        public void OnPreviousDeck()
        {
            _complexLevelUpDisplay.ActivatePreviousDeck();
        }
    }
}