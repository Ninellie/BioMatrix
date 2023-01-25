using UnityEngine;

namespace Assets.Scripts.Map
{
    public class LevelUpMapState : IMapState
    {
        public MapStateType Name => MapStateType.LevelUp;
        private readonly Map _map;
        private readonly IMapController _mapController;
        public LevelUpMapState(Map map, IMapController mapController)
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
            _map.ChangeState(MapStateType.Active);
            _mapController.CloseLevelUp();
            _mapController.Unfreeze();
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