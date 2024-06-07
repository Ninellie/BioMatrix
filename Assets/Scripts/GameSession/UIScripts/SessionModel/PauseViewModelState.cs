using UnityEngine;

namespace Assets.Scripts.GameSession.UIScripts.SessionModel
{
    public class PauseViewModelState : IViewModelState
    {
        public ViewModelStateType Name => ViewModelStateType.Menu;
        private readonly ViewModel _viewModel;
        private readonly IViewController _viewController;
        public PauseViewModelState(ViewModel viewModel, IViewController viewController)
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
            
            if (_viewModel.IsFromLvlUpScreen || _viewModel.IsFromGameEndScreen)
            {
                if (_viewModel.IsFromGameEndScreen)
                {
                    _viewModel.ChangeState(ViewModelStateType.GameEnd);
                }

                if (_viewModel.IsFromLvlUpScreen)
                {
                    _viewModel.ChangeState(ViewModelStateType.LevelUp);
                }
            }
            else
            {
                switch (prevState)
                {
                    case ViewModelStateType.GameEnd or ViewModelStateType.LevelUp:
                        _viewModel.ChangeState(prevState);
                        break;
                    case ViewModelStateType.Active or ViewModelStateType.Options:
                        _viewModel.ChangeState(ViewModelStateType.Active);
                        _viewController.Unfreeze();
                        break;
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