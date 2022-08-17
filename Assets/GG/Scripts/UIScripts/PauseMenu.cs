using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    public GameObject settingsMenuUI;
    public GameObject timer;

    private void OnEnable()
    {
        Player.OnGamePaused += OnPause;
    }

    private void OnDisable()
    {
        Player.OnGamePaused -= OnPause;
    }

    public void OnPause()
    {
        if (GameIsPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void Resume()
    {
        GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
        pauseMenuUI.SetActive(false);
        settingsMenuUI.SetActive(false);
        Time.timeScale = 1f;
        timer.GetComponent<Timer>().TimeStart();
        GameIsPaused = false;
    }

    public void Pause()
    {
        
        GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerInput>().SwitchCurrentActionMap("Menu");
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        timer.GetComponent<Timer>().TimeStop();
        GameIsPaused = true;
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
