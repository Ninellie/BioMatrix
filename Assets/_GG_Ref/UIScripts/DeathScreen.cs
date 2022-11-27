using UnityEngine;
using UnityEngine.InputSystem;

public class DeathScreen : MonoBehaviour
{
    public static bool gameIsOver = false;

    public GameObject deathScreenUI;
    public GameObject timer;

    private void OnEnable()
    {
        FindObjectOfType<Camera>()
            .GetComponent<PlayerCreator>()
            .onPlayerCreated += Subscription;
    }
    private void OnDisable()
    {
        FindObjectOfType<Camera>()
            .GetComponent<PlayerCreator>()
            .onPlayerCreated -= Subscription;

        GameObject.FindGameObjectsWithTag("Player")[0]
            .GetComponent<Player>()
            .onCharacterDeath -= EnableDeathScreen;
    }
    private void Subscription()
    {
        GameObject.FindGameObjectsWithTag("Player")[0]
            .GetComponent<Player>()
            .onCharacterDeath += EnableDeathScreen;
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
