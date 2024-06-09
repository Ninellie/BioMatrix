using System;
using Core.Events;
using UnityEngine;

namespace Assets.Scripts.GameSession.UIScripts.SessionModel
{
    [Serializable]
    public class ViewControllerOptions
    {
        [Header("Screen objects")]
        [SerializeField] private GameObject _startScreen;
        [SerializeField] private GameObject _pauseScreen;
        [SerializeField] private GameObject _optionsScreen;
        [SerializeField] private GameObject _levelUpScreen;
        [SerializeField] private GameObject _loseScreen;
        [SerializeField] private GameObject _winScreen;

        public GameObject StartScreen => _startScreen;
        public GameObject PauseScreen => _pauseScreen;
        public GameObject OptionsScreen => _optionsScreen;
        public GameObject LevelUpScreen => _levelUpScreen;
        public GameObject LoseScreen => _loseScreen;
        public GameObject WinScreen => _winScreen;

        [Header("Event assets")]
        [SerializeField] private GameEvent _pauseMapEvent;
        [SerializeField] private GameEvent _gameplayMapEvent;
        [SerializeField] private GameEvent _levelUpMapEvent;
        [SerializeField] private GameEvent _repulseEvent;

        public GameEvent PauseMapEvent => _pauseMapEvent;
        public GameEvent GameplayMapEvent => _gameplayMapEvent;
        public GameEvent LevelUpMapEvent => _levelUpMapEvent;
        public GameEvent RepulseEvent => _repulseEvent;
    }
}