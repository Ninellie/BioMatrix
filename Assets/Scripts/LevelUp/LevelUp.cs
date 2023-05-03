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
        var player = FindObjectOfType<Player>();

        for (var i = 0; i < _selectedCards[index].InfluencedStats.Length; i++)
        {
            var statName = _selectedCards[index].InfluencedStats[i];
            var modifier = _selectedCards[index].ModifierList[i];
            Debug.Log(statName);
            Debug.Log(modifier);
            switch (statName)
            {
                case "speed":
                    player.AddStatModifier(statName, modifier);
                    break;
                case "maximumLifePoints":
                    player.AddStatModifier(statName, modifier);
                    break;
                case "lifeRegenerationPerSecond":
                    player.AddStatModifier(statName, modifier);
                    break;
                case "magnetismRadius":
                    player.AddStatModifier(statName, modifier);
                    break;
                case "maxShieldLayersCount":
                    player.AddStatModifier(statName, modifier);
                    player.AddLayer();
                    break;
                case "maxRechargeableShieldLayersCount":
                    player.AddStatModifier(statName, modifier);
                    player.AddLayer();
                    break;
                case "shieldLayerRechargeRate":
                    player.AddStatModifier(statName, modifier);
                    break;
                case "turretCount":
                    player.AddStatModifier(statName, modifier);
                    break;
                case "fireRate":
                    player.CurrentFirearm.AddStatModifier(statName, modifier);
                    break;
                case "projectileSpeed":
                    player.CurrentFirearm.AddStatModifier(statName, modifier);
                    break;
                case "pierceNumber":
                    player.CurrentFirearm.AddStatModifier(statName, modifier);
                    break;
                case "reloadSpeed":
                    player.CurrentFirearm.AddStatModifier(statName, modifier);
                    break;
                case "projectileNumber":
                    player.CurrentFirearm.AddStatModifier(statName, modifier);
                    break;
            }
        }
    }
    
}
