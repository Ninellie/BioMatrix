using UnityEngine;

namespace Assets.Scripts.Map
{
    public class ActiveMapState : IMapState
    {
        public MapState Name => MapState.Active;
        public void Menu(Map map, IMapController mapController)
        {
            map.ChangeState(MapState.Menu);
            mapController.Freeze();
            mapController.OpenMenu();
        }
        public void Resume(Map map, IMapController mapController)
        {
            Debug.LogWarning("Massage");
        }
        public void Options(Map map, IMapController mapController)
        {
            Debug.LogWarning("Massage");
        }
        public void LevelUp(Map map, IMapController mapController)
        {
            map.ChangeState(MapState.LevelUp);
            mapController.Freeze();
            mapController.InitiateLevelUp();
        }
        public void Win(Map map, IMapController mapController)
        {
            map.ChangeState(MapState.GameEnd);
            mapController.Freeze();
            mapController.OpenWinScreen();
        }
        public void Lose(Map map, IMapController mapController)
        {
            map.ChangeState(MapState.GameEnd);
            mapController.Freeze();
            mapController.OpenLoseScreen();
        }
    }
}