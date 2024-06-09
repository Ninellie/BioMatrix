using System;
using System.Collections.Generic;
using System.Linq;
using Core.Events;
using UnityEngine;

namespace GameSession.Upgrades.Deck
{
    [Serializable]
    public class Card
    {
        [HideInInspector] public string inspectorName;
        public string title;
        [Multiline] public string description;
        public List<GameEvent> onTaken;
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

    [Serializable]
    public class HandDeckData
    {
        public string name;
        public int size;
        public int openedCardPosition;
        public float dropWeight;

        public HandDeckData(string name, int size, int openedCardPosition, float dropWeight)
        {
            this.name = name;
            this.size = size;
            this.openedCardPosition = openedCardPosition;
            this.dropWeight = dropWeight;
        }

        /// <summary>
        /// Without opened cards, with base weight 1
        /// </summary>
        /// <param name="name"></param>
        /// <param name="size"></param>
        public HandDeckData(string name, int size) : this(name, size, 0, 1)
        {
        }
    }

    public class Hand : MonoBehaviour
    {
        [SerializeField] private PatternDeckRepository _deckRepository;
        [SerializeField] private List<HandDeckData> _hand;
        
        private void Start()
        {
            Identify();
        }

        public List<HandDeckData> GetHandData()
        {
            return _hand;
        }

        public void TakeCardFromDeck(string deckName)
        {
            int cardPos = 0;
            foreach (var deckData in _hand)
            {
                if (deckData.size < deckData.openedCardPosition) return;
                if (deckData.name != deckName) continue;
                cardPos = deckData.openedCardPosition;
                deckData.openedCardPosition++;
            }

            var deck = _deckRepository.Decks.First(d => d.name.Equals(deckName));
            deck.cards.ToArray()[cardPos].status = CardStatus.Obtained;

            foreach (var onTakenEvent in deck.cards.ToArray()[cardPos].onTaken)
            {
                onTakenEvent.Raise();
            }
        }

        private void Identify()
        {
            _hand = new List<HandDeckData>();
            foreach (var deck in _deckRepository.Decks)
            {
                var deckSize = deck.cards.Count;
                var deckName = deck.name;
                var handDeckInfo = new HandDeckData(deckName, deckSize);
                _hand.Add(handDeckInfo);
            }
        }
    }
}