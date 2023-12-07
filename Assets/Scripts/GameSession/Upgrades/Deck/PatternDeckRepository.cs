using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.GameSession.Upgrades.Deck
{
    public interface IDeckRepository
    {
        List<Deck> GetDecks();
        Deck GetDeck(string deckName);
    }

    public class PatternDeckRepository : MonoBehaviour
    {
        [SerializeField]
        private PatternDeckPreset _preset;
        
        [SerializeField]
        private PatternDeckPreset _usedDecks;

        
        private void Awake()
        {
            _usedDecks = Instantiate(_preset);
        }

        public List<Deck> GetDecks()
        {
            var deckList = new List<Deck>();

            foreach (var patternDeck in _usedDecks.patternDeckList)
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
                    FillDeckWithPatternCard(deck, patternCard);
                }

                deck.cardsInspectorList.Reverse();
                deck.cards = new Stack<Card>(deck.cardsInspectorList);
                deck.cardsInspectorList.Reverse();
                deckList.Add(deck);
            }
            return deckList;
        }

        public Deck GetDeck(string deckName)
        {
            var patternDeck = _preset.patternDeckList.First(patternDeck => patternDeck.name == deckName);

            var deck = new Deck();
            deck.cardsInspectorList = new List<Card>(patternDeck.capacity);
            deck.name = patternDeck.name;
            deck.description = patternDeck.description;

            for (var index = 0; index < deck.cardsInspectorList.Capacity; index++)
            {
                var card = new Card();
                card.effectNames = new List<string>();
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

        private void FillDeckWithPatternCard(Deck deck, PatternCard patternCard)
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
                deck.cardsInspectorList[i].onTaken.AddRange(patternCard.card.onTaken);
            }
        }
    }
}