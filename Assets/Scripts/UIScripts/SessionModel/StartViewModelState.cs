using UnityEngine;

namespace UIScripts.SessionModel
{
    public class StartViewModelState : IViewModelState
    {
        public ViewModelStateType Name => ViewModelStateType.Start;
        private readonly ViewModel _viewModel;
        private readonly IViewController _viewController;
        public StartViewModelState(ViewModel viewModel, IViewController viewController)
        {
            _viewModel = viewModel;
            _viewController = viewController;
        }
        public void Menu()
        {
            Resume();
        }
        public void Resume()
        {
            _viewController.Unfreeze();
            _viewModel.ChangeState(ViewModelStateType.Active);
            _viewController.CloseStartScreen();
        }
        public void Options()
        {
            Debug.LogWarning("Message");
        }
        public void LevelUp()
        {
            Debug.LogWarning("Message");
        }
        public void Win()
        {
            Debug.LogWarning("Message");
        }
        public void Lose()
        {
            Debug.LogWarning("Message");
        }
    }
}