using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.GameSession.Upgrades.Deck
{
    [CreateAssetMenu]
    public class PatternDeckPreset : ScriptableObject, ISerializationCallbackReceiver
    {
        public List<PatternDeck> patternDeckList;

        public void OnBeforeSerialize() { }
        public void OnAfterDeserialize()
        {
            foreach (var patternCard in patternDeckList.SelectMany(patternDeck => patternDeck.cardsArray))
            {
                patternCard.name = patternCard.card.title;
            }
        }
    }
}