using UnityEngine;

namespace Assets.Scripts.Map
{
    public class GameEndMapState : IMapState
    {
        public MapState Name => MapState.GameEnd;
        public void Menu(Map map, IMapController mapController)
        {
            map.ChangeState(MapState.Menu);
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