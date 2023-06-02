namespace Assets.Scripts.GameSession.UIScripts.SessionModel
{
    public class ViewModel
    {   
        private readonly IViewModelState _active;
        private readonly IViewModelState _options;
        private readonly IViewModelState _menuState;
        private readonly IViewModelState _gameEnd;
        private readonly IViewModelState _levelUp;
        private readonly IViewModelState _start;

        private IViewModelState _currentState;
        private IViewModelState _previousState;
        public ViewModel(IViewController viewController)
        {
            _active = new ActiveViewModelState(this, viewController);
            _options = new OptionsViewModelState(this, viewController);
            _menuState = new MenuViewModelState(this, viewController);
            _gameEnd = new GameEndViewModelState(this, viewController);
            _levelUp = new LevelUpViewModelState(this, viewController);
            _start = new StartViewModelState(this, viewController);
            
            _currentState = _start;
            _previousState = _active;
        }
        public void ChangeState(ViewModelStateType stateType)
        {
            _previousState = _currentState;
            var mapStates = new[] {_active, _options, _menuState, _gameEnd, _levelUp, _start};
            foreach (var t in mapStates)
            {
                if (t.Name == stateType)
                {
                    _currentState = t;
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