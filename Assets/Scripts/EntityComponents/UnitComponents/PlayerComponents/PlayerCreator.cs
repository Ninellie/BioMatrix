using System;
using Assets.Scripts.Core;
using Assets.Scripts.EntityComponents.UnitComponents.TurretComponents;
using Assets.Scripts.GameSession.UIScripts.SessionModel;
using Assets.Scripts.GameSession.UIScripts.View;
using Assets.Scripts.View;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents
{
    [Serializable]
    public class DisplayedResourceData
    {
        public string resourceName;
        public string resourcePath;
        public ResourceCounter resourceCounter;
    }

    public class PlayerCreator : MonoBehaviour
    {
        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private GameObject _playerWeapon;
        [SerializeField] private GameObject _turretHub;

        [SerializeField] private DisplayedResourceData[] _displayedResources;
        public Player CurrentPlayer { get; private set; }

        //private ExpUI _experienceBar;
        //private AmmoBar _ammoBar;
        //private LifeBar _lifeBar;
        private GameSessionTimer _gameSessionTimer;
        private OptionsMenu _optionsMenu;
        private ViewController _viewController;


        private void Awake()
        {
            _gameSessionTimer = FindObjectOfType<GameSessionTimer>();
            _viewController = FindObjectOfType<ViewController>();
            _optionsMenu = FindObjectOfType<OptionsMenu>();

            if (_playerPrefab == null)
            {
                Debug.LogWarning("PlayerPrefab is null");
                return;
            }

            CreatePlayer(_playerPrefab);

            CurrentPlayer.CreateWeapon(_playerWeapon);

            var turretHubPrefab = Instantiate(_turretHub);
            var turretHub = turretHubPrefab.GetComponent<TurretHub>();
            turretHub.SetHolder(CurrentPlayer);
            CurrentPlayer.TurretHub = turretHub;
            turretHubPrefab.transform.SetParent(CurrentPlayer.transform);

            _viewController.AwakeController(CurrentPlayer.GetComponent<PlayerInput>());
            Subscription();

            foreach (var displayedResourceData in _displayedResources)
            {
                var resource = (Resource)EventHelper.GetPropByPath(CurrentPlayer, displayedResourceData.resourcePath);
                displayedResourceData.resourceCounter.SetResource(resource);
                displayedResourceData.resourceCounter.SetLabel(displayedResourceData.resourceName);
            }
        }

        private void CreatePlayer(GameObject playerPrefab)
        {
            var player = Instantiate(playerPrefab, new Vector2(0, 0), Quaternion.identity);
            CurrentPlayer = player.GetComponent<Player>();
        }

        private void Subscription()
        {
            CurrentPlayer.GamePausedEvent += _viewController.Menu;
            CurrentPlayer.Lvl.IncrementEvent += _viewController.LevelUpEvent;
            CurrentPlayer.OnDeath += _viewController.Lose;
            _gameSessionTimer.onGameWinning += _viewController.Win;
            CurrentPlayer.OnDeath += Unsubscription;
            _gameSessionTimer.onGameWinning += Unsubscription;
            _optionsMenu.onBackToMainMenu += Unsubscription;
        }

        public void Unsubscription()
        {
            CurrentPlayer.GamePausedEvent -= _viewController.Menu;
            CurrentPlayer.Lvl.IncrementEvent -= _viewController.LevelUpEvent;
            CurrentPlayer.OnDeath -= _viewController.Lose;
            _gameSessionTimer.onGameWinning -= _viewController.Win;
            CurrentPlayer.OnDeath -= Unsubscription;
            _gameSessionTimer.onGameWinning -= Unsubscription;
            _optionsMenu.onBackToMainMenu -= Unsubscription;
        }
    }
}