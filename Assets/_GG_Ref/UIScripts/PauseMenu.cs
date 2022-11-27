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
    private void OnEnable()
    {
        FindObjectOfType<Camera>().GetComponent<PlayerCreator>().onPlayerCreated += Subscription;
    }
    private void OnDisable()
    {
        FindObjectOfType<Camera>().GetComponent<PlayerCreator>().onPlayerCreated -= Subscription;
        GameObject.FindGameObjectsWithTag("Player")[0]
            .GetComponent<Player>().onGamePaused -= OnPause;

    }
    private void Subscription()
    {
        GameObject.FindGameObjectsWithTag("Player")[0]
            .GetComponent<Player>().onGamePaused += OnPause;
    }
    public void OnPause()
    {
        if (pauseMenuUI.activeInHierarchy || settingsMenuUI.activeInHierarchy)
            Resume();
        else
            Pause();
    }
    private void Resume()
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
    private void Pause()
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
    public void PauseGame()
    {
        GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerInput>().SwitchCurrentActionMap("Menu");
        Time.timeScale = 0f;
        timerUI.GetComponent<Timer>().TimeStop();
        GameIsPaused = true;
    }
    private void ResumeGame()
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