using System;
using System.Linq;

public class ArrayCardRepository : ICardRepository
{
    private static readonly Card[] DefaultCards = new Card[]
    {
        new Card
        {
            Title = "Movement speed",
            Description = "+ 25% to movement speed multiplier",
            DropWeight = 1000,
            InfluencedStats = new[]
            {
                "speed",
            },
            ModifierList = new StatModifier[]
                {
                    new StatModifier(OperationType.Multiplication, 25)
                },
        },
        new Card
        {
            Title = "Maximum HP",
            Description = "+ 1 to maximum HP",
            DropWeight = 1000,
            InfluencedStats = new[]
            {
                "maximumHP",
            },
            ModifierList = new StatModifier[]
            {
                new StatModifier(OperationType.Addition, 1)
            },
        },
        new Card
        {
            Title = "Fire rate",
            Description = "+ 10% to fire rate multiplier",
            DropWeight = 1000,
            InfluencedStats = new[]
            {
                "fireRate",
            },
            ModifierList = new StatModifier[]
            {
                new StatModifier(OperationType.Multiplication, 10)
            },
        },
        new Card
        {
            Title = "Projectile speed",
            Description = "+ 10% to projectile speed multiplier",
            DropWeight = 1000,
            InfluencedStats = new[]
            {
                "projectileSpeed",
            },
            ModifierList = new StatModifier[]
            {
                new StatModifier(OperationType.Multiplication, 10)
            },
        },
        new Card
        {
            Title = "Piercing projectiles",
            Description = "Projectiles pierce + 1 enemy",
            DropWeight = 100,
            InfluencedStats = new[]
            {
                "pierceNumber",
            },
            ModifierList = new StatModifier[]
            {
                new StatModifier(OperationType.Addition, 1)
            },
        },
        new Card
        {
            Title = "Reload speed",
            Description = "+25% to reload speed",
            DropWeight = 1000,
            InfluencedStats = new[]
            {
                "reloadSpeed",
            },
            ModifierList = new StatModifier[]
            {
                new StatModifier(OperationType.Multiplication, 25)
            },
        },
        new Card
        {
            Title = "Additional projectile",
            Description = "The weapon fires an additional secondary projectile",
            DropWeight = 10,
            InfluencedStats = new[]
            {
                "projectileNumber",
            },
            ModifierList = new StatModifier[]
            {
                new StatModifier(OperationType.Addition, 1)
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
