using UnityEngine;

public class HPUI : MonoBehaviour
{
    public TMPro.TMP_Text hpUI;
    //public TMPro.TMP_Text ammoUI;
    //private Player CurrentPlayer => GameObject.FindGameObjectsWithTag("Player")[0]
    //    .GetComponent<Player>();

    private void Start()
    {
        UpdateLifePointsBar();
    }
    private void OnEnable()
    {
        GameObject.FindGameObjectsWithTag("Player")[0]
            .GetComponent<Player>().onCurrentLifePointsChanged += UpdateLifePointsBar;
    }
    private void OnDisable()
    {
        GameObject.FindGameObjectsWithTag("Player")[0]
            .GetComponent<Player>().onCurrentLifePointsChanged -= UpdateLifePointsBar;
    }
    private void UpdateLifePointsBar()
    {
        var currentLifePointsText = "HP: " + GameObject.FindGameObjectsWithTag("Player")[0]
            .GetComponent<Player>().CurrentLifePoints.ToString();
        hpUI.text = currentLifePointsText;
    }
}
