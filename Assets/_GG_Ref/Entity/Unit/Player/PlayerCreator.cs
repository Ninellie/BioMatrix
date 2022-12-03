using System;
using UnityEngine;

public class PlayerCreator : MonoBehaviour
{
    //public Action onPlayerCreated;

    public ExpUI experienceBar;
    public AmmoBar ammoBar;
    public LifeBar lifeBar;
    public PauseMenu pauseMenu;
    public LVLUpManager levelUpManager;
    public DeathScreen deathScreen;

    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObject _playerWeapon;
    public GameObject CurrentPlayer { get; private set; }
    private void Awake()
    {
        if (_playerPrefab == null) return;
        CreatePlayer(_playerPrefab);
        CurrentPlayer.GetComponent<Player>().CreateWeapon(_playerWeapon);
        Subscription();
    }

    private void CreatePlayer(GameObject playerPrefab)
    {
        Instantiate(playerPrefab, new Vector2(0, 0), Quaternion.identity);
        CurrentPlayer = GameObject.FindGameObjectWithTag("Player");
        CurrentPlayerSeecker.SetCurrentPlayer(CurrentPlayer);
        //onPlayerCreated?.Invoke();
    }
    private void Subscription()
    {
        experienceBar.Subscription();
        ammoBar.Subscription();
        lifeBar.Subscription();
        pauseMenu.Subscription();
        levelUpManager.Subscription();
        deathScreen.Subscription();
    }
}