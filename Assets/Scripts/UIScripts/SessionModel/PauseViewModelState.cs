using System;
using UnityEngine;

namespace UIScripts.SessionModel
{
    public class PauseViewModelState : IViewModelState
    {
        public ViewModelStateType Name => ViewModelStateType.Pause;
        private readonly ViewModel _viewModel;
        private readonly IViewController _viewController;
        
        public PauseViewModelState(ViewModel viewModel, IViewController viewController)
        {
            _viewModel = viewModel;
            _viewController = viewController;
        }
        
        public void PauseScreen() => Resume();

        public void Resume()
        {
            var prevState = _viewModel.GetPreviousState().Name;

            if (_viewModel.OptionsFromSpecialScreen)
            {
                if (_viewModel.IsFromMutationsScreen)
                {
                    _viewModel.ChangeState(ViewModelStateType.Mutation);    
                }
                if (_viewModel.IsFromLvlUpScreen)
                {
                    _viewModel.ChangeState(ViewModelStateType.LevelUp);
                }
                if (_viewModel.IsFromGameEndScreen)
                {
                    _viewModel.ChangeState(ViewModelStateType.GameEnd);
                }
            }
            else
            {
                switch (prevState)
                {
                    case ViewModelStateType.Active:
                        _viewModel.ChangeState(ViewModelStateType.Active);
                        _viewController.Unfreeze();
                        break;
                    case ViewModelStateType.Options:
                        _viewModel.ChangeState(ViewModelStateType.Active);
                        _viewController.Unfreeze();
                        break;
                    case ViewModelStateType.Pause:
                        Debug.LogError($"previous state equals current state. PrevState: {prevState}. CurrentState: {Name}");
                        break;
                    case ViewModelStateType.GameEnd:
                        _viewModel.ChangeState(ViewModelStateType.GameEnd);
                        break;
                    case ViewModelStateType.LevelUp:
                        _viewModel.ChangeState(ViewModelStateType.LevelUp);
                        break;
                    case ViewModelStateType.Mutation:
                        _viewModel.ChangeState(ViewModelStateType.Mutation);
                        break;
                    case ViewModelStateType.Start:
                        _viewModel.ChangeState(ViewModelStateType.Start);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            _viewController.ClosePauseScreen();
        }
        
        public void Options()
        {
            _viewModel.ChangeState(ViewModelStateType.Options);
            _viewController.ClosePauseScreen();
            _viewController.OpenOptions();
        }
        
        public void LevelUp() => Debug.LogWarning($"Attempt to {nameof(LevelUp)} from {Name} state");
        public void Mutate() => Debug.LogWarning($"Attempt to {nameof(Mutate)} from {Name} state");
        public void Win() => Debug.LogWarning($"Attempt to {nameof(Win)} from {Name} state");
        public void Lose() => Debug.LogWarning($"Attempt to {nameof(Lose)} from {Name} state");
    }
}