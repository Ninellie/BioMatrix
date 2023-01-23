namespace Assets.Scripts.Map
{
    public class Map
    {   
        private readonly IMapState _active;
        private readonly IMapState _options;
        private readonly IMapState _menuState;
        private readonly IMapState _gameEnd;
        private readonly IMapState _levelUp;

        private IMapState _currentState;
        private IMapState _previousState;

        private readonly IMapController _mapController;
        public Map(IMapController mapController)
        {
            _mapController = mapController;
            _active = new ActiveMapState(this, mapController);
            _options = new OptionsMapState(this, mapController);
            _menuState = new MenuMapState(this, mapController);
            _gameEnd = new GameEndMapState(this, mapController);
            _levelUp = new LevelUpMapState(this, mapController);
            
            _currentState = _active;
            _previousState = _active;
        }
        public void ChangeState(MapStateType stateType)
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
        public IMapState GetCurrentState()
        {
            return _currentState;
        }
        public IMapState GetPreviousState()
        {
            return _previousState;
        }
        public void OpenMenu() { _currentState.Menu(); }
        public void Resume() { _currentState.Resume(); }
        public void OpenOptions() { _currentState.Options(); }
        public void InitiateLevelUp() { _currentState.LevelUp(); }
        public void Win() { _currentState.Win(); }
        public void Lose() { _currentState.Lose(); }
    }
}