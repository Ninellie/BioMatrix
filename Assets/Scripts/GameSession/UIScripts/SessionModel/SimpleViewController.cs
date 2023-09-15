using System;
using Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.GameSession.UIScripts.SessionModel
{
    public class SimpleViewController : IViewController
    {
        private readonly PlayerInput _playerInput;
        private readonly Player _player;
        private GameObject _menuUI;
        private GameObject _optionsUI;
        private GameObject _levelUpUI;
        private ILevelUpController _levelUpController;
        private GameObject _winScreenUI;
        private GameObject _loseScreenUI;
        private GameObject _startScreenUI;

        public SimpleViewController(PlayerInput playerInput, GameObject menuUI, GameObject optionsUI, GameObject levelUpUI, GameObject winScreenUI, GameObject loseScreenUI, GameObject startScreenUI)
        {
            _player = playerInput.gameObject.GetComponent<Player>();
            _playerInput = playerInput ?? throw new ArgumentNullException(nameof(playerInput));
            _menuUI = menuUI ?? throw new ArgumentNullException(nameof(menuUI));
            _optionsUI = optionsUI ?? throw new ArgumentNullException(nameof(optionsUI));
            _levelUpUI = levelUpUI ?? throw new ArgumentNullException(nameof(levelUpUI));
            _levelUpController = _levelUpUI.GetComponent<ILevelUpController>();
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
        public void Repulse()
        {
            _player.Shield.Layers.Increase();
            _player.Shield.Layers.Decrease();
        }
        public void OpenMenu() => _menuUI.SetActive(true);
        public void CloseMenu() => _menuUI.SetActive(false);
        public void CloseStartScreen() => _startScreenUI.SetActive(false);
        public void OpenOptions() => _optionsUI.SetActive(true);
        public void CloseOptions() => _optionsUI.SetActive(false);
        public void OpenWinScreen() => _winScreenUI.SetActive(true);
        public void OpenLoseScreen() => _loseScreenUI.SetActive(true);
        public void CloseLevelUp()
        {
            _playerInput.SwitchCurrentActionMap("Player");
            _levelUpUI.SetActive(false);
        }

        //public void GetBonusToPlayer()
        //{
        //    _levelUpController
        //    _player.
        //}

        public void InitiateLevelUp()
        {
            _playerInput.SwitchCurrentActionMap("LevelUpScreen");
            _levelUpUI.SetActive(true);
            _levelUpController.Initiate();
            //_levelUp.DisplayCards();
        }
    }
}