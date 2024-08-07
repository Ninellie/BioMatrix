﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameSession.Upgrades.Deck
{
    [CreateAssetMenu]
    public class Hand : ScriptableObject
    {
        [SerializeField] private PatternDeckRepository deckRepository;
        [SerializeField] private List<HandDeckData> handDeckData;
        
        private void OnEnable() => Identify();

        public List<HandDeckData> GetHandData()
        {
            return handDeckData;
        }

        public void TakeCardFromDeck(string deckName)
        {
            if (deckRepository == null)
            { 
                Debug.LogWarning($"Can't take card from deck. Deck Repository is null", this);
                return;
            }
            
            var cardPos = 0;
            foreach (var deckData in handDeckData)
            {
                if (deckData.size < deckData.openedCardPosition) return;
                if (deckData.name != deckName) continue;
                cardPos = deckData.openedCardPosition;
                deckData.openedCardPosition++;
            }

            var deck = deckRepository.Decks.First(d => d.name.Equals(deckName));
            deck.cards.ToArray()[cardPos].status = CardStatus.Obtained;

            foreach (var onTakenEvent in deck.cards.ToArray()[cardPos].onTaken)
            {
                onTakenEvent.Raise();
            }
        }

        private void Identify()
        {
            if (deckRepository == null)
            { 
                Debug.LogWarning($"Can't identify hand. Deck Repository is null", this);
                return;
            }
            
            var decks = deckRepository.GetDecks();
            
            if (decks == null)
            { 
                Debug.LogWarning($"Can't identify hand. Deck Repository вернул вместо массива decks null", this);
                return;
            }

            handDeckData = new List<HandDeckData>();
            
            foreach (var deck in decks)
            {
                if (deck.cards == null)
                { 
                    Debug.LogWarning($"Can't identify hand. Deck Repository's Cards is null", this);
                    return;
                }
                handDeckData.Add(new HandDeckData(deck.name, deck.cards.Count));
            }
        }
    }
}