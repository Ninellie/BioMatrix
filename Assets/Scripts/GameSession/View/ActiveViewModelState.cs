using UnityEngine;

namespace Assets.Scripts.Map
{
    public class ActiveViewModelState : IViewModelState
    {
        public ViewModelStateType Name => ViewModelStateType.Active;
        private readonly ViewModel _viewModel;
        private readonly IViewController _viewController;
        public ActiveViewModelState(ViewModel viewModel, IViewController viewController)
        {
            _viewModel = viewModel;
            _viewController = viewController;
        }
        public void Menu()
        {
            _viewModel.ChangeState(ViewModelStateType.Menu);
            _viewController.Freeze();
            _viewController.OpenMenu();
        }
        public void Resume()
        {
            Debug.LogWarning("Massage");
        }
        public void Options()
        {
            Debug.LogWarning("Massage");
        }
        public void LevelUp()
        {
            _viewModel.ChangeState(ViewModelStateType.LevelUp);
            _viewController.Freeze();
            _viewController.InitiateLevelUp();
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