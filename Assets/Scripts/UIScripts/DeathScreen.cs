using UnityEngine;
using UnityEngine.InputSystem;

public class DeathScreen : MonoBehaviour
{
    public GameObject deathScreen;
    public void Subscription()
    {
        FindObjectOfType<Player>().onDeath += EnableDeathScreen;
        FindObjectOfType<Player>().onDeath += Unsubscription;
    }
    private void Unsubscription()
    {
        FindObjectOfType<Player>().onDeath -= EnableDeathScreen;
        FindObjectOfType<Player>().onDeath -= Unsubscription;
    }
    public void EnableDeathScreen()
    {
        GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerInput>().SwitchCurrentActionMap("Death");
        deathScreen.SetActive(true);
        Time.timeScale = 0f;
        FindObjectOfType<GameTimer>().Stop();
    }
}
