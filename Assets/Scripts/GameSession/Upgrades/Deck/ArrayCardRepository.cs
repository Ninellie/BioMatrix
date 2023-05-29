﻿using System;
using System.Linq;

public enum CardTag
{
    Gun,
    Turret,
    Vitality,
    Shield,
    Movement,
    Magnetism,
    Experience,
}

[Serializable]
public class ArrayCardRepository : ICardRepository
{
    private static readonly Card[] DefaultCards = new Card[]
    {
        new Card
        {
            Title = "Gun 1",
            Description = "",
            DropWeight = 1,
            Effects = new[]
            {
                EffectRepository.CardEffects["Gun1"]
            }
        },
        new Card
        {
            Title = "Gun 2",
            Description = "",
            DropWeight = 1,
            Effects = new IEffect[]
            {
                EffectRepository.CardEffects["Gun2"]
            },
        },
        new Card
        {
            Title = "Gun 3",
            Description = "",
            DropWeight = 1,
            Effects = new IEffect[]
            {
                EffectRepository.CardEffects["Gun3"]
            },
        },
    };
    private readonly Card[] _cards;

    public ArrayCardRepository() : this(DefaultCards)
    {
    }

    public ArrayCardRepository(Card[] cards)
    {
        _cards = cards;
    }

    /// <summary>
    /// Returns a card by its index in the deck
    /// </summary>
    /// <param name="i">Index of card</param>
    /// <returns>Card</returns>
    public Card Get(int i) => _cards[i];

    /// <summary>
    /// Returns the sum of the drop weights of all cards in the deck
    /// </summary>
    /// <returns>Int</returns>
    public int GetDropWeightSum() => _cards.Sum(x => x.DropWeight);

    /// <summary>
    /// Returns the length of the deck array
    /// </summary>
    public int CardCount => _cards.Length;
}
