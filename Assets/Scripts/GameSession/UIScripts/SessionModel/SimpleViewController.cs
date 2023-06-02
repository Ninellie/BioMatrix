using System;
using Assets.Scripts.GameSession.UIScripts.View;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.GameSession.UIScripts.SessionModel
{
    public class SimpleViewController : IViewController
    {
        private readonly PlayerInput _playerInput;
        private LevelUp _levelUp;

        private GameObject _menuUI;
        private GameObject _optionsUI;
        private GameObject _levelUpUI;
        private GameObject _winScreenUI;
        private GameObject _loseScreenUI;
        private GameObject _startScreenUI;

        public SimpleViewController(PlayerInput playerInput, GameObject menuUI, GameObject optionsUI, GameObject levelUpUI, GameObject winScreenUI, GameObject loseScreenUI, GameObject startScreenUI)
        {
            _playerInput = playerInput ?? throw new ArgumentNullException(nameof(playerInput));
            _menuUI = menuUI ?? throw new ArgumentNullException(nameof(menuUI));
            _optionsUI = optionsUI ?? throw new ArgumentNullException(nameof(optionsUI));
            _levelUpUI = levelUpUI ?? throw new ArgumentNullException(nameof(levelUpUI));
            _winScreenUI = winScreenUI ?? throw new ArgumentNullException(nameof(winScreenUI));
            _loseScreenUI = loseScreenUI ?? throw new ArgumentNullException(nameof(loseScreenUI));
            _startScreenUI = startScreenUI ?? throw new ArgumentNullException(nameof(loseScreenUI));
        }
        public void Freeze()
        {
            Time.timeScale = 0f;
            _playerInput.SwitchCurrentActionMap("Menu");
        }
        public void Unfreeze()
        {
            Time.timeScale = 1f;
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
        public void CloseStartScreen()
        {
            _startScreenUI.SetActive(false);
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
    }
}