using UnityEngine;

namespace Assets.Scripts.Map
{
    public class ActiveMapState : IMapState
    {
        public MapStateType Name => MapStateType.Active;
        private readonly Map _map;
        private readonly IMapController _mapController;
        public ActiveMapState(Map map, IMapController mapController)
        {
            _map = map;
            _mapController = mapController;
        }
        public void Menu()
        {
            _map.ChangeState(MapStateType.Menu);
            _mapController.Freeze();
            _mapController.OpenMenu();
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
            _map.ChangeState(MapStateType.LevelUp);
            _mapController.Freeze();
            _mapController.InitiateLevelUp();
        }
        public void Win()
        {
            _map.ChangeState(MapStateType.GameEnd);
            _mapController.Freeze();
            _mapController.OpenWinScreen();
        }
        public void Lose()
        {
            _map.ChangeState(MapStateType.GameEnd);
            _mapController.Freeze();
            _mapController.OpenLoseScreen();
        }
    }
}