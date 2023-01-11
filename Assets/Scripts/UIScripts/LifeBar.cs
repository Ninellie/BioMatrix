using UnityEngine;

public class LifeBar : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text _life;

    private void Start()
    {
        UpdateBar();
    }
    public void Subscription()
    {
        Debug.Log("Life bar started subscribing on current life points of Player");
        
        FindObjectOfType<Player>().onCurrentLifePointsChanged += UpdateBar;
        FindObjectOfType<Player>().onDeath += Unsubscription;
    }
    private void Unsubscription()
    {
        Debug.Log("Life bar started unsubscribing from current life points of Player");

        FindObjectOfType<Player>().onCurrentLifePointsChanged -= UpdateBar;
        FindObjectOfType<Player>().onDeath -= Unsubscription;
    }
    private void UpdateBar()
    {
        var lifeText =
            $"HP: {FindObjectOfType<Player>().CurrentLifePoints}";
        _life.text = lifeText;
        Debug.Log("Life bar has been updated");
    }
}