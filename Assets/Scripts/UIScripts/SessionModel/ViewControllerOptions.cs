using System;
using Core.Events;
using UnityEngine;

namespace UIScripts.SessionModel
{
    [Serializable]
    public class ViewControllerOptions
    {
        [Header("Screen objects")]
        [SerializeField] private GameObject startScreen;
        [SerializeField] private GameObject pauseScreen;
        [SerializeField] private GameObject optionsScreen;
        [SerializeField] private GameObject levelUpScreen;
        [SerializeField] private GameObject mutationScreen;
        [SerializeField] private GameObject loseScreen;
        [SerializeField] private GameObject winScreen;

        public GameObject StartScreen => startScreen;
        public GameObject PauseScreen => pauseScreen;
        public GameObject OptionsScreen => optionsScreen;
        public GameObject LevelUpScreen => levelUpScreen;
        public GameObject MutationScreen => mutationScreen;
        public GameObject LoseScreen => loseScreen;
        public GameObject WinScreen => winScreen;

        [Header("Event assets")]
        [SerializeField] private GameEvent pauseMapEvent;
        [SerializeField] private GameEvent gameplayMapEvent;
        [SerializeField] private GameEvent levelUpMapEvent;
        [SerializeField] private GameEvent repulseEvent;

        public GameEvent PauseMapEvent => pauseMapEvent;
        public GameEvent GameplayMapEvent => gameplayMapEvent;
        public GameEvent LevelUpMapEvent => levelUpMapEvent;
        public GameEvent RepulseEvent => repulseEvent;
    }
}