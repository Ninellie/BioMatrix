using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameSession.Upgrades.Deck
{
    [CreateAssetMenu]
    public class PatternDeckPreset : ScriptableObject
    {
        public List<PatternDeck> patternDeckList;
    }
}