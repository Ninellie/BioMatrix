using UnityEngine;

namespace Assets.Scripts.Map
{
    public class GameEndMapState : IMapState
    {
        public MapStateType Name => MapStateType.GameEnd;
        private readonly Map _map;
        private readonly IMapController _mapController;
        public GameEndMapState(Map map, IMapController mapController)
        {
            _map = map;
            _mapController = mapController;
        }
        public void Menu()
        {
            _map.ChangeState(MapStateType.Menu);
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