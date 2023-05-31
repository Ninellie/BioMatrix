using Assets.Scripts.GameSession.View;
using UnityEngine;

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

        if (_playerPrefab == null) return;

        CreatePlayer(_playerPrefab);
        CurrentPlayer.GetComponent<Player>().CreateWeapon(_playerWeapon);
        var turretHub = Instantiate(_turretHub);
        CurrentPlayer.TurretHub = turretHub.GetComponent<TurretHub>();
        turretHub.transform.SetParent(CurrentPlayer.transform);

        _viewController.AwakeController();
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