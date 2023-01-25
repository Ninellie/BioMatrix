using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Map
{
    public class ScreenUIController : MonoBehaviour
    {
        [SerializeField] private GameObject _menuUi;
        [SerializeField] private GameObject _optionsUi;
        [SerializeField] private GameObject _levelUpUi;
        [SerializeField] private GameObject _winScreenUi;
        [SerializeField] private GameObject _loseScreenUi;
        private Map _map;

        public void AwakeController()
        {
            var mapController = CreateMapController();
            _map = new Map(mapController);
        }
        public void Menu() { _map.GetCurrentState().Menu(); }
        public void Resume() { _map.GetCurrentState().Resume(); }
        public void Options() { _map.GetCurrentState().Options(); }
        public void LevelUp() { _map.GetCurrentState().LevelUp(); }
        public void Win() { _map.GetCurrentState().Win(); }
        public void Lose() { _map.GetCurrentState().Lose(); }

        private IMapController CreateMapController()
        {
            var currentPlayer = FindObjectOfType<Player>();
            var playerInput = currentPlayer.GetComponent<PlayerInput>();
            var mapController = new SimpleMapController(playerInput, _menuUi, _optionsUi, _levelUpUi, _winScreenUi, _loseScreenUi);
            return mapController;
        }
    }
}