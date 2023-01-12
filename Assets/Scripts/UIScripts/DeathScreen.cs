using UnityEngine;
using UnityEngine.InputSystem;

public class DeathScreen : MonoBehaviour
{
    public bool gameIsOver = false;
    public GameObject deathScreen;
    //public GameObject timer;
    public void Subscription()
    {
        FindObjectOfType<Player>().onPlayerDeath += EnableDeathScreen;
        FindObjectOfType<Player>().onPlayerDeath += Unsubscription;
    }
    private void Unsubscription()
    {
        FindObjectOfType<Player>().onPlayerDeath -= EnableDeathScreen;
        FindObjectOfType<Player>().onPlayerDeath -= Unsubscription;
    }
    public void EnableDeathScreen()
    {
        GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerInput>().SwitchCurrentActionMap("Death");
        deathScreen.SetActive(true);
        Time.timeScale = 0f;
        FindObjectOfType<GameTimer>().TimeStop();
    }
}
