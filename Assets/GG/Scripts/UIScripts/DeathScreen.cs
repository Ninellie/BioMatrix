using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System;

public class DeathScreen : MonoBehaviour
{
    public static bool gameIsOver = false;

    public GameObject deathScreenUI;
    public GameObject timer;

    private void OnEnable()
    {
        Player.onCharacterDeath += EnableDeathScreen;
    }

    private void OnDisable()
    {
        Player.onCharacterDeath -= EnableDeathScreen;
    }

    public void EnableDeathScreen()
    {
        GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerInput>().SwitchCurrentActionMap("Death");
        deathScreenUI.SetActive(true);
        Time.timeScale = 0f;
        timer.GetComponent<Timer>().TimeStop();
        gameIsOver = true;
    }
}
