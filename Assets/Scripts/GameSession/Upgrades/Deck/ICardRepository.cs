using System.Collections.Generic;

public interface ICardRepository
{
    int CardCount { get; }

    List<Card> GetCardsByDeckId(int deckId); // True Interface, want to change

    Card GetCardByIndex(int i);

    Card GetCardById(int id);

    int GetDropWeightSum();
}