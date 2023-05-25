using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.GameSession.View
{
    public class ViewController : MonoBehaviour
    {
        [SerializeField] private GameObject _menuUi;
        [SerializeField] private GameObject _optionsUi;
        [SerializeField] private GameObject _levelUpUi;
        [SerializeField] private GameObject _winScreenUi;
        [SerializeField] private GameObject _loseScreenUi;
        [SerializeField] private GameObject _startScreenUi;
        private ViewModel _viewModel;

        public void AwakeController()
        {
            var mapController = CreateMapController();
            
            _viewModel = new ViewModel(mapController);
            
        }
        public void Menu() { _viewModel.GetCurrentState().Menu(); }
        public void Resume() { _viewModel.GetCurrentState().Resume(); }
        public void Options() { _viewModel.GetCurrentState().Options(); }
        public void LevelUpEvent() { _viewModel.GetCurrentState().LevelUp(); }
        public void Win() { _viewModel.GetCurrentState().Win(); }
        public void Lose() { _viewModel.GetCurrentState().Lose(); }

        private IViewController CreateMapController()
        {
            var currentPlayer = FindObjectOfType<Player>();
            var playerInput = currentPlayer.GetComponent<PlayerInput>();
            var mapController = new SimpleViewController(playerInput, _menuUi, _optionsUi, _levelUpUi, _winScreenUi, _loseScreenUi, _startScreenUi);
            return mapController;
        }
    }
}