public interface ICardRepository
{
    int CardCount { get; }

    Card Get(int i);

    int GetDropWeightSum();
}