using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.GameSession.Upgrades.Deck
{
    [Serializable]
    public class PatternCard
    {
        [HideInInspector]
        public string name;

        [Min(0)]
        public int occurrenceFrequency;
        public Card card;
    }

    [Serializable]
    public class PatternDeck
    {
        public string name;
        public string description;
        public int capacity;
        public PatternCard[] cardsArray;
    }

    [Serializable]
    public class Card
    {
        public string title;
        [Multiline] public string description;
        public float dropWeight;
        public List<string> effectNames;
    }

    [Serializable]
    public class Deck
    {
        public string name;
        public string description;

        [SerializeField] public Card[] cardsInitArray;

        public Stack<Card> cards;
    }

    public class DeckRepository : MonoBehaviour
    {
        [SerializeField]
        private bool _usePreset;

        [SerializeField]
        private PatternDeckPreset _preset;

        [SerializeField]
        private List<PatternDeck> _patternDeckList;

        [SerializeField]
        private List<Deck> _deckList;

        [SerializeField]
        private List<Card> _obtainedCards;

        private void Awake()
        {
            FillDeckListFromPattern(_usePreset ? _preset.patternDeckList : _patternDeckList);
        }

        public void ObtainCard(Card card)
        {
            _obtainedCards.Add(card);
            RemoveCard(card);
        }

        public void RemoveCard(Card card)
        {
            foreach (var deck in _deckList)
            {
                if (!deck.cards.TryPeek(out var result))
                    continue;

                if (result == card)
                    deck.cards.Pop();
            }
        }

        public List<Card> GetRandomOpenedCards(int numberOfCards)
        {
            var openedCardsTemp = GetOpenedCards();
            var cardsCount = Mathf.Min(_deckList.Count, numberOfCards);
            var selectedCards = new List<Card>(cardsCount);

            for (int i = 0; i < selectedCards.Capacity; i++)
            {
                var card = GetRandomCardFromList(openedCardsTemp);
                selectedCards.Add(card);
                openedCardsTemp.Remove(card);
            }
            return selectedCards;
        }

        private List<Card> GetOpenedCards()
        {
            var cards = new List <Card>();
            cards.AddRange(_deckList.Select(deck => deck.cards.Peek()));
            return cards;
        }

        private Card GetRandomCardFromList(List<Card> cardList)
        {
            var sum = cardList.Sum(x => x.dropWeight);
            var next = Random.Range(0, sum);
            var limit = 0f;

            foreach (var card in cardList)
            {
                limit += card.dropWeight;
                if (next < limit)
                {
                    return card;
                }
            }
            throw new InvalidOperationException("");
        }


        private void FillDeckListFromPattern(List<PatternDeck> patternDeckList)
        {
            foreach (var patternDeck in patternDeckList)
            {
                var deck = new Deck
                {
                    cardsInitArray = new Card[patternDeck.capacity],
                    name = patternDeck.name,
                    description = patternDeck.description,
                };

                for (var index = 0; index < deck.cardsInitArray.Length; index++)
                {
                    deck.cardsInitArray[index] = new Card();
                    deck.cardsInitArray[index].effectNames = new List<string>();
                }

                foreach (var patternCard in patternDeck.cardsArray)
                {
                    for (int i = 0; i < deck.cardsInitArray.Length; i++)
                    {
                        if (patternCard.occurrenceFrequency == 0) continue;
                        if ((i + 1) % patternCard.occurrenceFrequency != 0) continue;
                        
                        deck.cardsInitArray[i].title = $"{deck.name} {i + 1} Level";
                        deck.cardsInitArray[i].description += string.IsNullOrEmpty(deck.cardsInitArray[i].description)
                            ? patternCard.card.description
                            : "\r\n" + patternCard.card.description;
                        deck.cardsInitArray[i].dropWeight = patternCard.card.dropWeight;
                        deck.cardsInitArray[i].effectNames.AddRange(patternCard.card.effectNames);
                    }
                }

                deck.cards = new Stack<Card>(deck.cardsInitArray.Reverse());

                _deckList.Add(deck);
            }
        }
    }
}