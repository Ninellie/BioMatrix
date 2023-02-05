namespace Assets.Scripts.Map
{
    public class ViewModel
    {   
        private readonly IViewModelState _active;
        private readonly IViewModelState _options;
        private readonly IViewModelState _menuState;
        private readonly IViewModelState _gameEnd;
        private readonly IViewModelState _levelUp;

        private IViewModelState _currentState;
        private IViewModelState _previousState;

        private readonly IViewController _viewController;
        public ViewModel(IViewController viewController)
        {
            _viewController = viewController;
            _active = new ActiveViewModelState(this, viewController);
            _options = new OptionsViewModelState(this, viewController);
            _menuState = new MenuViewModelState(this, viewController);
            _gameEnd = new GameEndViewModelState(this, viewController);
            _levelUp = new LevelUpViewModelState(this, viewController);
            
            _currentState = _active;
            _previousState = _active;
        }
        public void ChangeState(ViewModelStateType stateType)
        {
            _previousState = _currentState;
            var mapStates = new[] {_active, _options, _menuState, _gameEnd, _levelUp};
            for (var i = 0; i < mapStates.Length; i++)
            {
                if (mapStates[i].Name == stateType)
                {
                    _currentState = mapStates[i];
                }
            }
        }
        public IViewModelState GetCurrentState()
        {
            return _currentState;
        }
        public IViewModelState GetPreviousState()
        {
            return _previousState;
        }
    }
}