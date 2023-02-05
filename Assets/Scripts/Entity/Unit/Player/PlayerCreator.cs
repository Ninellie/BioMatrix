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
    private ViewController _UIController;

    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObject _playerWeapon;
    public Player CurrentPlayer { get; private set; }
    private void Awake()
    {
        _gameTimer = FindObjectOfType<GameTimer>();
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
        Instantiate(playerPrefab, new Vector2(0, 0), Quaternion.identity);
        CurrentPlayer = FindObjectOfType<Player>();
    }
    private void Subscription()
    {
        _experienceBar.Subscription();
        _ammoBar.Subscription();
        _lifeBar.Subscription();

        CurrentPlayer.onGamePaused += _UIController.Menu;
        CurrentPlayer.onLevelUp += _UIController.LevelUp;
        CurrentPlayer.onDeath += _UIController.Lose;
        _gameTimer.onGameWinning += _UIController.Win;
        CurrentPlayer.onDeath += Unsubscription;
        _gameTimer.onGameWinning += Unsubscription;
        _optionsMenu.onBackToMainMenu += Unsubscription;
    }

    public void Unsubscription()
    {
        CurrentPlayer.onGamePaused -= _UIController.Menu;
        CurrentPlayer.onLevelUp -= _UIController.LevelUp;
        CurrentPlayer.onDeath -= _UIController.Lose;
        _gameTimer.onGameWinning -= _UIController.Win;
        CurrentPlayer.onDeath -= Unsubscription;
        _gameTimer.onGameWinning -= Unsubscription;
        _optionsMenu.onBackToMainMenu -= Unsubscription;
    }
}