using UnityEngine;

namespace UIScripts.SessionModel
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
        
        public void PauseScreen()
        {
            _viewModel.IsFromGameEndScreen = true;
            _viewModel.ChangeState(ViewModelStateType.Pause);
            _viewController.OpenPauseScreen();
        }
        
        public void Resume() => Debug.LogWarning($"Attempt to {nameof(Resume)} from {Name} state");
        public void Options() => Debug.LogWarning($"Attempt to {nameof(Options)} from {Name} state");
        public void LevelUp() => Debug.LogWarning($"Attempt to {nameof(LevelUp)} from {Name} state");
        public void Mutate() => Debug.LogWarning($"Attempt to {nameof(Mutate)} from {Name} state");
        public void Win() => Debug.LogWarning($"Attempt to {nameof(Win)} from {Name} state");
        public void Lose() => Debug.LogWarning($"Attempt to {nameof(Lose)} from {Name} state");
    }
}