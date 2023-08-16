using System;
using Assets.Scripts.EntityComponents.Resources;
using Assets.Scripts.EntityComponents.UnitComponents.Turret;
using Assets.Scripts.GameSession.Events;
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
        [SerializeField] private LevelUp _levelUp;
        [SerializeField] private GameTimeScheduler _gameTimeScheduler;

        private Player _player;
        private OverUnitDataAggregator _playerDataAggregator;
        private GameSessionTimer _gameSessionTimer;
        private OptionsMenu _optionsMenu;
        private ViewController _viewController;

        private ResourceListenerData levelUpListener;
        private ResourceListenerData loseListener;
        private ResourceListenerData unsubscriptionListener;

        private void Awake()
        {
            _gameTimeScheduler = FindObjectOfType<GameTimeScheduler>();
            _gameSessionTimer = FindObjectOfType<GameSessionTimer>();
            _viewController = FindObjectOfType<ViewController>();
            _optionsMenu = FindObjectOfType<OptionsMenu>();

            levelUpListener = new ResourceListenerData(_viewController.LevelUpEvent,
                TargetName.Player, ResourceName.Level, ResourceEventType.Increment);
            loseListener = new ResourceListenerData(_viewController.Lose,
                TargetName.Player, ResourceName.Health, ResourceEventType.Empty);
            unsubscriptionListener = new ResourceListenerData(Unsubscription,
                TargetName.Player, ResourceName.Health, ResourceEventType.Empty);

            CreatePlayer(_playerPrefab);
            CreatePlayerFirearm();
            CreateTurretHub();

            _viewController.AwakeController(_player.GetComponent<PlayerInput>());
            Subscription();
            SetupIndicators();
        }

        [ContextMenu("Find GameTimeScheduler")]
        public void FindGameTimeScheduler()
        {
            _gameTimeScheduler = FindObjectOfType<GameTimeScheduler>();
        }

        private void CreatePlayer(GameObject playerPrefab)
        {
            var player = Instantiate(playerPrefab, new Vector2(0, 0), Quaternion.identity);
            player.GetComponent<EffectsList>().GameTimeScheduler = _gameTimeScheduler;
            _player = player.GetComponent<Player>();
            _playerDataAggregator = player.GetComponent<OverUnitDataAggregator>();
            _levelUp.EffectsAggregator = _playerDataAggregator;
            _playerDataAggregator.ReadInfoFromTarget(player, TargetName.Player);
            //
            _player.Shield.GetComponent<EffectsList>().GameTimeScheduler = _gameTimeScheduler;
            _playerDataAggregator.ReadInfoFromTarget(_player.Shield.gameObject, TargetName.Shield);
        }

        private void CreatePlayerFirearm()
        {
            var firearm = _player.CreateWeapon(_playerWeapon);
            firearm.GetComponent<EffectsList>().GameTimeScheduler = _gameTimeScheduler;
            _playerDataAggregator.ReadInfoFromTarget(firearm, TargetName.Firearm);
        }

        private void CreateTurretHub()
        {
            var turretHubPrefab = Instantiate(_turretHub);
            var turretHub = turretHubPrefab.GetComponent<TurretHub>();
            turretHub.SetHolder(_player);
            _player.TurretHub = turretHub;
            turretHubPrefab.transform.SetParent(_player.transform);
            var firearm = turretHub.CreateTurretWeapon();
            firearm.GetComponent<EffectsList>().GameTimeScheduler = _gameTimeScheduler;
            _playerDataAggregator.ReadInfoFromTarget(firearm, TargetName.TurretHubWeapon);
        }

        private void SetupIndicators()
        {
            foreach (var displayedResourceData in _displayedResources)
            {
                var key = displayedResourceData.targetName;
                var resourceName = displayedResourceData.resourceName;
                var resource = _playerDataAggregator.Resources[key].GetResourceByName(resourceName);
                if (resource == null) continue;
                displayedResourceData.resourceCounter.SetResource(resource);
                displayedResourceData.resourceCounter.SetLabel(resourceName.ToString());
            }
        }

        private void Subscription()
        {
            _gameSessionTimer.onGameWinning += _viewController.Win;
            _gameSessionTimer.onGameWinning += Unsubscription;
            _optionsMenu.onBackToMainMenu += Unsubscription;
            _player.GamePausedEvent += _viewController.Menu;

            _playerDataAggregator.AddListener(levelUpListener);
            _playerDataAggregator.AddListener(loseListener);
            _playerDataAggregator.AddListener(unsubscriptionListener);
        }

        public void Unsubscription()
        {
            _gameSessionTimer.onGameWinning -= _viewController.Win;
            _gameSessionTimer.onGameWinning -= Unsubscription;
            _optionsMenu.onBackToMainMenu -= Unsubscription;
            _player.GamePausedEvent -= _viewController.Menu;

            _playerDataAggregator.RemoveListener(levelUpListener);
            _playerDataAggregator.RemoveListener(loseListener);
            _playerDataAggregator.RemoveListener(unsubscriptionListener);
        }
    }
}