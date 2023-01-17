using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class WinScreen : MonoBehaviour
{
    public readonly float winTime = 600;
    public GameObject winScreen;
    private GameTimer _gameTimer;
    public Action onGameWinning;
    private void Update()
    {
        if (!(_gameTimer.GetTotalSeconds() > winTime)) return;
        onGameWinning?.Invoke();
        Unsubscription();
    }
    private void Awake()
    {
        _gameTimer = FindObjectOfType<GameTimer>();
    }
    public void Subscription()
    {
        FindObjectOfType<Player>().onDeath += Unsubscription;
        onGameWinning += EnableWinScreen;
    }
    private void Unsubscription()
    {
        FindObjectOfType<Player>().onDeath -= Unsubscription;
        onGameWinning -= EnableWinScreen;
    }
    private void EnableWinScreen()
    {
        Debug.Log("Win screen enabled");
        winScreen.SetActive(true);
        GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerInput>().SwitchCurrentActionMap("Death");
        FindObjectOfType<GameTimer>().Stop();
        Time.timeScale = 0f;
        onGameWinning -= EnableWinScreen;
    }
}