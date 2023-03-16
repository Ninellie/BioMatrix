using UnityEngine;

namespace Assets.Scripts.GameSession.View
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
            var prevState = _viewModel.GetPreviousState().Name;
            _viewController.Unfreeze();
            _viewModel.ChangeState(ViewModelStateType.Active);
            _viewController.CloseStartScreen();
        }
        public void Options()
        {
            Debug.LogWarning("Massage");
        }
        public void LevelUp()
        {
            Debug.LogWarning("Massage");
        }
        public void Win()
        {
            Debug.LogWarning("Massage");
        }
        public void Lose()
        {
            Debug.LogWarning("Massage");
        }
    }
}