using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameSession.Upgrades.Deck
{
    [Serializable]
    public class Deck
    {
        public string name;
        public string description;

        [SerializeField] public List<Card> cardsInspectorList;

        public Stack<Card> cards;
    }
}