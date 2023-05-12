using System.Collections.Generic;
using UnityEngine;

public class LevelUp : MonoBehaviour
{
    public TMPro.TMP_Text[] cardsText;

    private List<Card> _selectedCards = new();
    private static readonly ICardRepository CardRepository = new ArrayCardRepository();
    private readonly CardManager _cardManager = new(CardRepository);
    private CardStorage _storage;

    private void Start()
    {
        _storage = Camera.main.GetComponent<CardStorage>();
    }
    public void DisplayCards()
    {
        _selectedCards = _cardManager.GetDeck(cardsText.Length);

        for (var i = 0; i < cardsText.Length; i++)
        {
            cardsText[i].text = _selectedCards[i].Title;
        }
    }
    public void Improve(int index) // Get bonus
    {
        _storage.AddCard(_selectedCards[index]);
        Debug.Log("Improve");
    }
}
