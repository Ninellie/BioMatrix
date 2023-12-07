﻿using System;

namespace Assets.Scripts.GameSession.Upgrades.Deck
{
    [Serializable]
    public class PatternDeck
    {
        public string name;
        public string description;
        public int capacity;
        public PatternCard[] cardsArray;
    }
}