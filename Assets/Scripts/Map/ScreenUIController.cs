using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Map
{
    public class ScreenUIController : MonoBehaviour 
    {
        public GameObject menuUI;
        public GameObject optionsUI;
        public GameObject levelUpUI;
        public GameObject winScreenUI;
        public GameObject loseScreenUI;
        public Map map;

        private Player currentPlayer;

        public void AwakeController()
        {
            currentPlayer = FindObjectOfType<Player>();
            var playerInput = currentPlayer.GetComponent<PlayerInput>();
            var mapController =
                new SimpleMapController(playerInput, menuUI, optionsUI, levelUpUI, winScreenUI, loseScreenUI);
            map = new Map(mapController);
        }
        public void Menu() { map.Menu(); }
        public void Resume() { map.Resume(); }
        public void Options() { map.Options(); }
        public void LevelUp() { map.LevelUp(); }
        public void Win() { map.Win(); }
        public void Lose() { map.Lose(); }
    }
}