using System;
using Assets.Scripts.EntityComponents.Resources;
using Assets.Scripts.EntityComponents.UnitComponents.Turret;
using Assets.Scripts.GameSession.UIScripts.SessionModel;
using Assets.Scripts.View;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents
{
    [Serializable]
    public class OldDisplayedResourceData
    {
        public ResourceName resourceName;
        public TargetName targetName;
        public ResourceCounter resourceCounter;
    }

    public class PlayerCreator : MonoBehaviour
    {
        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private GameObject _playerWeapon;
        [SerializeField] private GameObject _turretHub;
        [SerializeField] private OldDisplayedResourceData[] _displayedResources;
        public Player CurrentPlayer { get; private set; }

        [SerializeField] private OverUnitDataAggregator _dataAggregator;
        [SerializeField] private LevelUp _levelUp;
        private GameSessionTimer _gameSessionTimer;
        private OptionsMenu _optionsMenu;
        private ViewController _viewController;

        private void Awake()
        {
            _gameSessionTimer = FindObjectOfType<GameSessionTimer>();
            _viewController = FindObjectOfType<ViewController>();
            _optionsMenu = FindObjectOfType<OptionsMenu>();

            CreatePlayer(_playerPrefab);
            CreatePlayerFirearm();
            CreateTurretHub();

            _viewController.AwakeController(CurrentPlayer.GetComponent<PlayerInput>());
            Subscription();
            SetupIndicators();
        }

        private void CreatePlayerFirearm()
        { 
            var firearm = CurrentPlayer.CreateWeapon(_playerWeapon);
            _dataAggregator.ReadInfoFromTarget(firearm, TargetName.Firearm);
        }

        private void CreatePlayer(GameObject playerPrefab)
        {
            var player = Instantiate(playerPrefab, new Vector2(0, 0), Quaternion.identity);
            CurrentPlayer = player.GetComponent<Player>();
            _dataAggregator = player.GetComponent<OverUnitDataAggregator>();
            _levelUp.EffectsAggregator = _dataAggregator;
            _dataAggregator.ReadInfoFromTarget(player, TargetName.Player);
            //
            var shield = player.GetComponentInChildren<Shield>().gameObject;
            _dataAggregator.ReadInfoFromTarget(shield, TargetName.Shield);
        }

        private void CreateTurretHub()
        {
            var turretHubPrefab = Instantiate(_turretHub);
            var turretHub = turretHubPrefab.GetComponent<TurretHub>();
            turretHub.SetHolder(CurrentPlayer);
            CurrentPlayer.TurretHub = turretHub;
            turretHubPrefab.transform.SetParent(CurrentPlayer.transform);
            var firearm = turretHub.CreateTurretWeapon();
            _dataAggregator.ReadInfoFromTarget(firearm, TargetName.TurretHubWeapon);
        }

        private void SetupIndicators()
        {
            foreach (var displayedResourceData in _displayedResources)
            {
                var key = displayedResourceData.targetName;
                var resourceName = displayedResourceData.resourceName;
                var resource = _dataAggregator.Resources[key].GetResourceByName(resourceName);
                if (resource == null) continue;
                displayedResourceData.resourceCounter.SetResource(resource);
                displayedResourceData.resourceCounter.SetLabel(resourceName.ToString());
            }
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