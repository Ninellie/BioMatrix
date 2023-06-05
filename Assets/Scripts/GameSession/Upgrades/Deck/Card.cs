﻿using System;
using Assets.Scripts.Entity.Effects;

namespace Assets.Scripts.GameSession.Upgrades.Deck
{
    [Serializable]
    public class Card
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Id { get; set; }
        public int DeckId { get; set; }
        public int OrderInDeck { get; set; }
        public int DropWeight { get; set; }
        public IEffect[] Effects { get; set; }
    }
}