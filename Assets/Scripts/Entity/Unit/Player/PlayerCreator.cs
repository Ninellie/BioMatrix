using Assets.Scripts.GameSession.View;
using UnityEngine;

public class PlayerCreator : MonoBehaviour
{
    //public Action onPlayerCreated;

    private ExpUI _experienceBar;
    private AmmoBar _ammoBar;
    private LifeBar _lifeBar;
    private GameSessionTimer _gameSessionTimer;
    private OptionsMenu _optionsMenu;
    private ViewController _UIController;

    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObject _playerWeapon;
    public Player CurrentPlayer { get; private set; }
    private void Awake()
    {
        _gameSessionTimer = FindObjectOfType<GameSessionTimer>();
        _UIController = FindObjectOfType<ViewController>();
        _experienceBar = FindObjectOfType<ExpUI>();
        _lifeBar = FindObjectOfType<LifeBar>();
        _ammoBar = FindObjectOfType<AmmoBar>();
        _optionsMenu = FindObjectOfType<OptionsMenu>();
        if (_playerPrefab == null) return;
        CreatePlayer(_playerPrefab);
        CurrentPlayer.GetComponent<Player>().CreateWeapon(_playerWeapon);
        _UIController.AwakeController();
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

        CurrentPlayer.GamePausedEvent += _UIController.Menu;
        CurrentPlayer.LevelUpEvent += _UIController.LevelUpEvent;
        CurrentPlayer.OnDeath += _UIController.Lose;
        _gameSessionTimer.onGameWinning += _UIController.Win;
        CurrentPlayer.OnDeath += Unsubscription;
        _gameSessionTimer.onGameWinning += Unsubscription;
        _optionsMenu.onBackToMainMenu += Unsubscription;
    }

    public void Unsubscription()
    {
        CurrentPlayer.GamePausedEvent -= _UIController.Menu;
        CurrentPlayer.LevelUpEvent -= _UIController.LevelUpEvent;
        CurrentPlayer.OnDeath -= _UIController.Lose;
        _gameSessionTimer.onGameWinning -= _UIController.Win;
        CurrentPlayer.OnDeath -= Unsubscription;
        _gameSessionTimer.onGameWinning -= Unsubscription;
        _optionsMenu.onBackToMainMenu -= Unsubscription;
    }
}