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
            ModifierList = new Modifier[]
                {
                    new Modifier
                    {
                        Target = EntityType.Player,
                        ParameterName = "movementSpeed",
                        Operation = Operation.Multiplication,
                        Value = 25,
                    }
                },
        },
        new Card
        {
            Title = "Maximum HP",
            Description = "+ 1 to maximum HP",
            DropWeight = 1000,
            ModifierList = new Modifier[]
            {
                new Modifier
                {
                    Target = EntityType.Player,
                    ParameterName = "maximumHP",
                    Operation = Operation.Addition,
                    Value = 1,
                }
            },
        },
        new Card
        {
            Title = "Fire rate",
            Description = "+ 10% to fire rate multiplier",
            DropWeight = 1000,
            ModifierList = new Modifier[]
            {
                new Modifier
                {
                    Target = EntityType.Player,
                    ParameterName = "fireRate",
                    Operation = Operation.Multiplication,
                    Value = 10,
                }
            },
        },
        new Card
        {
            Title = "Projectile speed",
            Description = "+ 10% to projectile speed multiplier",
            DropWeight = 1000,
            ModifierList = new Modifier[]
            {
                new Modifier
                {
                    Target = EntityType.Player,
                    ParameterName = "projectileSpeed",
                    Operation = Operation.Multiplication,
                    Value = 10,
                }
            },
        },
        new Card
        {
            Title = "Piercing projectiles",
            Description = "Projectiles pierce + 1 enemy",
            DropWeight = 100,
            ModifierList = new Modifier[]
            {
                new Modifier
                {
                    Target = EntityType.Player,
                    ParameterName = "pierceNumber",
                    Operation = Operation.Addition,
                    Value = 1,
                }
            },
        },
        new Card
        {
            Title = "Reload speed",
            Description = "+25% to reload speed",
            DropWeight = 1000,
            ModifierList = new Modifier[]
            {
                new Modifier
                {
                    Target = EntityType.Player,
                    ParameterName = "reloadSpeed",
                    Operation = Operation.Multiplication,
                    Value = 25,
                }
            },
        },
        new Card
        {
            Title = "Additional projectile",
            Description = "The weapon fires an additional secondary projectile",
            DropWeight = 10,
            ModifierList = new Modifier[]
            {
                new Modifier
                {
                    Target = EntityType.Player,
                    ParameterName = "projectileNumber",
                    Operation = Operation.Addition,
                    Value = 1,
                }
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
