using System.Collections.Generic;
using UnityEngine;

public class LevelUp : MonoBehaviour
{
    public TMPro.TMP_Text[] cardsNameText;
    public TMPro.TMP_Text[] cardsDescriptionText;

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
        _selectedCards = _cardManager.GetDeck(cardsNameText.Length);

        for (var i = 0; i < cardsNameText.Length; i++)
        {
            cardsNameText[i].text = _selectedCards[i].Title;
            cardsDescriptionText[i].text = _selectedCards[i].Description;
        }
    }
    public void Improve(int index) // Get bonus
    {
        _storage.AddCard(_selectedCards[index]);
        Debug.LogWarning("Improve");
    }
}
