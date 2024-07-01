using System;

namespace GameSession.Upgrades.Deck
{
    [Serializable]
    public class HandDeckData
    {
        public string name;
        public int size;
        public int openedCardPosition;
        public float dropWeight;

        public HandDeckData(string name, int size, int openedCardPosition = 0, float dropWeight = 1)
        {
            this.name = name;
            this.size = size;
            this.openedCardPosition = openedCardPosition;
            this.dropWeight = dropWeight;
        }
    }
}