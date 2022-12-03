using System;
using System.Collections.Generic;
using System.Linq;

public class CardManager
{
    private readonly System.Random _random = new();
    private readonly ICardRepository _cardRepository;

    public CardManager(ICardRepository cardRepository)
    {
        this._cardRepository = cardRepository;
    }

    public Card GetRandomCard()
    {
        var sum = _cardRepository.GetDropWeightSum();

        var next = _random.Next(sum);

        var limit = 0;

        for (int i = 0; i < _cardRepository.CardCount; i++)
        {
            var card = _cardRepository.Get(i);
            limit += card.DropWeight;
            if (next < limit)
            {
                return card;
            }
        }
        throw new InvalidOperationException("");
    }

    public Card GetRandomCardFromList(List<Card> cardList)
    {
        var sum = cardList.Sum(x => x.DropWeight);

        var next = _random.Next(sum);

        var limit = 0;

        foreach (var card in cardList)
        {
            limit += card.DropWeight;
            if (next < limit)
            {
                return card;
            }
        }
        throw new InvalidOperationException("");
    }

    public List<Card> GetDeck(int numberOfCards)
    {
        List<Card> cardsList = new();
        
        for (int i = 0; i < _cardRepository.CardCount; i++)
        {
            var card = _cardRepository.Get(i);
            cardsList.Add(card);
        }

        List<Card> selectedCards = new();

        for (int i = 0; i < numberOfCards; i++)
        {
            var card = GetRandomCardFromList(cardsList);
            selectedCards.Add(card);
            cardsList.Remove(card);
        }
        return selectedCards;
    }
}
