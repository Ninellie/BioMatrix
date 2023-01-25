using System;
using UnityEngine;

namespace Assets.Scripts.Map
{
    public class MenuMapState : IMapState
    {
        public MapStateType Name => MapStateType.Menu;
        private readonly Map _map;
        private readonly IMapController _mapController;
        public MenuMapState(Map map, IMapController mapController)
        {
            _map = map;
            _mapController = mapController;
        }
        public void Menu()
        {
            Resume();
        }
        public void Resume()
        {
            var prevState = _map.GetPreviousState().Name;
            switch (prevState)
            {
                case MapStateType.GameEnd or MapStateType.LevelUp:
                    _map.ChangeState(prevState);
                    break;
                case MapStateType.Active or MapStateType.Options:
                    _map.ChangeState(MapStateType.Active);
                    _mapController.Unfreeze();
                    break;
            }
            _mapController.CloseMenu();
        }
        public void Options()
        {
            _map.ChangeState(MapStateType.Options);
            _mapController.CloseMenu();
            _mapController.OpenOptions();
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