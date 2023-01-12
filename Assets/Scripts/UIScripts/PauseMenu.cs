using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject settingsMenuUI;
    public GameObject timerUI;
    public GameObject lvlUpMenuUI;
    public void Subscription()
    {
        FindObjectOfType<Player>().onGamePaused += OnPause;
        FindObjectOfType<Player>().onPlayerDeath += Unsubscription;
    }
    private void Unsubscription()
    {
        FindObjectOfType<Player>().onGamePaused -= OnPause;
        FindObjectOfType<Player>().onPlayerDeath -= Unsubscription;
    }
    public void OnPause()
    {
        if (pauseMenuUI.activeInHierarchy || settingsMenuUI.activeInHierarchy)
            Resume();
        else
            Pause();
    }
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        settingsMenuUI.SetActive(false);

        if (lvlUpMenuUI.activeInHierarchy)
        {
            var buttons = lvlUpMenuUI.GetComponentsInChildren<Button>();
            foreach (var button in buttons)
            {
                button.interactable = true;
            }
        }
        else
        {
            ResumeGame();
        }
    }
    private void Pause()
    {
        PauseGame();
        if (lvlUpMenuUI.activeInHierarchy)
        {
            var buttons = lvlUpMenuUI.GetComponentsInChildren<Button>();
            foreach (var button in buttons)
            {
                button.interactable = false;
            }
        }
        pauseMenuUI.SetActive(true);
    }
    public void PauseGame()
    {
        GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerInput>().SwitchCurrentActionMap("Menu");
        Time.timeScale = 0f;
        timerUI.GetComponent<GameTimer>().TimeStop();
        GameIsPaused = true;
    }
    private void ResumeGame()
    {
        GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
        Time.timeScale = 1f;
        timerUI.GetComponent<GameTimer>().TimeStart();
        GameIsPaused = false;
    }
    public void BackToMainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}