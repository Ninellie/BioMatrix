using System;
using UnityEngine;

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
}