using System;
using Assets.Scripts.Map;
using UnityEngine;

public class PlayerCreator : MonoBehaviour
{
    //public Action onPlayerCreated;

    private ExpUI _experienceBar;
    private AmmoBar _ammoBar;
    private LifeBar _lifeBar;
    private GameTimer _gameTimer;
    private OptionsMenu _optionsMenu;
    private ScreenUIController _UIController;
    //public PauseMenu pauseMenu;
    //public LevelUp levelUp;
    //public DeathScreen deathScreen;
    //public WinScreen winScreen;

    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObject _playerWeapon;
    public Player CurrentPlayer { get; private set; }
    private void Awake()
    {
        _gameTimer = FindObjectOfType<GameTimer>();
        _UIController = FindObjectOfType<ScreenUIController>();
        _experienceBar = FindObjectOfType<ExpUI>();
        _lifeBar = FindObjectOfType<LifeBar>();
        _ammoBar = FindObjectOfType<AmmoBar>();
        _optionsMenu = FindObjectOfType<OptionsMenu>();
        if (_playerPrefab == null) return;
        //winScreen = FindObjectOfType<WinScreen>(true);
        CreatePlayer(_playerPrefab);
        CurrentPlayer.GetComponent<Player>().CreateWeapon(_playerWeapon);
        _UIController.AwakeController();
        Subscription();
    }
    private void CreatePlayer(GameObject playerPrefab)
    {
        Instantiate(playerPrefab, new Vector2(0, 0), Quaternion.identity);
        CurrentPlayer = FindObjectOfType<Player>();
    }
    private void Subscription()
    {
        _experienceBar.Subscription();
        _ammoBar.Subscription();
        _lifeBar.Subscription();
        //pauseMenu.Subscription();
        //levelUp.Subscription();
        //deathScreen.Subscription();
        //winScreen.Subscription();

        CurrentPlayer.onGamePaused += _UIController.map.Menu;
        CurrentPlayer.onLevelUp += _UIController.map.LevelUp;
        CurrentPlayer.onDeath += _UIController.map.Lose;
        _gameTimer.onGameWinning += _UIController.map.Win;
        CurrentPlayer.onDeath += Unsubscription;
        _gameTimer.onGameWinning += Unsubscription;
        _optionsMenu.onBackToMainMenu += Unsubscription;
    }

    public void Unsubscription()
    {
        CurrentPlayer.onGamePaused -= _UIController.map.Menu;
        CurrentPlayer.onLevelUp -= _UIController.map.LevelUp;
        CurrentPlayer.onDeath -= _UIController.map.Lose;
        _gameTimer.onGameWinning -= _UIController.map.Win;
        CurrentPlayer.onDeath -= Unsubscription;
        _gameTimer.onGameWinning -= Unsubscription;
        _optionsMenu.onBackToMainMenu -= Unsubscription;
    }
}