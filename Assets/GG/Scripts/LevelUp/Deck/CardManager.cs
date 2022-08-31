using System.Collections;
using System;

public class CardManager
{
    private System.Random random = new System.Random();
    private readonly ICardRepository cardRepository;

    public CardManager(ICardRepository cardRepository)
    {
        this.cardRepository = cardRepository;
    }

    public Card GetRandomCard()
    {
        //var weights = cards.Select(x => x.dropWeight).ToArray();
        var sum = cardRepository.GetDropWeightSum();

        var n = random.Next(sum);

        var limit = 0;

        for (int i = 0; i < cardRepository.CardCount; i++)
        {
            var card = cardRepository.Get(i);
            limit += card.dropWeight;
            if (n < limit)
            {
                return card;
            }
        }
        throw new InvalidOperationException("");
    }
}
