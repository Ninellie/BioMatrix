using System;
using System.Collections.Generic;
using System.Linq;
using log4net.Filter;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.GameSession.Upgrades.Deck
{

    public enum CardStatus
    {
        Opened,
        Closed,
        Obtained
    }

    [Serializable]
    public class Card
    {
        [HideInInspector] public string inspectorName;
        public string title;
        [Multiline] public string description;
        public float dropWeight;
        public List<string> effectNames;
        public CardStatus status;
    }

    [Serializable]
    public class Deck
    {
        public string name;
        public string description;

        [SerializeField] public List<Card> cardsInspectorList;

        public Stack<Card> cards;
    }

    public interface IHand
    {
        Dictionary<string, int> GetHand();
        void TakeCardFromDeck(string deckName);
    }

    public class Hand : MonoBehaviour, IHand
    {
        private Dictionary<string, int> _hand;

        private IDeckRepository _deckRepository;

        private List<Deck> _decks;

        private void Start()
        {
            _decks = _deckRepository.GetDecks();

            _hand = new Dictionary<string, int>(_decks.Count);

            foreach (var deck in _decks)
            {
                _hand.Add(deck.name, 0);
            }
        }

        public void TakeCard(string deckName)
        {
            var deck = _decks.First(d => d.name.Equals(deckName));

            var openedCard = deck.cards.First(c => c.status == CardStatus.Opened);


        }

        public Dictionary<string, int> GetHand()
        {
            return _hand;
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
    }
}