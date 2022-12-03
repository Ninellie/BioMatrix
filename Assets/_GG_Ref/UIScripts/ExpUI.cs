using UnityEngine;

public class ExpUI : MonoBehaviour
{
    public TMPro.TMP_Text experienceToNextLevelBar;
    public TMPro.TMP_Text levelBar;

    private void Start()
    {
        UpdateExperienceBar();
        UpdateLevelBar();
    }
    //private void OnEnable()
    //{
    //    FindObjectOfType<Camera>()
    //        .GetComponent<PlayerCreator>().onPlayerCreated += Subscription;
    //}
    //private void OnDisable()
    //{
    //    FindObjectOfType<Camera>()
    //        .GetComponent<PlayerCreator>().onPlayerCreated -= Subscription;
    //}
    public void Subscription()
    {
        Debug.Log("Experience and level started subscribing on current exp and level of Player");
        GameObject.FindGameObjectsWithTag("Player")[0]
            .GetComponent<Player>().onLevelUp += UpdateLevelBar;

        GameObject.FindGameObjectsWithTag("Player")[0]
            .GetComponent<Player>().onExperienceTaken += UpdateExperienceBar;

        GameObject.FindGameObjectsWithTag("Player")[0]
            .GetComponent<Player>().onPlayerDeath += Unsubscription;
    }
    private void Unsubscription()
    {
        Debug.Log("Experience and level started unsubscribing on current exp and level of Player");

        GameObject.FindGameObjectsWithTag("Player")[0]
            .GetComponent<Player>().onLevelUp -= UpdateLevelBar;

        GameObject.FindGameObjectsWithTag("Player")[0]
            .GetComponent<Player>().onExperienceTaken -= UpdateExperienceBar;

        GameObject.FindGameObjectsWithTag("Player")[0]
            .GetComponent<Player>().onPlayerDeath -= Unsubscription;
    }
    private void UpdateExperienceBar()
    {
        var experienceToNextLevelText = "exp to lvl up: "
                                        + GameObject.FindGameObjectsWithTag("Player")[0]
                                            .GetComponent<Player>().ExpToLvlup.ToString();
        experienceToNextLevelBar.text = experienceToNextLevelText;
        Debug.Log("Experience bar was updated");
    }
    private void UpdateLevelBar()
    {
        var levelText = "LVL: " +
                        GameObject.FindGameObjectsWithTag("Player")[0]
                            .GetComponent<Player>().Level.ToString();
        levelBar.text = levelText;
        Debug.Log("Level bar was updated");
    }
}