using UnityEngine;
using UnityEngine.InputSystem;

public class DeathScreen : MonoBehaviour
{
    public static bool gameIsOver = false;
    public GameObject deathScreen;
    public GameObject timer;

    private void OnEnable()
    {
        FindObjectOfType<Camera>()
            .GetComponent<PlayerCreator>().onPlayerCreated += Subscription;
    }
    private void OnDisable()
    {
        FindObjectOfType<Camera>()
            .GetComponent<PlayerCreator>().onPlayerCreated -= Subscription;
    }
    private void Subscription()
    {
        GameObject.FindGameObjectsWithTag("Player")[0]
            .GetComponent<Player>().onPlayerDeath += EnableDeathScreen;
        GameObject.FindGameObjectsWithTag("Player")[0]
            .GetComponent<Player>().onPlayerDeath += Unsubscription;

    }
    private void Unsubscription()
    {
        GameObject.FindGameObjectsWithTag("Player")[0]
            .GetComponent<Player>().onPlayerDeath -= EnableDeathScreen;
        GameObject.FindGameObjectsWithTag("Player")[0]
            .GetComponent<Player>().onPlayerDeath -= Unsubscription;
    }
    public void EnableDeathScreen()
    {
        GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerInput>().SwitchCurrentActionMap("Death");
        deathScreen.SetActive(true);
        Time.timeScale = 0f;
        timer.GetComponent<Timer>().TimeStop();
        gameIsOver = true;
    }
}
