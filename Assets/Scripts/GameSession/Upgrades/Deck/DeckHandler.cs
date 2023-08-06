using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameSession.Upgrades.Deck
{
    [Serializable]
    public class PatternCard
    {
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
        [Multiline]
        public string description;
        //public float dropWeight;
    }

    [Serializable]
    public class Deck
    {
        public string name;
        public string description;

        [SerializeField] public Card[] cardsArray;

        public Stack<Card> cards;
    }

    public class DeckHandler : MonoBehaviour
    {
        [SerializeField]
        private bool _usePreset;

        [SerializeField]
        private PatternDeckPreset _preset;

        [SerializeField]
        private List<PatternDeck> _patternDeckList;

        [SerializeField]
        private List<Deck> _deckList;

        private void Awake()
        {
            if (_usePreset)
                _patternDeckList = _preset.patternDeckList;

            FillDeckList(_patternDeckList);

            foreach (var deck in _deckList)
                deck.cards = new Stack<Card>(deck.cardsArray);
        }

        private void FillDeckList(List<PatternDeck> patternDeckList)
        {
            foreach (var patternDeck in patternDeckList)
            {
                var deck = new Deck
                {
                    cardsArray = new Card[patternDeck.capacity],
                    name = patternDeck.name,
                    description = patternDeck.description,
                };

                for (var index = 0; index < deck.cardsArray.Length; index++)
                    deck.cardsArray[index] = new Card();

                foreach (var patternCard in patternDeck.cardsArray)
                {
                    for (int i = 0; i < deck.cardsArray.Length; i++)
                    {
                        if (patternCard.occurrenceFrequency == 0) continue;
                        if ((i + 1) % patternCard.occurrenceFrequency != 0) continue;
                        
                        deck.cardsArray[i].title = $"{i + 1} Level";

                        var description = patternCard.card.description;
                        deck.cardsArray[i].description += string.IsNullOrEmpty(deck.cardsArray[i].description)
                            ? description
                            : "\r\n" + description;
                    }
                }

                _deckList.Add(deck);
            }
        }
    }
}