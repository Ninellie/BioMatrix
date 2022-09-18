using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject settingsMenuUI;
    public GameObject timerUI;
    public GameObject lvlUpMenuUI;

    //Подписка метода OnPause() на событие OnGamePaused в методе OnEnable()
    private void OnEnable()
    {
        Player.OnGamePaused += OnPause;
    }
    //Отписка метода OnPause() от события OnGamePaused в методе OnDisable()
    private void OnDisable()
    {
        Player.OnGamePaused -= OnPause;
    }

    /// <summary>
    /// Если меню паузы или меню настроек открыто, вызывает функцию Resume(), иначе вызывает функцию Pause()
    /// </summary>
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

    /// <summary>
    /// Скрывает pauseMenuUI и settingsMenuUI. Также, если lvlUpMenuUI открыто, делает его кнопки интерактивными, иначе вызывает метод ResumeGame()
    /// </summary>
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        settingsMenuUI.SetActive(false);

        if (lvlUpMenuUI.activeInHierarchy)
        {
            var button = lvlUpMenuUI.GetComponentsInChildren<Button>();
            for (int i = 0; i < button.Length; i++)
            {
                button[i].interactable = true;
            }
        }
        else
        {
            ResumeGame();
        }
    }

    /// <summary>
    /// Вызывает метод PauseGame() и открывает pauseMenuUI. Также, если lvlUpMenuUI открыто, делает его кнопки неинтерактивными.
    /// </summary>
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
        }
        pauseMenuUI.SetActive(true);
    }

    /// <summary>
    /// Меняет ActionMap на "Menu". Выставляет timeScale = 0, замораживая игру. Останавливает игровой таймер. Устанавливает GameIsPaused = true.
    /// </summary>
    public void PauseGame()
    {
        GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerInput>().SwitchCurrentActionMap("Menu");
        Time.timeScale = 0f;
        timerUI.GetComponent<Timer>().TimeStop();
        GameIsPaused = true;
    }
    /// <summary>
    /// Меняет ActionMap на "Player". Выставляет timeScale = 1, возобновляя течение игры. Возобновляет игровой таймер. Устанавливает GameIsPaused = false.
    /// </summary>
    public void ResumeGame()
    {
        GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
        Time.timeScale = 1f;
        timerUI.GetComponent<Timer>().TimeStart();
        GameIsPaused = false;
    }

    /// <summary>
    /// Загружает сцену с индексом 1 (Главное меню игры)
    /// </summary>
    public void BackToMainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}