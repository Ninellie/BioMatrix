using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.GameSession.Upgrades.Deck
{
    public class CardManager
    {
        private readonly Random _random = new();
        private readonly ICardRepository _cardRepository;
        private readonly List<Card> _obtainedCards = new();
        private readonly Deck[] _defaultDecks = 
        {
            new Deck
            {
                Name = string.Empty,
                Id = 1,
                Description = string.Empty,
                Tags = CardTag.Gun,
                CardsId = new Stack<int>()
            },
            new Deck
            {
                Name = string.Empty,
                Id = 2,
                Description = string.Empty,
                Tags = CardTag.Gun | CardTag.Turret,
                CardsId = new Stack<int>()
            },
            new Deck
            {
                Name = string.Empty,
                Id = 3,
                Description = string.Empty,
                Tags = CardTag.Gun | CardTag.Vitality,
                CardsId = new Stack<int>()
            },
            new Deck
            {
                Name = string.Empty,
                Id = 4,
                Description = string.Empty,
                Tags = CardTag.Gun | CardTag.Movement | CardTag.Experience,
                CardsId = new Stack<int>()
            },
            new Deck
            {
                Name = string.Empty,
                Id = 5,
                Description = string.Empty,
                Tags = CardTag.Turret,
                CardsId = new Stack<int>()
            },

        };

        public CardManager(ICardRepository cardRepository)
        {
            this._cardRepository = cardRepository;
            foreach (var deck in _defaultDecks)
            {
                var deckCards = _cardRepository.GetCardsByDeckId(deck.Id);

                for (int i = deckCards.Count - 1; i >= 0; i--)
                {
                    int biggestOrder = deckCards.Select(deckCard => deckCard.OrderInDeck).Prepend(0).Max();
                    var bottomCard = deckCards.First(deckCard => deckCard.OrderInDeck == biggestOrder);
                    deck.CardsId.Push(bottomCard.Id);
                    deckCards.Remove(bottomCard);
                }
            }
        }

        public List<Card> GetRandomOpenedCards(int numberOfCards)
        {
            var cardsList = GetOpenedCards();

            List<Card> selectedCards = new();

            var cardsCount = cardsList.Count;
            if (cardsCount > numberOfCards)
            {
                cardsCount = numberOfCards;
            }

            for (int i = 0; i < cardsCount; i++)
            {
                var card = GetRandomCardFromList(cardsList);
                selectedCards.Add(card);
                cardsList.Remove(card);
            }
            return selectedCards;
        }

        public List<Card> GetObtainedCards()
        {
            return _obtainedCards;
        }

        public void AddCard(int cardId)
        {
            _obtainedCards.Add(_cardRepository.GetCardById(cardId));
            foreach (var deck in _defaultDecks)
            {
                if (!deck.CardsId.TryPeek(out var result)) continue;
                if (result == cardId)
                {
                    deck.CardsId.Pop();
                }
            }
        }

        private List<Card> GetOpenedCards()
        {
            List<int> openedCardIds = new();

            foreach (var deck in _defaultDecks)
            {
                if (deck.CardsId.TryPeek(out var result))
                {
                    openedCardIds.Add(result);
                }
            }

            List<Card> openedCards = new(openedCardIds.Count);
            openedCards.AddRange(openedCardIds.Select(openedCardId => _cardRepository.GetCardById(openedCardId)));
            return openedCards;
        }

        private Card GetRandomCardFromList(List<Card> cardList)
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

        //private List<Card> GetClosedCards()
        //{
        //    var closedCardIds = new List<int>();
        //    closedCardIds.AddRange(_obtainedCards.Select(obtainedCard => obtainedCard.Id));
        //    foreach (var deck in _defaultDecks)
        //    {
        //        var cardIds = deck.CardsId;
        //        cardIds.Pop();
        //        while (cardIds.Count > 0)
        //        {
        //            closedCardIds.Add(cardIds.Pop());
        //        }
        //    }

        //    List<Card> closedCards = new(closedCardIds.Count);
        //    closedCards.AddRange(closedCardIds.Select(cardId => _cardRepository.GetCardById(cardId)));
        //    return closedCards;
        //}

        //private List<Card> GetCards(int[] cardIndexes)
        //{
        //    List<Card> cardsList = new();

        //    for (int i = 0; i < _cardRepository.CardCount; i++)
        //    {
        //        var card = _cardRepository.GetCardByIndex(i);
        //        cardsList.Add(card);
        //    }

        //    for (int i = cardsList.Count - 1; i >= 0; i--)
        //    {
        //        if (cardIndexes.Any(t => cardsList[i].Id == t))
        //        {
        //            cardsList.RemoveAt(i);
        //        }
        //    }

        //    return cardsList;
        //}

        //public List<Card> GetCards(int numberOfCards)
        //{
        //    List<Card> cardsList = new();

        //    for (int i = 0; i < _cardRepository.CardCount; i++)
        //    {
        //        var card = _cardRepository.GetCardByIndex(i);
        //        cardsList.Add(card);
        //    }

        //    List<Card> selectedCards = new();

        //    for (int i = 0; i < numberOfCards; i++)
        //    {
        //        var card = GetRandomCardFromList(cardsList);
        //        selectedCards.Add(card);
        //        cardsList.Remove(card);
        //    }
        //    return selectedCards;
        //}

        //public Card GetRandomCard()
        //{
        //    var sum = _cardRepository.GetDropWeightSum();

        //    var next = _random.Next(sum);

        //    var limit = 0;

        //    for (int i = 0; i < _cardRepository.CardCount; i++)
        //    {
        //        var card = _cardRepository.GetCardByIndex(i);
        //        limit += card.DropWeight;
        //        if (next < limit)
        //        {
        //            return card;
        //        }
        //    }
        //    throw new InvalidOperationException("");
        //}
    }
}