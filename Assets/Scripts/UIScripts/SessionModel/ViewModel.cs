namespace UIScripts.SessionModel
{
    public class ViewModel
    {   
        public bool IsFromLvlUpScreen { get; set; }
        public bool IsFromMutationsScreen { get; set; }
        public bool IsFromGameEndScreen { get; set; }

        public bool OptionsFromSpecialScreen
        {
            get
            {
                var isFromOptions = _previousState.Name == ViewModelStateType.Options;
                var isFromSpecialScreen = IsFromLvlUpScreen || IsFromMutationsScreen || IsFromGameEndScreen;
                return isFromOptions && isFromSpecialScreen;
            }
        }

        private readonly IViewModelState _gameplay;
        private readonly IViewModelState _options;
        private readonly IViewModelState _menuState;
        private readonly IViewModelState _gameEnd;
        private readonly IViewModelState _levelUp;
        private readonly IViewModelState _start;
        private readonly IViewModelState _mutation;

        private IViewModelState _currentState;
        private IViewModelState _previousState;

        public ViewModel(IViewController viewController)
        {
            _gameplay = new GameplayViewModelState(this, viewController);
            _options = new OptionsViewModelState(this, viewController);
            _menuState = new PauseViewModelState(this, viewController);
            _gameEnd = new GameEndViewModelState(this, viewController);
            _levelUp = new LevelUpViewModelState(this, viewController);
            _start = new StartViewModelState(this, viewController);
            _mutation = new MutationViewModelState(this, viewController);
            
            _currentState = _start;
            _previousState = _gameplay;

            IsFromLvlUpScreen = false;
            IsFromGameEndScreen = false;
            IsFromMutationsScreen = false;
        }

        public void ChangeState(ViewModelStateType stateType)
        {
            _previousState = _currentState;
            var mapStates = new[] {_gameplay, _options, _menuState, _gameEnd, _levelUp, _start, _mutation};
            foreach (var t in mapStates)
            {
                if (t.Name == stateType)
                {
                    _currentState = t;
                }
            }
        }

        public IViewModelState GetCurrentState() => _currentState;
        public IViewModelState GetPreviousState() => _previousState;
    }
}