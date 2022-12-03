using UnityEngine;

public class AmmoBar : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text _ammo;

    private void Start()
    {
        UpdateBar();
    }
    //private void OnEnable()
    //{
    //    Debug.Log("AmmoBar OnEnable");
    //    FindObjectOfType<Camera>()
    //        .GetComponent<PlayerCreator>().onPlayerCreated += Subscription;
    //}
    //private void OnDisable()
    //{
    //    Debug.Log("AmmoBar OnDisable");
    //    FindObjectOfType<Camera>()
    //        .GetComponent<PlayerCreator>().onPlayerCreated -= Subscription;
    //}
    public void Subscription()
    {
        Debug.Log("Ammo bar started subscribing on current ammo amount tof Player's Firearm");

        GameObject.FindObjectOfType<Firearm>().Magazine.onCurrentAmountChanged += UpdateBar;
        //GameObject.FindGameObjectsWithTag("Player")[0]
        //    .GetComponentInChildren<Firearm>().Magazine.onCurrentAmountChanged += UpdateBar;
        CurrentPlayerSeecker.CurrentPlayer
            .GetComponent<Player>().onPlayerDeath += Unsubscription;
    }
    private void Unsubscription()
    {
        Debug.Log("Ammo bar started unsubscribing from current ammo amount tof Player's Firearm");

        //GameObject.FindGameObjectsWithTag("Player")[0]
        //.GetComponentInChildren<Firearm>().Magazine.onCurrentAmountChanged -= UpdateBar;
        GameObject.FindObjectOfType<Firearm>().Magazine.onCurrentAmountChanged -= UpdateBar;
        GameObject.FindGameObjectsWithTag("Player")[0]
            .GetComponent<Player>().onPlayerDeath -= Unsubscription;
    }
    private void UpdateBar()
    {
        var ammoAmount = GameObject.FindGameObjectsWithTag("Player")[0]
            .GetComponentInChildren<Firearm>().Magazine.CurrentAmount;
        var ammoText = $"Ammo: {ammoAmount}";
        _ammo.text = ammoText;
        Debug.Log("Ammo bar has been updated");
    }
}