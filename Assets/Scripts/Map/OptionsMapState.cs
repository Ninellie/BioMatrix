using UnityEngine;

namespace Assets.Scripts.Map
{
    public class OptionsMapState : IMapState
    {
        public MapState Name => MapState.Options;
        public void Menu(Map map, IMapController mapController)
        {
            map.ChangeState(MapState.Menu);
            mapController.CloseOptions();
            mapController.OpenMenu();
        }
        public void Resume(Map map, IMapController mapController)
        {
            Menu(map, mapController);
        }
        public void Options(Map map, IMapController mapController)
        {
            Debug.LogWarning("Massage");
        }
        public void LevelUp(Map map, IMapController mapController)
        {
            Debug.LogWarning("Massage");
        }
        public void Win(Map map, IMapController mapController)
        {
            Debug.LogWarning("Massage");
        }
        public void Lose(Map map, IMapController mapController)
        {
            Debug.LogWarning("Massage");
        }
    }
}