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
        public void Menu() { _map.OpenMenu(); }
        public void Resume() { _map.Resume(); }
        public void Options() { _map.OpenOptions(); }
        public void LevelUp() { _map.InitiateLevelUp(); }
        public void Win() { _map.Win(); }
        public void Lose() { _map.Lose(); }

        private IMapController CreateMapController()
        {
            if (!IsUIObjectsNotNull()) return null;
            var currentPlayer = FindObjectOfType<Player>();
            var playerInput = currentPlayer.GetComponent<PlayerInput>();
            var mapController = new SimpleMapController(playerInput, _menuUi, _optionsUi, _levelUpUi, _winScreenUi, _loseScreenUi);
            return mapController;
        }

        private bool IsUIObjectsNotNull()
        {
            var isNotNull = true;
            if (_menuUi is null)
            {
                Debug.LogWarning("_menuUI is null");
                isNotNull = false;
            }
            if (_optionsUi is null)
            {
                Debug.LogWarning("_optionsUi is null");
                isNotNull = false;
            }
            if (_levelUpUi is null)
            {
                Debug.LogWarning("_levelUpUi is null");
                isNotNull = false;
            }
            if (_winScreenUi is null)
            {
                Debug.LogWarning("_winScreenUi is null");
                isNotNull = false;
            }
            if (_loseScreenUi is null)
            {
                Debug.LogWarning("_loseScreenUi is null");
                isNotNull = false;
            }
            return isNotNull;
        }
    }
}