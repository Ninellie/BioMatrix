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
        
        public void PauseScreen()
        {
            _viewModel.ChangeState(ViewModelStateType.Pause);
            _viewController.Freeze();
            _viewController.OpenPauseScreen();
        }
        
        public void Resume() => Debug.LogWarning($"Attempt to {nameof(Resume)} from {Name} state");
        public void Options() => Debug.LogWarning($"Attempt to {nameof(Options)} from {Name} state");

        public void LevelUp()
        {
            _viewModel.ChangeState(ViewModelStateType.LevelUp);
            _viewController.Freeze();
            _viewController.InitiateLevelUp();
        }

        public void Mutate()
        {
            _viewModel.ChangeState(ViewModelStateType.Mutation);
            _viewController.Freeze();
            _viewController.InitiateMutation();
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