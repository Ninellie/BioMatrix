using System.Collections.Generic;
using UnityEngine;

public class LevelUp : MonoBehaviour
{
    public TMPro.TMP_Text[] cardsText;

    private List<Card> _selectedCards = new();
    private static readonly ICardRepository CardRepository = new ArrayCardRepository();
    private readonly CardManager _cardManager = new(CardRepository);
    public void DisplayCards()
    {
        _selectedCards = _cardManager.GetDeck(cardsText.Length);

        for (var i = 0; i < cardsText.Length; i++)
        {
            cardsText[i].text = _selectedCards[i].Title;
        }
    }
    public void Improve(int index)
    {
        for (var i = 0; i < _selectedCards[index].InfluencedStats.Length; i++)
        {
            var statName = _selectedCards[index].InfluencedStats[i];
            var modifier = _selectedCards[index].ModifierList[i];
            Debug.Log(statName);
            Debug.Log(modifier);

            switch (statName)
            {
                case "speed":
                    FindObjectOfType<Player>().AddStatModifier(statName, modifier);
                    break;
                case "maximumLifePoints":
                    FindObjectOfType<Player>().AddStatModifier(statName, modifier);
                    break;
                case "magnetismRadius":
                    FindObjectOfType<Player>().AddStatModifier(statName, modifier);
                    break;
                case "maxApproachableShieldLayers":
                    FindObjectOfType<Player>().AddStatModifier(statName, modifier);
                    break;
                case "lifeRegenerationPerSecond":
                    FindObjectOfType<Player>().AddStatModifier(statName, modifier);
                    break;
                case "fireRate":
                    FindObjectOfType<Firearm>().AddStatModifier(statName, modifier);
                    break;
                case "projectileSpeed":
                    FindObjectOfType<Firearm>().AddStatModifier(statName, modifier);
                    break;
                case "pierceNumber":
                    FindObjectOfType<Firearm>().AddStatModifier(statName, modifier);
                    break;
                case "reloadSpeed":
                    FindObjectOfType<Firearm>().AddStatModifier(statName, modifier);
                    break;
                case "projectileNumber":
                    FindObjectOfType<Firearm>().AddStatModifier(statName, modifier);
                    break;
            }
        }
    }
    
}
