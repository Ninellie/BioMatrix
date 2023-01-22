using UnityEngine;

namespace Assets.Scripts.Map
{
    public class LevelUpMapState : IMapState
    {
        public MapState Name => MapState.LevelUp;
        public void Menu(Map map, IMapController mapController)
        {
            map.ChangeState(MapState.Menu);
            mapController.OpenMenu();
        }
        public void Resume(Map map, IMapController mapController)
        {
            map.ChangeState(MapState.Active);
            mapController.CloseLevelUp();
            mapController.Unfreeze();
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