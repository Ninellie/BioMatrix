using UnityEngine;

namespace UIScripts.SessionModel
{
    public class MutationViewModelState : IViewModelState
    {
        public ViewModelStateType Name => ViewModelStateType.Mutation;
        private readonly ViewModel _viewModel;
        private readonly IViewController _viewController;

        public MutationViewModelState(ViewModel viewModel, IViewController viewController)
        {
            _viewModel = viewModel;
            _viewController = viewController;
        }

        public void PauseScreen()
        {
            _viewModel.IsFromMutationsScreen = true;
            _viewModel.ChangeState(ViewModelStateType.Pause);
            _viewController.OpenPauseScreen();
        }

        public void Resume()
        {
            _viewModel.IsFromMutationsScreen = false;
            _viewModel.ChangeState(ViewModelStateType.Active);
            _viewController.CloseMutationScreen();
            _viewController.Unfreeze();
            _viewController.Repulse();
        }

        public void Options() => Debug.LogWarning($"Attempt to {nameof(Options)} from {Name} state");
        public void LevelUp() => Debug.LogWarning($"Attempt to {nameof(LevelUp)} from {Name} state");
        public void Mutate() => Debug.LogWarning($"Attempt to {nameof(Mutate)} from {Name} state");
        public void Win() => Debug.LogWarning($"Attempt to {nameof(Win)} from {Name} state");
        public void Lose() => Debug.LogWarning($"Attempt to {nameof(Lose)} from {Name} state");
    }
}