using System;
using Assets.Scripts.EntityComponents.Resources;
using Assets.Scripts.EntityComponents.UnitComponents.Turret;
using Assets.Scripts.GameSession.Events;
using Assets.Scripts.GameSession.UIScripts.SessionModel;
using Assets.Scripts.GameSession.Upgrades.Deck;
using Assets.Scripts.View;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents
{
    [Serializable]
    public class OldDisplayedResourceData
    {
        public ResourceName resourceName;
        public TargetName targetName;
        public Counter counter;
    }

    public class PlayerCreator : MonoBehaviour
    {
        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private GameObject _turretHub;
        [SerializeField] private OldDisplayedResourceData[] _displayedResources;
        //[SerializeField] private LevelUp _levelUp;
        [SerializeField] private LevelUpController _levelUpController;
        [SerializeField] private GameTimeScheduler _gameTimeScheduler;

        private GameSessionTimer _gameSessionTimer;
        private OptionsMenu _optionsMenu;
        private GameSessionController _gameSessionController;
        private EffectsRepository _effectsRepository;
        private IDeckRepository _deckRepository;

        //private Player _player;
        private IHand _hand;
        private OverUnitDataAggregator _playerDataAggregator;

        //private ResourceListenerData levelUpListener;
        //private ResourceListenerData loseListener;
        //private ResourceListenerData unsubscriptionListener;

        private void Awake()
        {
            _gameTimeScheduler = FindObjectOfType<GameTimeScheduler>();
            _gameSessionTimer = FindObjectOfType<GameSessionTimer>();
            _gameSessionController = FindObjectOfType<GameSessionController>();
            _optionsMenu = FindObjectOfType<OptionsMenu>();

            _effectsRepository = FindObjectOfType<EffectsRepository>();
            _deckRepository = FindObjectOfType<PatternDeckRepository>();

            //levelUpListener = new ResourceListenerData(_gameSessionController.LevelUpEvent, TargetName.Player, ResourceName.Level, ResourceEventType.Increment);
            //loseListener = new ResourceListenerData(_gameSessionController.Lose, TargetName.Player, ResourceName.Health, ResourceEventType.Empty);
            //unsubscriptionListener = new ResourceListenerData(Unsubscription, TargetName.Player, ResourceName.Health, ResourceEventType.Empty);

            CreatePlayer(_playerPrefab);
            //CreatePlayerFirearm();
            CreateTurretHub();

            //_gameSessionController.AwakeController(_player.GetComponent<PlayerInput>());
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
            //_player = player.GetComponent<Player>();
            _playerDataAggregator = player.GetComponent<OverUnitDataAggregator>();
            _hand = player.GetComponent<IHand>();
            _hand.SetDeckRepository(_deckRepository);
            _hand.SetEffectRepository(_effectsRepository);

            _levelUpController.SetHand(_hand);
            _playerDataAggregator.ReadInfoFromTarget(player, TargetName.Player);
            //
            //_player.Shield.GetComponent<EffectsList>().GameTimeScheduler = _gameTimeScheduler;
            //_playerDataAggregator.ReadInfoFromTarget(_player.Shield.gameObject, TargetName.Shield);
        }

        private void CreateTurretHub()
        {
            var turretHubPrefab = Instantiate(_turretHub);
            var turretHub = turretHubPrefab.GetComponent<TurretHub>();
            //turretHub.SetSource(_player);
            //_player.TurretHub = turretHub;
            //turretHubPrefab.transform.SetParent(_player.transform);
            turretHubPrefab.GetComponent<EffectsList>().GameTimeScheduler = _gameTimeScheduler;
            _playerDataAggregator.ReadInfoFromTarget(turretHubPrefab, TargetName.TurretHub);

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
                var resource = _playerDataAggregator.Resources[key].GetResource(resourceName);
                if (resource == null) continue;
                //displayedResourceData.reserveCounter.SetResource(resource);
                //displayedResourceData.reserveCounter.SetLabel(resourceName.ToString());
            }
        }

        private void Subscription()
        {
            //_gameSessionTimer.onGameWinning += _gameSessionController.Win;
            //_gameSessionTimer.onGameWinning += Unsubscription;
            _optionsMenu.onBackToMainMenu += Unsubscription;
            //_player.GamePausedEvent += _gameSessionController.Menu;

            //_playerDataAggregator.AddListener(levelUpListener);
            //_playerDataAggregator.AddListener(loseListener);
            //_playerDataAggregator.AddListener(unsubscriptionListener);
        }

        private void Unsubscription()
        {
            //_gameSessionTimer.onGameWinning -= _gameSessionController.Win;
            //_gameSessionTimer.onGameWinning -= Unsubscription;
            _optionsMenu.onBackToMainMenu -= Unsubscription;
            //_player.GamePausedEvent -= _gameSessionController.Menu;

            //_playerDataAggregator.RemoveListener(levelUpListener);
            //_playerDataAggregator.RemoveListener(loseListener);
            //_playerDataAggregator.RemoveListener(unsubscriptionListener);
        }
    }
}