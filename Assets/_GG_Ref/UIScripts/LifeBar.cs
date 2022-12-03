using UnityEngine;

public class LifeBar : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text _life;

    private void Start()
    {
        UpdateBar();
    }
    //private void OnEnable()
    //{
    //    Debug.Log("LifeBar OnEnable");
    //    FindObjectOfType<Camera>().GetComponent<PlayerCreator>().onPlayerCreated += Subscription;
    //}
    //private void OnDisable()
    //{
    //    Debug.Log("LifeBar OnDisable");
    //    FindObjectOfType<Camera>()
    //        .GetComponent<PlayerCreator>().onPlayerCreated -= Subscription;
    //}
    public void Subscription()
    {
        Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAAAAAALife bar started subscribing on current life points of Player");

        //GameObject.FindGameObjectsWithTag("Player")[0]
        //    .GetComponent<Player>().onCurrentLifePointsChanged += UpdateBar;
        //GameObject.FindGameObjectsWithTag("Player")[0]
        //    .GetComponent<Player>().onPlayerDeath += Unsubscription;

        CurrentPlayerSeecker.CurrentPlayer.GetComponent<Player>().onCurrentLifePointsChanged += UpdateBar;
        CurrentPlayerSeecker.CurrentPlayer.GetComponent<Player>().onPlayerDeath += Unsubscription;
    }
    private void Unsubscription()
    {
        Debug.Log("Life bar started unsubscribing from current life points of Player");

        GameObject.FindGameObjectsWithTag("Player")[0]
            .GetComponent<Player>().onCurrentLifePointsChanged -= UpdateBar;
        GameObject.FindGameObjectsWithTag("Player")[0]
            .GetComponent<Player>().onPlayerDeath -= Unsubscription;
    }
    private void UpdateBar()
    {
        var lifeText =
            $"HP: {GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Player>().CurrentLifePoints}";
        _life.text = lifeText;
        Debug.Log("Life bar has been updated");
    }
}