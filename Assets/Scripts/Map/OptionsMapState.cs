using UnityEngine;

namespace Assets.Scripts.Map
{
    public class OptionsMapState : IMapState
    {
        public MapStateType Name => MapStateType.Options;
        private readonly Map _map;
        private readonly IMapController _mapController;
        public OptionsMapState(Map map, IMapController mapController)
        {
            _map = map;
            _mapController = mapController;
        }
        public void Menu()
        {
            _map.ChangeState(MapStateType.Menu);
            _mapController.CloseOptions();
            _mapController.OpenMenu();
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