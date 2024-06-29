using System.Collections.Generic;
using System.Linq;
using Core.Events;
using UnityEngine;

namespace GameSession.Upgrades.Deck
{
    [CreateAssetMenu]
    public class PatternDeckRepository : ScriptableObject
    {
        [SerializeField] public List<PatternDeck> patternDeckList;
        [SerializeField] private List<Deck> _decks;

        public List<Deck> Decks => _decks;

        private void OnValidate()
        {
            foreach (var patternCard in patternDeckList.SelectMany(patternDeck => patternDeck.cardsArray))
            {
                patternCard.name = patternCard.card.title;
            }
        }

        private void Awake()
        {
            _decks = GetDecks();
        }

        private void OnEnable()
        {
            _decks = GetDecks();
        }

        private List<Deck> GetDecks()
        {
            return patternDeckList.Select(patternDeck => GetDeck(patternDeck.name)).ToList();
        }

        private Deck GetDeck(string deckName)
        {
            var patternDeck = patternDeckList.First(patternDeck => patternDeck.name == deckName);

            var deck = new Deck
            {
                cardsInspectorList = new List<Card>(patternDeck.capacity),
                name = patternDeck.name,
                description = patternDeck.description
            };

            for (var index = 0; index < deck.cardsInspectorList.Capacity; index++)
            {
                var card = new Card
                {
                    onTaken = new List<GameEvent>(3)
                };
                deck.cardsInspectorList.Add(card);
            }

            foreach (var patternCard in patternDeck.cardsArray)
            {
                FillDeckWithPatternCard(deck, patternCard);
            }

            deck.cardsInspectorList.Reverse();
            deck.cards = new Stack<Card>(deck.cardsInspectorList);
            deck.cardsInspectorList.Reverse();

            return deck;
        }

        private static void FillDeckWithPatternCard(Deck deck, PatternCard patternCard)
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
                deck.cardsInspectorList[i].onTaken.AddRange(patternCard.card.onTaken);
            }
        }
    }
}