using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Map
{
    public class SimpleMapController : IMapController
    {
        private PlayerInput _playerInput;
        private GameTimer _gameTimer;
        private LevelUp _levelUp;

        private GameObject _menuUI;
        private GameObject _optionsUI;
        private GameObject _levelUpUI;
        private GameObject _winScreenUI;
        private GameObject _loseScreenUI;

        public SimpleMapController(PlayerInput playerInput, GameObject menuUI, GameObject optionsUI, GameObject levelUpUI, GameObject winScreenUI, GameObject loseScreenUI)
        {
            _playerInput = playerInput;
            _menuUI = menuUI;
            _optionsUI = optionsUI;
            _levelUpUI = levelUpUI;
            _winScreenUI = winScreenUI;
            _loseScreenUI = loseScreenUI;

        }
        public void Freeze()
        {
            Time.timeScale = 0f;
            GameTimerStop();
            _playerInput.SwitchCurrentActionMap("Menu");
        }
        public void Unfreeze()
        {
            Time.timeScale = 1f;
            GameTimerResume();
            _playerInput.SwitchCurrentActionMap("Player");
        }
        public void OpenMenu()
        {
            _menuUI.SetActive(true);
        }
        public void CloseMenu()
        {
            _menuUI.SetActive(false);
        }
        public void OpenOptions()
        {
            _optionsUI.SetActive(true);
        }
        public void CloseOptions()
        {
            _optionsUI.SetActive(false);
        }
        public void OpenWinScreen()
        {
            _winScreenUI.SetActive(true);
        }
        public void OpenLoseScreen()
        {
            _loseScreenUI.SetActive(true);
        }
        public void CloseLevelUp()
        {
            _levelUpUI.SetActive(false);
        }
        public void InitiateLevelUp()
        {
            _levelUpUI.SetActive(true);
            _levelUp ??= GameObject.FindObjectOfType<LevelUp>();
            _levelUp.DisplayCards();
        }
        private void GameTimerStop()
        {
            _gameTimer ??= GameObject.FindObjectOfType<GameTimer>();
            _gameTimer.Stop();
        }
        private void GameTimerResume()
        {
            _gameTimer ??= GameObject.FindObjectOfType<GameTimer>();
            _gameTimer.Resume();
        }
    }
}