using UnityEngine;

namespace UIScripts.SessionModel
{
    public class GameplayViewModelState : IViewModelState
    {
        public ViewModelStateType Name => ViewModelStateType.Active;
        private readonly ViewModel _viewModel;
        private readonly IViewController _viewController;
        public GameplayViewModelState(ViewModel viewModel, IViewController viewController)
        {
            _viewModel = viewModel;
            _viewController = viewController;
        }
        public void Menu()
        {
            _viewModel.ChangeState(ViewModelStateType.Menu);
            _viewController.Freeze();
            _viewController.OpenPauseScreen();
        }
        public void Resume()
        {
            Debug.LogWarning("Message");
        }
        public void Options()
        {
            Debug.LogWarning("Message");
        }
        public void LevelUp()
        {
            _viewModel.ChangeState(ViewModelStateType.LevelUp);
            _viewController.Freeze();
            _viewController.InitiateLevelUp();
        }
        public void Win()
        {
            _viewModel.ChangeState(ViewModelStateType.GameEnd);
            _viewController.Freeze();
            _viewController.OpenWinScreen();
        }
        public void Lose()
        {
            _viewModel.ChangeState(ViewModelStateType.GameEnd);
            _viewController.Freeze();
            _viewController.OpenLoseScreen();
        }
    }
}