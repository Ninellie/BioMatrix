using Assets.Scripts.Entity.Unit.Turret;
using Assets.Scripts.GameSession.UIScripts.SessionModel;
using Assets.Scripts.GameSession.UIScripts.View;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Entity.Unit.Player
{
    public class PlayerCreator : MonoBehaviour
    {
        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private GameObject _playerWeapon;
        [SerializeField] private GameObject _turretHub;

        private ExpUI _experienceBar;
        private AmmoBar _ammoBar;
        private LifeBar _lifeBar;
        private GameSessionTimer _gameSessionTimer;
        private OptionsMenu _optionsMenu;
        private ViewController _viewController;

        public Player CurrentPlayer { get; private set; }
        private void Awake()
        {
            _gameSessionTimer = FindObjectOfType<GameSessionTimer>();
            _viewController = FindObjectOfType<ViewController>();
            _experienceBar = FindObjectOfType<ExpUI>();
            _lifeBar = FindObjectOfType<LifeBar>();
            _ammoBar = FindObjectOfType<AmmoBar>();
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
        }

        private void CreatePlayer(GameObject playerPrefab)
        {
            var player = Instantiate(playerPrefab, new Vector2(0, 0), Quaternion.identity);
            CurrentPlayer = player.GetComponent<Player>();
        }

        private void Subscription()
        {
            _experienceBar.Subscription();
            _ammoBar.Subscription();
            _lifeBar.Subscription();

            CurrentPlayer.GamePausedEvent += _viewController.Menu;
            CurrentPlayer.LevelUpEvent += _viewController.LevelUpEvent;
            CurrentPlayer.OnDeath += _viewController.Lose;
            _gameSessionTimer.onGameWinning += _viewController.Win;
            CurrentPlayer.OnDeath += Unsubscription;
            _gameSessionTimer.onGameWinning += Unsubscription;
            _optionsMenu.onBackToMainMenu += Unsubscription;
        }

        public void Unsubscription()
        {
            CurrentPlayer.GamePausedEvent -= _viewController.Menu;
            CurrentPlayer.LevelUpEvent -= _viewController.LevelUpEvent;
            CurrentPlayer.OnDeath -= _viewController.Lose;
            _gameSessionTimer.onGameWinning -= _viewController.Win;
            CurrentPlayer.OnDeath -= Unsubscription;
            _gameSessionTimer.onGameWinning -= Unsubscription;
            _optionsMenu.onBackToMainMenu -= Unsubscription;
        }
    }
}