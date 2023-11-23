using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.GameSession.UIScripts.SessionModel
{
    public class GameSessionController : MonoBehaviour
    {
        [SerializeField] private PlayerInput _playerInput;
        [Space]
        [Header("Interface objects")]
        [SerializeField] private GameObject _menuUi;
        [SerializeField] private GameObject _optionsUi;
        [SerializeField] private GameObject _levelUpUi;
        [SerializeField] private GameObject _winScreenUi;
        [SerializeField] private GameObject _loseScreenUi;
        [SerializeField] private GameObject _startScreenUi;

        private ViewModel _viewModel;

        private void Awake()
        {
            AwakeController();
        }

        private void AwakeController()
        {
            var viewController = new SimpleViewController(_playerInput, _menuUi, _optionsUi, _levelUpUi, _winScreenUi, _loseScreenUi, _startScreenUi);
            _viewModel = new ViewModel(viewController);
            Time.timeScale = 0f;
            _playerInput.SwitchCurrentActionMap("Menu");
        }
        public void Menu() { _viewModel.GetCurrentState().Menu(); }
        public void Resume() { _viewModel.GetCurrentState().Resume(); }
        public void Options() { _viewModel.GetCurrentState().Options(); }
        public void LevelUpEvent() { _viewModel.GetCurrentState().LevelUp(); }
        public void Win() { _viewModel.GetCurrentState().Win(); }
        public void Lose() { _viewModel.GetCurrentState().Lose(); }
    }
}