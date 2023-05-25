using UnityEngine;

public class LifeBar : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text _lifebar;
    private Player _player;
    private void Start()
    {
        UpdateBar();
    }
    public void Subscription()
    {
        Debug.Log("Life bar started subscribing on current life points of Player");
        _player = FindObjectOfType<Player>();
        _player.MaximumLifePoints.ValueChangedEvent += UpdateBar;
        _player.LifePoints.ValueChangedEvent += UpdateBar;
        _player.OnDeath += Unsubscription;
    }
    private void Unsubscription()
    {
        Debug.Log("Life bar started unsubscribing from current life points of Player");
        _player.MaximumLifePoints.ValueChangedEvent -= UpdateBar;
        _player.LifePoints.ValueChangedEvent -= UpdateBar;
        _player.OnDeath -= Unsubscription;
    }
    private void UpdateBar()
    {
        var text =
            $"HP: {_player.LifePoints.GetValue()} / {_player.MaximumLifePoints.Value}";
        _lifebar.text = text;
        Debug.Log("Life bar has been updated");
    }
}