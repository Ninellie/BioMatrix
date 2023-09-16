﻿using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents;
using UnityEngine;

namespace Assets.Scripts.GameSession.Upgrades.Deck
{
    [Serializable]
    public class HandDeckData
    {
        public string name;
        public int size;
        public int openedCardPosition;

        public HandDeckData(string name, int size, int openedCardPosition)
        {
            this.name = name;
            this.size = size;
            this.openedCardPosition = openedCardPosition;
        }
        /// <summary>
        /// Without opened cards
        /// </summary>
        /// <param name="name"></param>
        /// <param name="size"></param>
        public HandDeckData(string name, int size) : this(name, size, 0)
        {
        }
    }

    public interface IHand
    {
        List<HandDeckData> GetHandData();
        void TakeCardFromDeck(string deckName);
        void SetEffectRepository(EffectsRepository effectRepository);
        void SetDeckRepository(IDeckRepository deckRepository);
    }

    public class Hand : MonoBehaviour, IHand
    {
        [SerializeField] private List<HandDeckData> _hand;
        [SerializeField] private OverUnitDataAggregator _dataAggregator;
        [SerializeField] private List<Deck> _decks;

        [SerializeField] private EffectsRepository _effectsRepository;
        private IDeckRepository _deckRepository;

        private void Awake()
        {
            _dataAggregator = GetComponent<OverUnitDataAggregator>();
        }
        private void Start()
        {
            _decks = _deckRepository.GetDecks();
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

            var deck = _decks.First(d => d.name.Equals(deckName));
            deck.cards.ToArray()[cardPos].status = CardStatus.Obtained;

            foreach (var effectName in deck.cards.ToArray()[cardPos].effectNames)
            {
                var effect = _effectsRepository.GetEffectByName(effectName);
                _dataAggregator.AddEffect(effect);
            }
        }

        public void SetEffectRepository(EffectsRepository effectRepository)
        {
            _effectsRepository = effectRepository;
        }

        public void SetDeckRepository(IDeckRepository deckRepository)
        {
            _deckRepository = deckRepository;
        }

        private void Identify()
        {
            _hand = new List<HandDeckData>();
            foreach (var deck in _decks)
            {
                var deckSize = deck.cards.Count;
                var deckName = deck.name;
                var handDeckInfo = new HandDeckData(deckName, deckSize);
                _hand.Add(handDeckInfo);
            }
        }
    }
}