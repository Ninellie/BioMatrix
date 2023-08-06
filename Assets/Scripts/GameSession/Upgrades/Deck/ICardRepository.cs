using System.Collections.Generic;

namespace Assets.Scripts.GameSession.Upgrades.Deck
{
    public interface ICardRepository
    {
        int CardCount { get; }

        List<OldCard> GetCardsByDeckId(int deckId); // True Interface, want to change

        OldCard GetCardByIndex(int i);

        OldCard GetCardById(int id);

        float GetDropWeightSum();
    }
}