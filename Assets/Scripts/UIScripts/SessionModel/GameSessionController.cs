using UnityEngine;

namespace UIScripts.SessionModel
{
    public class GameSessionController : MonoBehaviour
    {
        [SerializeField] private ViewControllerOptions _controllerOptions;

        private ViewModel _viewModel;

        private void Awake()
        {
            var viewController = new SimpleViewController(_controllerOptions);
            _viewModel = new ViewModel(viewController);
            Time.timeScale = 0f;
            _controllerOptions.PauseMapEvent.Raise();
        }

        public void Menu() { _viewModel.GetCurrentState().PauseScreen(); }
        public void Resume() { _viewModel.GetCurrentState().Resume(); }
        public void Options() { _viewModel.GetCurrentState().Options(); }
        public void LevelUpEvent() { _viewModel.GetCurrentState().LevelUp(); }
        public void Win() { _viewModel.GetCurrentState().Win(); }
        public void Lose() { _viewModel.GetCurrentState().Lose(); }
    }
}