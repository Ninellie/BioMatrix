using UnityEngine;

namespace Assets.Scripts.Map
{
    public class OptionsViewModelState : IViewModelState
    {
        public ViewModelStateType Name => ViewModelStateType.Options;
        private readonly ViewModel _viewModel;
        private readonly IViewController _viewController;
        public OptionsViewModelState(ViewModel viewModel, IViewController viewController)
        {
            _viewModel = viewModel;
            _viewController = viewController;
        }
        public void Menu()
        {
            _viewModel.ChangeState(ViewModelStateType.Menu);
            _viewController.CloseOptions();
            _viewController.OpenMenu();
        }
        public void Resume()
        {
            Menu();
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