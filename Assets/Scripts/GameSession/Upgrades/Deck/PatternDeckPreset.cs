using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.GameSession.Upgrades.Deck
{
    [CreateAssetMenu]
    public class PatternDeckPreset : ScriptableObject
    {
        public List<PatternDeck> patternDeckList;

        public void OnValidate()
        {
            foreach (var patternCard in patternDeckList.SelectMany(patternDeck => patternDeck.cardsArray))
            {
                patternCard.name = patternCard.card.title;
            }
        }
    }
}