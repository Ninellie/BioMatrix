using UnityEngine;

namespace Assets.Scripts.GameSession.View
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
        public void Menu()
        {
            _viewModel.ChangeState(ViewModelStateType.Menu);
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