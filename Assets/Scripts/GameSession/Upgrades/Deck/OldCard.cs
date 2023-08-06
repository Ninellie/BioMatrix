using System;
using Assets.Scripts.EntityComponents.Effects;

namespace Assets.Scripts.GameSession.Upgrades.Deck
{
    [Serializable]
    public class OldCard
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public CardTag[] Tags { get; set; }
        public int Id { get; set; }
        public int DeckId { get; set; }
        public int OrderInDeck { get; set; }
        public float DropWeight { get; set; }
        public IOldEffect[] Effects { get; set; }
    }
}