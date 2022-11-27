using UnityEngine;

public class HPUI : MonoBehaviour
{
    public TMPro.TMP_Text currentLifePointsBar;
    //public TMPro.TMP_Text ammoUI;

    private void Start()
    {
        UpdateLifePointsBar();
    }
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
            .GetComponent<Player>().onCurrentLifePointsChanged += UpdateLifePointsBar;
        Debug.Log("Life points bar was Subscribe on UpdateLifePointsBar of Player");
        GameObject.FindGameObjectsWithTag("Player")[0]
            .GetComponent<Player>().onPlayerDeath += Unsubscription;
    }
    private void Unsubscription()
    {
        GameObject.FindGameObjectsWithTag("Player")[0]
            .GetComponent<Player>().onCurrentLifePointsChanged -= UpdateLifePointsBar;
        Debug.Log("Life points bar was Unsubscribe on UpdateLifePointsBar of Player");
        GameObject.FindGameObjectsWithTag("Player")[0]
            .GetComponent<Player>().onPlayerDeath -= Unsubscription;
    }
    private void UpdateLifePointsBar()
    {
        Debug.Log("UpdateLifePointsBar UPDATE UPDATE");

        var currentLifePointsText = "HP: " + GameObject.FindGameObjectsWithTag("Player")[0]
            .GetComponent<Player>().CurrentLifePoints.ToString();
        currentLifePointsBar.text = currentLifePointsText;
    }
}
