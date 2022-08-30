using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    //Is pause menu active
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    public GameObject settingsMenuUI;
    public GameObject timerUI;
    public GameObject lvlUpMenuUI;

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
        if (pauseMenuUI.activeInHierarchy || settingsMenuUI.activeInHierarchy)
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
        if (lvlUpMenuUI.activeInHierarchy)
        {
            pauseMenuUI.SetActive(false);
            settingsMenuUI.SetActive(false);
            var button = lvlUpMenuUI.GetComponentsInChildren<Button>();
            for (int i = 0; i < button.Length; i++)
            {
                button[i].interactable = true;
            }
        }
        else
        {
            pauseMenuUI.SetActive(false);
            settingsMenuUI.SetActive(false);
            ResumeGame();
        }
    }

    public void Pause()
    {
        PauseGame();
        if (lvlUpMenuUI.activeInHierarchy)
        {
            var button = lvlUpMenuUI.GetComponentsInChildren<Button>();
            for(int i = 0;  i < button.Length; i++)
            {
                button[i].interactable = false;
            }
            pauseMenuUI.SetActive(true);
        }
        else
        {
            pauseMenuUI.SetActive(true);
        }
    }

    public void PauseGame()
    {
        GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerInput>().SwitchCurrentActionMap("Menu");
        Time.timeScale = 0f;
        timerUI.GetComponent<Timer>().TimeStop();
        GameIsPaused = true;
    }

    public void ResumeGame()
    {
        GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
        Time.timeScale = 1f;
        timerUI.GetComponent<Timer>().TimeStart();
        GameIsPaused = false;
    }


    public void BackToMainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}