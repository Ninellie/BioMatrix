using System;
using System.Net.Sockets;

namespace Assets.Scripts.Map
{
    public class Map
    {   
        private static readonly IMapState Active = new ActiveMapState();
        private static readonly IMapState Options = new OptionsMapState();
        private static readonly IMapState MenuState = new MenuMapState();
        private static readonly IMapState GameEnd = new GameEndMapState();
        private static readonly IMapState LevelUp = new LevelUpMapState();
        private IMapState _currentState = Active;
        private IMapState _previousState = Active;
        private readonly IMapController _mapController;
        public Map(IMapController mapController)
        {
            _mapController = mapController;
        }
        public void ChangeState(MapState state)
        {
            _previousState = _currentState;
            var mapStates = new[] {Active, Options, MenuState, GameEnd, LevelUp};
            for (var i = 0; i < mapStates.Length; i++)
            {
                if (mapStates[i].Name == state)
                {
                    _currentState = mapStates[i];
                }
            }
        }
        public MapState GetCurrentState()
        {
            return _currentState.Name;
        }
        public MapState GetPreviousState()
        {
            return _previousState.Name;
        }
        public void OpenMenu() { _currentState.Menu(this, _mapController); }
        public void Resume() { _currentState.Resume(this, _mapController); }
        public void OpenOptions() { _currentState.Options(this, _mapController); }
        public void InitiateLevelUp() { _currentState.LevelUp(this, _mapController); }
        public void Win() { _currentState.Win(this, _mapController); }
        public void Lose() { _currentState.Lose(this, _mapController); }
    }
}