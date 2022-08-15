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

    private void OnEnable()
    {
        Player.OnCharacterDeath += EnableDeathScreen;
    }

    private void OnDisable()
    {
        Player.OnCharacterDeath -= EnableDeathScreen;
    }

    public void EnableDeathScreen()
    {
        GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerInput>().SwitchCurrentActionMap("Death");
        deathScreenUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsOver = true;
    }
}
