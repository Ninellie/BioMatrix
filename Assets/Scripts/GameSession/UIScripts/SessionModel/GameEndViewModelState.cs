using UnityEngine;

namespace Assets.Scripts.GameSession.UIScripts.SessionModel
{
    public class GameEndViewModelState : IViewModelState
    {
        public ViewModelStateType Name => ViewModelStateType.GameEnd;
        private readonly ViewModel _viewModel;
        private readonly IViewController _viewController;
        public GameEndViewModelState(ViewModel viewModel, IViewController viewController)
        {
            _viewModel = viewModel;
            _viewController = viewController;
        }
        public void Menu()
        {
            _viewModel.IsFromGameEndScreen = true;
            _viewModel.ChangeState(ViewModelStateType.Menu);
            _viewController.OpenPauseScreen();
        }
        public void Resume()
        {
            Debug.LogWarning("Resume in Game End State");
        }
        public void Options()
        {
            Debug.LogWarning("Options in Game End State");
        }
        public void LevelUp()
        {
            Debug.LogWarning("LevelUp in Game End State");
        }
        public void Win()
        {
            Debug.LogWarning("Win in Game End State");
        }
        public void Lose()
        {
            Debug.LogWarning("Lose in Game End State");
        }
    }
}