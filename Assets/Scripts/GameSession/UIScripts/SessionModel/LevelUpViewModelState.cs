using UnityEngine;

namespace Assets.Scripts.GameSession.UIScripts.SessionModel
{
    public class LevelUpViewModelState : IViewModelState
    {
        public ViewModelStateType Name => ViewModelStateType.LevelUp;
        private readonly ViewModel _viewModel;
        private readonly IViewController _viewController;

        public LevelUpViewModelState(ViewModel viewModel, IViewController viewController)
        {
            _viewModel = viewModel;
            _viewController = viewController;
        }

        public void Menu()

        {
            _viewModel.IsFromLvlUpScreen = true;
            _viewModel.ChangeState(ViewModelStateType.Menu);
            _viewController.OpenMenu();
        }

        public void Resume()
        {
            _viewModel.IsFromLvlUpScreen = false;
            _viewModel.ChangeState(ViewModelStateType.Active);
            _viewController.CloseLevelUp();
            _viewController.Unfreeze();
            _viewController.Repulse();
        }

        public void Options() => Debug.LogWarning("This state model does not implement this method");
        public void LevelUp() => Debug.LogWarning("This state model does not implement this method");
        public void Win() => Debug.LogWarning("This state model does not implement this method");
        public void Lose() => Debug.LogWarning("This state model does not implement this method");
    }
}