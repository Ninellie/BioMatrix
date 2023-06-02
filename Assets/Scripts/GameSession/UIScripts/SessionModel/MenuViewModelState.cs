using Assets.Scripts.GameSession.View;
using UnityEngine;

namespace Assets.Scripts.GameSession.UIScripts.SessionModel
{
    public class MenuViewModelState : IViewModelState
    {
        public ViewModelStateType Name => ViewModelStateType.Menu;
        private readonly ViewModel _viewModel;
        private readonly IViewController _viewController;
        public MenuViewModelState(ViewModel viewModel, IViewController viewController)
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
            _viewController.CloseMenu();
        }
        public void Options()
        {
            _viewModel.ChangeState(ViewModelStateType.Options);
            _viewController.CloseMenu();
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