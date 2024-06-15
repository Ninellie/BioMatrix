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

        public HandDeckData(string name, int size, int openedCardPosition, float dropWeight)
        {
            this.name = name;
            this.size = size;
            this.openedCardPosition = openedCardPosition;
            this.dropWeight = dropWeight;
        }

        /// <summary>
        /// Without opened cards, with base weight 1
        /// </summary>
        /// <param name="name"></param>
        /// <param name="size"></param>
        public HandDeckData(string name, int size) : this(name, size, 0, 1)
        {
        }
    }
}