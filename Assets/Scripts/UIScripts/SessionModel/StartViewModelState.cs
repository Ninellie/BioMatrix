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
        
        public void PauseScreen() => Resume();
        
        public void Resume()
        {
            _viewController.Unfreeze();
            _viewModel.ChangeState(ViewModelStateType.Active);
            _viewController.CloseStartScreen();
        }

        public void Options() => Debug.LogWarning($"Attempt to {nameof(Options)} from {Name} state");
        public void LevelUp() => Debug.LogWarning($"Attempt to {nameof(LevelUp)} from {Name} state");
        public void Mutate() => Debug.LogWarning($"Attempt to {nameof(Mutate)} from {Name} state");
        public void Win() => Debug.LogWarning($"Attempt to {nameof(Win)} from {Name} state");
        public void Lose() => Debug.LogWarning($"Attempt to {nameof(Lose)} from {Name} state");
    }
}