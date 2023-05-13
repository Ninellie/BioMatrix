using UnityEngine;

public class AmmoBar : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text _ammo;

    private void Start()
    {
        UpdateBar();
    }
    public void Subscription()
    {
        Debug.Log("Ammo bar started subscribing on current ammo amount to Player's Firearm");
        FindObjectOfType<Player>().CurrentFirearm.magazine.onValueChanged += UpdateBar;
        FindObjectOfType<Player>().onDeath += Unsubscription;
    }
    private void Unsubscription()
    {
        Debug.Log("Ammo bar started unsubscribing from current ammo amount tof Player's Firearm");
        FindObjectOfType<Player>().CurrentFirearm.magazine.onValueChanged += UpdateBar;
        FindObjectOfType<Player>().onDeath -= Unsubscription;
        FindObjectOfType<Player>().onDeath -= Unsubscription;
    }
    private void UpdateBar()
    {
        var ammoAmount = FindObjectOfType<Player>().CurrentFirearm.magazine.GetValue();
        var ammoText = $"Ammo: {ammoAmount}";
        _ammo.text = ammoText;
        Debug.Log("Ammo bar has been updated");
    }
}