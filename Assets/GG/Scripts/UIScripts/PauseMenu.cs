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

    //�������� ������ OnPause() �� ������� OnGamePaused � ������ OnEnable()
    private void OnEnable()
    {
        Player.onGamePaused += OnPause;
    }
    //������� ������ OnPause() �� ������� OnGamePaused � ������ OnDisable()
    private void OnDisable()
    {
        Player.onGamePaused -= OnPause;
    }

    /// <summary>
    /// ���� ���� ����� ��� ���� �������� �������, �������� ������� Resume(), ����� �������� ������� Pause()
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
    /// �������� pauseMenuUI � settingsMenuUI. �����, ���� lvlUpMenuUI �������, ������ ��� ������ ��������������, ����� �������� ����� ResumeGame()
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
    /// �������� ����� PauseGame() � ��������� pauseMenuUI. �����, ���� lvlUpMenuUI �������, ������ ��� ������ ����������������.
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
    /// ������ ActionMap �� "Menu". ���������� timeScale = 0, ����������� ����. ������������� ������� ������. ������������� GameIsPaused = true.
    /// </summary>
    public void PauseGame()
    {
        GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerInput>().SwitchCurrentActionMap("Menu");
        Time.timeScale = 0f;
        timerUI.GetComponent<Timer>().TimeStop();
        GameIsPaused = true;
    }
    /// <summary>
    /// ������ ActionMap �� "Player". ���������� timeScale = 1, ����������� ������� ����. ������������ ������� ������. ������������� GameIsPaused = false.
    /// </summary>
    public void ResumeGame()
    {
        GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
        Time.timeScale = 1f;
        timerUI.GetComponent<Timer>().TimeStart();
        GameIsPaused = false;
    }

    /// <summary>
    /// ��������� ����� � �������� 1 (������� ���� ����)
    /// </summary>
    public void BackToMainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}