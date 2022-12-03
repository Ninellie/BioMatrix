using System.Collections.Generic;
using UnityEngine;

public class LevelUp : MonoBehaviour
{
    public GameObject lvlUpUI;
    public GameObject canvasUI;
    public TMPro.TMP_Text[] cardsText;

    private List<Card> _selectedCards = new();
    private static readonly ICardRepository CardRepository = new ArrayCardRepository();
    private readonly CardManager _cardManager = new(CardRepository);
    public void Subscription()
    {
        FindObjectOfType<Player>().onLevelUp += InitiateLvlUp;

        FindObjectOfType<Player>().onPlayerDeath += Unsubscription;
    }
    private void Unsubscription()
    {
        FindObjectOfType<Player>().onLevelUp -= InitiateLvlUp;

        FindObjectOfType<Player>().onPlayerDeath -= Unsubscription;
    }
    private void InitiateLvlUp()
    {
        canvasUI.GetComponent<PauseMenu>().PauseGame();
        DisplayThreeCards();
        lvlUpUI.SetActive(true);
    }
    private void DisplayThreeCards()
    {
        _selectedCards = _cardManager.GetDeck(3);

        for(var i = 0; i < cardsText.Length; i++)
        {
            cardsText[i].text = _selectedCards[i].Title;
        }
    }
    public void Improve(int index)
    {
        for (var i = 0; i < _selectedCards[index].InfluencedStats.Length; i++)
        {
            var improvedStat = _selectedCards[index].InfluencedStats[i];
            var statModifier = _selectedCards[index].ModifierList[i];
            Debug.Log(improvedStat);
            Debug.Log(statModifier);

            switch (improvedStat)
            {
                case "speed":
                    FindObjectOfType<Player>().AddStatModifier(improvedStat, statModifier);
                    break;
                case "maximumLifePoints":
                    FindObjectOfType<Player>().AddStatModifier(improvedStat, statModifier);
                    break;
                case "fireRate":
                    FindObjectOfType<Firearm>().AddStatModifier(improvedStat, statModifier);
                    break;
                case "projectileSpeed":
                    FindObjectOfType<Firearm>().AddStatModifier(improvedStat, statModifier);
                    break;
                case "pierceNumber":
                    FindObjectOfType<Firearm>().AddStatModifier(improvedStat, statModifier);
                    break;
                case "reloadSpeed":
                    FindObjectOfType<Firearm>().AddStatModifier(improvedStat, statModifier);
                    break;
                case "projectileNumber":
                    FindObjectOfType<Firearm>().AddStatModifier(improvedStat, statModifier);
                    break;
            }
        }
    }
}
