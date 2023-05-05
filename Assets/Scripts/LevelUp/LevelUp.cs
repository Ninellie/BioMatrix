using System.Collections.Generic;
using UnityEngine;

public class LevelUp : MonoBehaviour
{
    public TMPro.TMP_Text[] cardsText;

    private List<Card> _selectedCards = new();
    private static readonly ICardRepository CardRepository = new ArrayCardRepository();
    private readonly CardManager _cardManager = new(CardRepository);
    private Player _player;

    private void Start()
    {
        _player = FindObjectOfType<Player>();
    }
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
        foreach (var modifier in _selectedCards[index].ModifierList)
        {
            Debug.Log("Improve");
            _player.AddStatModifier(modifier);
        }
    }
    
}
