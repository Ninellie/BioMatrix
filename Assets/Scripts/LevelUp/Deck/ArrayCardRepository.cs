using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class ArrayCardRepository : ICardRepository
{
    private static readonly Card[] DefaultCards = new Card[]
    {
        new Card
        {
            Title = "Movement speed while on full life",
            Description = "+ 100% to movement speed multiplier while on full life",
            DropWeight = 1000,
            Effects = new IEffect[]
            {
                new AddModWhileResource()
                {
                    Name = "Movement speed while full life card",
                    Description = "+ 100% to movement speed multiplier while on full life",
                    Modifiers = new List<(StatModifier mod, string statName)>
                    {
                        (new StatModifier(OperationType.Multiplication, 100), "Speed"),
                    },
                    TargetName = nameof(Player),
                    AddTrigger = new Trigger
                    {
                        Name = nameof(Entity.LifePoints.FillEvent),
                        PropName = nameof(Entity.LifePoints),
                    },
                    RemoveTrigger = new Trigger
                    {
                        Name = nameof(Entity.LifePoints.DecreaseEvent),
                        PropName = nameof(Entity.LifePoints),
                    },
                    TriggerCondition = new Trigger
                    {
                        Name = nameof(Entity.LifePoints) + "." + nameof(Entity.LifePoints.IsFull),
                        PropName = nameof(Entity.LifePoints) + "." + nameof(Entity.LifePoints.IsFull),
                    }
                }
            },
        },
        new Card
        {
            Title = "Movement speed per",
            Description = "+ 50% to movement speed multiplier per lost hp",
            DropWeight = 1,
            Effects = new IEffect[]
            {
                new AddModPerMissingResource
                {
                    Name = "Movement speed per lost hp card",
                    Description = "+ 50% to movement speed multiplier per lost hp",
                    Modifiers = new List<(StatModifier mod, string statName)>
                    {
                        (new StatModifier(OperationType.Multiplication, 30), "Speed"),
                    },
                    TargetName = nameof(Player),
                    TriggerStat = new Trigger
                    {
                        Name = nameof(Entity.MaximumLifePoints.ValueChangedEvent),
                        PropName = nameof(Entity.MaximumLifePoints),
                    },
                    TriggerResource = new Trigger
                    {
                        Name = nameof(Entity.LifePoints.ValueChangedEvent),
                        PropName = nameof(Entity.LifePoints),
                    },
                }
            },
        },
        new Card
        {
            Title = "Movement speed",
            Description = "+ 50% to movement speed multiplier",
            DropWeight = 1,
            Effects = new IEffect[]
            {
                new AddModOn
                {
                    Name = "Movement speed card",
                    Description = "+ 50% to movement speed multiplier",
                    Modifiers = new List<(StatModifier mod, string statName)>
                    {
                        (new StatModifier(OperationType.Multiplication, 200, 2), "Speed"),
                    },
                    TargetName = nameof(Player),
                    Trigger = new Trigger
                    {
                        Name = nameof(Magazine.onEmpty),
                        PropName = nameof(Player.CurrentFirearm) + "." + nameof(Player.CurrentFirearm.Magazine),
                    },
                }
            },
        },
        new Card
        {
            Title = "Maximum life points",
            Description = "+ 1 to maximum HP",
            DropWeight = 1,
            InfluencedStats = new[]
            {
                "maximumLifePoints",
            },
            ModifierList = new StatModifier[]
            {
                new StatModifier(OperationType.Addition, 1)
            },
        },
        new Card
        {
            Title = "Fire rate",
            Description = "+ 25% to fire rate multiplier",
            DropWeight = 1,
            InfluencedStats = new[]
            {
                "fireRate",
            },
            ModifierList = new StatModifier[]
            {
                new StatModifier(OperationType.Multiplication, 25)
            },
        },
        new Card
        {
            Title = "Projectile speed",
            Description = "+ 25% to projectile speed multiplier",
            DropWeight = 1,
            InfluencedStats = new[]
            {
                "projectileSpeed",
            },
            ModifierList = new StatModifier[]
            {
                new StatModifier(OperationType.Multiplication, 25)
            },
        },
        new Card
        {
            Title = "Piercing projectiles",
            Description = "Projectiles pierce + 1 enemy",
            DropWeight = 1,
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
            DropWeight = 1,
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
            DropWeight = 1,
            InfluencedStats = new[]
            {
                "projectileNumber",
            },
            ModifierList = new StatModifier[]
            {
                new StatModifier(OperationType.Addition, 1)
            },
        },
        new Card
        {
            Title = "Magnetism radius",
            Description = "+ 50% to radius of boon attraction",
            DropWeight = 1,
            InfluencedStats = new[]
            {
                "magnetismRadius",
            },
            ModifierList = new StatModifier[]
            {
                new StatModifier(OperationType.Multiplication, 50)
            },
        },
        new Card
        {
            Title = "Life regeneration",
            Description = "Recover one life point per five seconds",
            DropWeight = 5,
            InfluencedStats = new[]
            {
                "lifeRegenerationPerSecond",
            },
            ModifierList = new StatModifier[]
            {
                new StatModifier(OperationType.Addition, 0.2f)
            },
        },
        new Card
        {
            Title = "Max shield layers",
            Description = "Adds +1 to max shield layers and recover one layer instantly",
            DropWeight = 1,
            InfluencedStats = new[]
            {
                "maxShieldLayersCount",
            },
            ModifierList = new StatModifier[]
            {
                new StatModifier(OperationType.Addition, 1)
            },
        },
        new Card
        {
            Title = "Max rechargeable shield layers",
            Description = "Adds +1 to max rechargeable shield layers and recover one layer instantly",
            DropWeight = 1,
            InfluencedStats = new[]
            {
                "maxRechargeableShieldLayersCount",
            },
            ModifierList = new StatModifier[]
            {
                new StatModifier(OperationType.Addition, 1)
            },
        },
        new Card
        {
            Title = "Shield recharge rate",
            Description = "+50 to shield recharge rate multiplier",
            DropWeight = 5,
            InfluencedStats = new[]
            {
                "shieldLayerRechargeRate",
            },
            ModifierList = new StatModifier[]
            {
                new StatModifier(OperationType.Multiplication, 50)
            },
        },
        new Card
        {
            Title = "Turret count",
            Description = "+1 to turret count",
            DropWeight = 5,
            InfluencedStats = new[]
            {
                "turretCount",
            },
            ModifierList = new StatModifier[]
            {
                new StatModifier(OperationType.Addition, 1)
            },
        },
        new Card
        {
            Title = "Turret count for 5 sec",
            Description = "+1 to turret count for 5 sec",
            DropWeight = 1,
            InfluencedStats = new[]
            {
                "turretCount",
            },
            ModifierList = new StatModifier[]
            {
                new StatModifier(OperationType.Addition, 1, 5)
            },
        },
        new Card
        {
            Title = "Turret count",
            Description = "+1 to turret count",
            DropWeight = 1,
            InfluencedStats = new[]
            {
                "turretCount",
            },
            ModifierList = new StatModifier[]
            {
                //new StatModifier(OperationType.Addition, 1, 5, "OnLifePointLost", "TurretCount")
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
