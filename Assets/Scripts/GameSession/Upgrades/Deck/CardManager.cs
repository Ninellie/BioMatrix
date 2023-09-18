using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

namespace Assets.Scripts.GameSession.Upgrades.Deck
{
    //public class CardManager
    //{
    //    private readonly ICardRepository _cardRepository;
    //    private readonly List<OldCard> _obtainedCards = new();
    //    private readonly OldDeck[] _defaultDecks = 
    //    {
    //        new OldDeck
    //        {
    //            Name = string.Empty,
    //            Id = 1,
    //            Description = string.Empty,
    //            Tags = CardTag.Gun,
    //            CardsId = new Stack<int>()
    //        },
    //        new OldDeck
    //        {
    //            Name = string.Empty,
    //            Id = 2,
    //            Description = string.Empty,
    //            Tags = CardTag.Gun | CardTag.Turret,
    //            CardsId = new Stack<int>()
    //        },
    //        new OldDeck
    //        {
    //            Name = string.Empty,
    //            Id = 3,
    //            Description = string.Empty,
    //            Tags = CardTag.Gun | CardTag.Vitality,
    //            CardsId = new Stack<int>()
    //        },
    //        new OldDeck
    //        {
    //            Name = string.Empty,
    //            Id = 4,
    //            Description = string.Empty,
    //            Tags = CardTag.Gun | CardTag.Movement | CardTag.Experience,
    //            CardsId = new Stack<int>()
    //        },
    //        new OldDeck
    //        {
    //            Name = string.Empty,
    //            Id = 5,
    //            Description = string.Empty,
    //            Tags = CardTag.Turret,
    //            CardsId = new Stack<int>()
    //        },
    //        new OldDeck
    //        {
    //            Name = string.Empty,
    //            Id = 6,
    //            Description = string.Empty,
    //            Tags = CardTag.Turret | CardTag.Shield,
    //            CardsId = new Stack<int>()
    //        },
    //        new OldDeck
    //        {
    //            Name = string.Empty,
    //            Id = 7,
    //            Description = string.Empty,
    //            Tags = CardTag.Turret | CardTag.Movement | CardTag.Magnetism,
    //            CardsId = new Stack<int>()
    //        },
    //        new OldDeck
    //        {
    //            Name = string.Empty,
    //            Id = 8,
    //            Description = string.Empty,
    //            Tags = CardTag.Vitality | CardTag.Experience,
    //            CardsId = new Stack<int>()
    //        },
    //        new OldDeck
    //        {
    //            Name = string.Empty,
    //            Id = 9,
    //            Description = string.Empty,
    //            Tags = CardTag.Shield | CardTag.Magnetism,
    //            CardsId = new Stack<int>()
    //        },
    //    };

    //    public CardManager(ICardRepository cardRepository)
    //    {
    //        this._cardRepository = cardRepository;
    //        foreach (var deck in _defaultDecks)
    //        {
    //            var deckCards = _cardRepository.GetCardsByDeckId(deck.Id);

    //            for (int i = deckCards.Count - 1; i >= 0; i--)
    //            {
    //                var biggestOrder = deckCards.Select(deckCard => deckCard.OrderInDeck).Prepend(0).Max();
    //                var bottomCard = deckCards.First(deckCard => deckCard.OrderInDeck == biggestOrder);
    //                deck.CardsId.Push(bottomCard.Id);
    //                deckCards.Remove(bottomCard);
    //            }
    //        }
    //    }

    //    public List<OldCard> GetRandomOpenedCards(int numberOfCards)
    //    {
    //        var cardsList = GetOpenedCards();

    //        List<OldCard> selectedCards = new();

    //        var cardsCount = cardsList.Count;
    //        if (cardsCount > numberOfCards)
    //        {
    //            cardsCount = numberOfCards;
    //        }

    //        for (int i = 0; i < cardsCount; i++)
    //        {
    //            var card = GetRandomCardFromList(cardsList);
    //            selectedCards.Add(card);
    //            cardsList.Remove(card);
    //        }
    //        return selectedCards;
    //    }

    //    public List<OldCard> GetObtainedCards()
    //    {
    //        return _obtainedCards;
    //    }

    //    public void AddCard(int cardId)
    //    {
    //        _obtainedCards.Add(_cardRepository.GetCardById(cardId));
    //        foreach (var deck in _defaultDecks)
    //        {
    //            if (!deck.CardsId.TryPeek(out var result)) continue;
    //            if (result == cardId)
    //            {
    //                deck.CardsId.Pop();
    //            }
    //        }
    //    }

    //    private List<OldCard> GetOpenedCards()
    //    {
    //        List<int> openedCardIds = new();

    //        foreach (var deck in _defaultDecks)
    //        {
    //            if (deck.CardsId.TryPeek(out var result))
    //            {
    //                openedCardIds.Add(result);
    //            }
    //        }

    //        List<OldCard> openedCards = new(openedCardIds.Count);
    //        openedCards.AddRange(openedCardIds.Select(openedCardId => _cardRepository.GetCardById(openedCardId)));
    //        return openedCards;
    //    }

    //    private OldCard GetRandomCardFromList(List<OldCard> cardList)
    //    {
    //        var sum = cardList.Sum(x => x.DropWeight);
    //        var next = Random.Range(0, sum);
    //        var limit = 0f;

    //        foreach (var card in cardList)
    //        {
    //            limit += card.DropWeight;
    //            if (next < limit)
    //            {
    //                return card;
    //            }
    //        }
    //        throw new InvalidOperationException("");
    //    }
    //}
}