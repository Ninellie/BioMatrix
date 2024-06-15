using System;
using System.Collections.Generic;
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
}