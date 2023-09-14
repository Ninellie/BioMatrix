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
        [HideInInspector] public string inspectorName;
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

        [SerializeField] public List<Card> cardsInspectorList;

        public Stack<Card> cards;
    }

    public interface IDeckRepository
    {
        List<Deck> GetDecks();
        Deck GetDeck(string deckName);
    }

    public interface IHand
    {
        Dictionary<string, int> GetHand();
    }

    public class PatternDeckRepository : MonoBehaviour, IDeckRepository
    {
        [SerializeField]
        private PatternDeckPreset _preset;


        public List<Deck> GetDecks()
        {
            var deckList = new List<Deck>();

            foreach (var patternDeck in _preset.patternDeckList)
            {
                
            }
            
            return deckList;
        }

        public Deck GetDeck(string deckName)
        {
            var deck = new Deck();

            var pattern 

            return deck;
        }

        private Card GetCard(string deckName, int cardPosition)
        {

        }

        private void FillDeckListFromPattern(List<PatternDeck> patternDeckList)
        {
            foreach (var patternDeck in patternDeckList)
            {
                var deck = new Deck
                {
                    cardsInspectorList = new List<Card>(patternDeck.capacity),
                    name = patternDeck.name,
                    description = patternDeck.description,
                };

                for (var index = 0; index < deck.cardsInspectorList.Capacity; index++)
                {
                    var card = new Card
                    {
                        effectNames = new List<string>()
                    };

                    deck.cardsInspectorList.Add(card);
                }

                foreach (var patternCard in patternDeck.cardsArray)
                {
                    FillCardFromPattern(deck, patternCard);
                }

                deck.cardsInspectorList.Reverse();
                deck.cards = new Stack<Card>(deck.cardsInspectorList);
                deck.cardsInspectorList.Reverse();
                _deckList.Add(deck);
            }
        }

        private void FillCardFromPattern(Deck deck, PatternCard patternCard)
        {
            for (int i = 0; i < deck.cardsInspectorList.Count; i++)
            {
                if (patternCard.occurrenceFrequency == 0) continue;
                if ((i + 1) % patternCard.occurrenceFrequency != 0) continue;

                deck.cardsInspectorList[i].inspectorName +=
                    string.IsNullOrEmpty(deck.cardsInspectorList[i].inspectorName)
                        ? $"{deck.name} Level {i + 1} | Pattern: {patternCard.occurrenceFrequency}"
                        : $" {patternCard.occurrenceFrequency}";

                deck.cardsInspectorList[i].title = $"{deck.name} \r\n Level {i + 1}";

                deck.cardsInspectorList[i].description +=
                    string.IsNullOrEmpty(deck.cardsInspectorList[i].description)
                    ? patternCard.card.description
                    : "\r\n" + patternCard.card.description;

                deck.cardsInspectorList[i].dropWeight = patternCard.card.dropWeight;

                deck.cardsInspectorList[i].effectNames.AddRange(patternCard.card.effectNames);
            }
        }
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

        private void RemoveCard(Card card)
        {
            foreach (var deck in _deckList)
            {
                if (!deck.cards.TryPeek(out var result)) continue;
                if (result != card) continue;
                deck.cards.Pop();
                card.inspectorName += $" | (TAKEN)";
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

            foreach (var deck in _deckList)
            {
                if (!deck.cards.TryPeek(out var card)) continue;
                cards.Add(card);
            }
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
                    cardsInspectorList = new List<Card>(patternDeck.capacity),
                    name = patternDeck.name,
                    description = patternDeck.description,
                };

                

                for (var index = 0; index < deck.cardsInspectorList.Capacity; index++)
                {
                    var card = new Card
                    {
                        effectNames = new List<string>()
                    };

                    deck.cardsInspectorList.Add(card);
                }

                foreach (var patternCard in patternDeck.cardsArray)
                {
                    FillCardFromPattern(deck, patternCard);
                }

                deck.cardsInspectorList.Reverse();
                deck.cards = new Stack<Card>(deck.cardsInspectorList);
                deck.cardsInspectorList.Reverse();
                _deckList.Add(deck);
            }
        }

        private void FillCardFromPattern(Deck deck, PatternCard patternCard)
        {
            for (int i = 0; i < deck.cardsInspectorList.Count; i++)
            {
                if (patternCard.occurrenceFrequency == 0) continue;
                if ((i + 1) % patternCard.occurrenceFrequency != 0) continue;
                
                deck.cardsInspectorList[i].inspectorName +=
                    string.IsNullOrEmpty(deck.cardsInspectorList[i].inspectorName)
                        ? $"{deck.name} Level {i + 1} | Pattern: {patternCard.occurrenceFrequency}"
                        : $" {patternCard.occurrenceFrequency}";

                deck.cardsInspectorList[i].title = $"{deck.name} \r\n Level {i + 1}";

                deck.cardsInspectorList[i].description += 
                    string.IsNullOrEmpty(deck.cardsInspectorList[i].description) 
                    ? patternCard.card.description
                    : "\r\n" + patternCard.card.description;

                deck.cardsInspectorList[i].dropWeight = patternCard.card.dropWeight;

                deck.cardsInspectorList[i].effectNames.AddRange(patternCard.card.effectNames);
            }
        }
    }
}