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
    public void Subscription()
    {
        Debug.Log("Experience and level started subscribing on current exp and level of Player");
        FindObjectOfType<Player>().LevelUpEvent += UpdateLevelBar;

        FindObjectOfType<Player>().ExperienceTakenEvent += UpdateExperienceBar;

        FindObjectOfType<Player>().OnDeath += Unsubscription;
    }
    private void Unsubscription()
    {
        Debug.Log("Experience and level started unsubscribing on current exp and level of Player");

        FindObjectOfType<Player>().LevelUpEvent -= UpdateLevelBar;

        FindObjectOfType<Player>().ExperienceTakenEvent -= UpdateExperienceBar;

        FindObjectOfType<Player>().OnDeath -= Unsubscription;
    }

    private void UpdateExperienceBar()
    {
        var experienceToNextLevelText = $"exp to lvl up: {FindObjectOfType<Player>().ExpToLvlup}";

    experienceToNextLevelBar.text = experienceToNextLevelText;
        Debug.Log("Experience bar was updated");
    }
    private void UpdateLevelBar()
    {
        var levelText = "LVL: " +
                        FindObjectOfType<Player>().Level.ToString();
        levelBar.text = levelText;
        Debug.Log("Level bar was updated");
    }
}