using System;
using System.Collections.Generic;

namespace Assets.Scripts.GameSession.Upgrades.Deck
{
    public class OldDeck
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public string Description { get; set; }
        public CardTag Tags { get; set; }
        public Stack<int> CardsId { get; set; }
    }
}