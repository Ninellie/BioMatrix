using System;
using System.Linq;

[Serializable]
public class ArrayCardRepository : ICardRepository
{
    private static readonly IEffectRepository EffectRepository = new EffectRepository();

    private static readonly Card[] DefaultCards = new[]
    {
        new Card
        {
            Title = "Gun 1",
            Description = "+50% proj dmg, +2 magazine capacity",
            DropWeight = 1,
            Effects = new []
            {
                EffectRepository.Get("Gun1")
            },
        },
        new Card
        {
            Title = "Gun 2",
            Description = "+50% firerate for 2 sec after reloading",
            DropWeight = 1,
            Effects = new[]
            {
                EffectRepository.Get("Gun2")
            },
        },
        new Card
        {
            Title = "Gun 3",
            Description = "+2 proj pierce, +100% proj speed",
            DropWeight = 1,
            Effects = new[]
            {
                EffectRepository.Get("Gun3")

            },
        },
        new Card
        {
            Title = "Gun Turret 1",
            Description = "+ 1 turret",
            DropWeight = 1,
            Effects = new[]
            {
                EffectRepository.Get("GunTurret1")
            }
        },
        new Card
        {
            Title = "Gun Turret 2",
            Description = "Turret shoots where the player shoots, +50 turret firerate",
            DropWeight = 100,
            Effects = new[]
            {
                EffectRepository.Get("GunTurret2")
            },
        },
        new Card
        {
            Title = "Gun Turret 3",
            Description = "While magazine full, turrets get +2 proj",
            DropWeight = 1,
            Effects = new[]
            {
                EffectRepository.Get("GunTurret3")
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
