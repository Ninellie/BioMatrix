using System;
using UnityEngine;

public class PlayerCreator : MonoBehaviour
{
    public Action onPlayerCreated;
    [SerializeField] private GameObject _playerPrefab;
    public Player CurrentPlayer { get; private set; }
    private void Awake()
    {
        if (_playerPrefab == null) return;
        CreatePlayer(_playerPrefab);
    }

    public void CreatePlayer(GameObject playerPrefab)
    {
        Instantiate(playerPrefab, new Vector2(0, 0), Quaternion.identity);
        CurrentPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        onPlayerCreated?.Invoke();
    }
}