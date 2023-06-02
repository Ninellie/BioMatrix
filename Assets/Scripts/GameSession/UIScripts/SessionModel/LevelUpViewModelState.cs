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
            _viewModel.ChangeState(ViewModelStateType.Menu);
            _viewController.OpenMenu();
        }
        public void Resume()
        {
            _viewModel.ChangeState(ViewModelStateType.Active);
            _viewController.CloseLevelUp();
            _viewController.Unfreeze();
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