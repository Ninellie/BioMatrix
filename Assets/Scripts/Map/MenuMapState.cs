using System;
using UnityEngine;

namespace Assets.Scripts.Map
{
    public class MenuMapState : IMapState
    {
        public MapState Name => MapState.Menu;
        public void Menu(Map map, IMapController mapController)
        {
            Resume(map, mapController);
        }
        public void Resume(Map map, IMapController mapController)
        {
            var prevState = map.GetPreviousState();
            switch (prevState)
            {
                case MapState.GameEnd or MapState.LevelUp:
                    map.ChangeState(prevState);
                    break;
                case MapState.Active or MapState.Options:
                    map.ChangeState(MapState.Active);
                    mapController.Unfreeze();
                    break;
            }
            mapController.CloseMenu();
        }
        public void Options(Map map, IMapController mapController)
        {
            map.ChangeState(MapState.Options);
            mapController.CloseMenu();
            mapController.OpenOptions();
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