using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.EntityComponents.Effects;

namespace Assets.Scripts.GameSession.Upgrades.Deck
{
    public class TagsWeightMultiplierData
    {
        private readonly Dictionary<CardTag, float> _multipliers;

        public float GetMultiplier(CardTag[] tags)
        {
            if (tags.Length == 0) throw new ArgumentException("Value cannot be an empty collection.", nameof(tags));

            return tags.Aggregate(1f, (current, tag) => current * _multipliers[tag]);
        }

        public TagsWeightMultiplierData()
        {
            _multipliers = new Dictionary<CardTag, float>
            {
                { CardTag.Gun, 1f },
                { CardTag.Turret, 1f },
                { CardTag.Vitality, 1f },
                { CardTag.Shield, 1f },
                { CardTag.Movement, 0.5f },
                { CardTag.Magnetism, 0.5f },
                { CardTag.Experience, 0.5f },
            };
        }
    }

    //[Serializable]
    //public class ArrayCardRepository : ICardRepository
    //{
    //    private static readonly IEffectRepository EffectRepository; // = new EffectRepository();

    //    private static TagsWeightMultiplierData _multiplierData = new();

    //    private static readonly OldCard[] DefaultCards = new[]
    //    {
    //        // Gun deck
    //        new OldCard
    //        {
    //            Title = "Gun 1",
    //            Description = "+50% Damage\r\n+2 Magazine capacity",
    //            Tags = new[] { CardTag.Gun },
    //            Id = 1,
    //            DeckId = 1,
    //            OrderInDeck = 1,
    //            DropWeight = 1 * _multiplierData.GetMultiplier(new[] { CardTag.Gun }),
    //            Effects = new []
    //            {
    //                EffectRepository.Get("Gun1")
    //            },
    //        },
    //        new OldCard
    //        {
    //            Title = "Gun 2",
    //            Description = "After reloading, gain +50% Firerate for 2 sec",
    //            Id = 2,
    //            DeckId = 1,
    //            OrderInDeck = 2,
    //            DropWeight = 1 * _multiplierData.GetMultiplier(new[] { CardTag.Gun }),
    //            Effects = new[]
    //            {
    //                EffectRepository.Get("Gun2")
    //            },
    //        },
    //        new OldCard
    //        {
    //            Title = "Gun 3",
    //            Description = "+2 Projectile pierce\r\n+50% Projectile speed",
    //            Id = 3,
    //            DeckId = 1,
    //            OrderInDeck = 3,
    //            DropWeight = 1 * _multiplierData.GetMultiplier(new[] { CardTag.Gun }),
    //            Effects = new[]
    //            {
    //                EffectRepository.Get("Gun3")

    //            },
    //        },
    //        // Gun Turret deck
    //        new OldCard
    //        {
    //            Title = "Gun Turret 1",
    //            Description = "+1 Turret",
    //            Id = 4,
    //            DeckId = 2,
    //            OrderInDeck = 1,
    //            DropWeight = 1 * _multiplierData.GetMultiplier(new[] { CardTag.Gun, CardTag.Turret }),
    //            Effects = new[]
    //            {
    //                EffectRepository.Get("GunTurret1")
    //            }
    //        },
    //        new OldCard
    //        {
    //            Title = "Gun Turret 2",
    //            Description = "Turret shoots where the player shoots\r\n+50 Turret Firerate",
    //            Id = 5,
    //            DeckId = 2,
    //            OrderInDeck = 2,
    //            DropWeight = 1 * _multiplierData.GetMultiplier(new[] { CardTag.Gun, CardTag.Turret }),
    //            Effects = new[]
    //            {
    //                EffectRepository.Get("GunTurret2")
    //            },
    //        },
    //        new OldCard
    //        {
    //            Title = "Gun Turret 3",
    //            Description = "+2 Projectiles for Turrets, while magazine is full",
    //            Id = 6,
    //            DeckId = 2,
    //            OrderInDeck = 3,
    //            DropWeight = 1 * _multiplierData.GetMultiplier(new[] { CardTag.Gun, CardTag.Turret }),
    //            Effects = new[]
    //            {
    //                EffectRepository.Get("GunTurret3")
    //            },
    //        },
    //        // Gun Vitality deck
    //        new OldCard
    //        {
    //            Title = "Gun Vitality 1",
    //            Description = "+ 1 Max Health\r\n+100% Projectile size multiplier",
    //            Id = 7,
    //            DeckId = 3,
    //            OrderInDeck = 1,
    //            DropWeight = 1 * _multiplierData.GetMultiplier(new[] { CardTag.Gun, CardTag.Vitality }),
    //            Effects = new[]
    //            {
    //                EffectRepository.Get("GunVitality1")
    //            }
    //        },
    //        new OldCard
    //        {
    //            Title = "Gun Vitality 2",
    //            Description = "+ 1 Health Regeneration per minute",
    //            Id = 8,
    //            DeckId = 3,
    //            OrderInDeck = 2,
    //            DropWeight = 1 * _multiplierData.GetMultiplier(new[] { CardTag.Gun, CardTag.Vitality }),
    //            Effects = new[]
    //            {
    //                EffectRepository.Get("GunVitality2")
    //            },
    //        },
    //        new OldCard
    //        {
    //            Title = "Gun Vitality 3",
    //            Description = "+100% Firerate\r\n+50% Damage while you have only 1 Health Point",
    //            Id = 9,
    //            DeckId = 3,
    //            OrderInDeck = 3,
    //            DropWeight = 1 * _multiplierData.GetMultiplier(new[] { CardTag.Gun, CardTag.Vitality }),
    //            Effects = new[]
    //            {
    //                EffectRepository.Get("GunVitality3")
    //            },
    //        },
    //        // Gun Movement Experience deck
    //        new OldCard
    //        {
    //            Title = "Gun Movement Experience 1",
    //            Description = "Gain stack of Adrenalin for 5 sec per exp taken\r\nAdrenalin gives you +5% Movement Speed and +5% Firerate per stack\r\nMaximum 10 stacks\r\nDuration is updated when a new stack is received",
    //            Id = 10,
    //            DeckId = 4,
    //            OrderInDeck = 1,
    //            DropWeight = 1 * _multiplierData.GetMultiplier(new[] { CardTag.Gun, CardTag.Movement, CardTag.Experience }),
    //            Effects = new[]
    //            {
    //                EffectRepository.Get("GunMovementExperience1")
    //            }
    //        },
    //        new OldCard
    //        {
    //            Title = "Gun Movement Experience 2",
    //            Description = "+25% Movement Speed\r\n+25% Projectile Speed",
    //            Id = 11,
    //            DeckId = 4,
    //            OrderInDeck = 2,
    //            DropWeight = 1 * _multiplierData.GetMultiplier(new[] { CardTag.Gun, CardTag.Movement, CardTag.Experience }),
    //            Effects = new[]
    //            {
    //                EffectRepository.Get("GunMovementExperience2")
    //            },
    //        },
    //        new OldCard
    //        {
    //            Title = "Gun Movement Experience 3",
    //            Description = "+100% Experience gain",
    //            Id = 12,
    //            DeckId = 4,
    //            OrderInDeck = 3,
    //            DropWeight = 1 * _multiplierData.GetMultiplier(new[] { CardTag.Gun, CardTag.Movement, CardTag.Experience }),
    //            Effects = new[]
    //            {
    //                EffectRepository.Get("GunMovementExperience3")
    //            },
    //        },
    //        // Turret deck
    //        new OldCard
    //        {
    //            Title = "Turret 1",
    //            Description = "+1 Turret",
    //            Id = 13,
    //            DeckId = 5,
    //            OrderInDeck = 1,
    //            DropWeight = 1 * _multiplierData.GetMultiplier(new[] { CardTag.Turret}),
    //            Effects = new[]
    //            {
    //                EffectRepository.Get("Turret1")
    //            }
    //        },
    //        new OldCard
    //        {
    //            Title = "Turret 2",
    //            Description = "+2 Projectiles for Turrets after Turret's kill for 2 sec",
    //            Id = 14,
    //            DeckId = 5,
    //            OrderInDeck = 2,
    //            DropWeight = 1 * _multiplierData.GetMultiplier(new[] { CardTag.Turret}),
    //            Effects = new[]
    //            {
    //                EffectRepository.Get("Turret2")
    //            },
    //        },
    //        new OldCard
    //        {
    //            Title = "Turret 3",
    //            Description = "+1 Turret Projectile Pierce\r\n+50% Turret Firerate",
    //            Id = 15,
    //            DeckId = 5,
    //            OrderInDeck = 3,
    //            DropWeight = 1 * _multiplierData.GetMultiplier(new[] { CardTag.Turret}),
    //            Effects = new[]
    //            {
    //                EffectRepository.Get("Turret3")
    //            },
    //        },
    //        // Turret Shield deck
    //        new OldCard
    //        {
    //            Title = "Turret Shield 1",
    //            Description = "+1 Rechargeable Shield Layer",
    //            Id = 16,
    //            DeckId = 6,
    //            OrderInDeck = 1,
    //            DropWeight = 1 * _multiplierData.GetMultiplier(new[] { CardTag.Turret, CardTag.Shield}),
    //            Effects = new[]
    //            {
    //                EffectRepository.Get("TurretShield1")
    //            }
    //        },
    //        new OldCard
    //        {
    //            Title = "Turret Shield 2",
    //            Description = "+100% Shield Recharge Rate",
    //            Id = 17,
    //            DeckId = 6,
    //            OrderInDeck = 2,
    //            DropWeight = 1 * _multiplierData.GetMultiplier(new[] { CardTag.Turret, CardTag.Shield}),
    //            Effects = new[]
    //            {
    //                EffectRepository.Get("TurretShield2")
    //            },
    //        },
    //        new OldCard
    //        {
    //            Title = "Turret Shield 3",
    //            Description = "+50% Turret Damage while under the Shield\r\n+1 Turret while has no Shield",
    //            Id = 18,
    //            DeckId = 6,
    //            OrderInDeck = 3,
    //            DropWeight = 1 * _multiplierData.GetMultiplier(new[] { CardTag.Turret, CardTag.Shield}),
    //            Effects = new[]
    //            {
    //                EffectRepository.Get("TurretShield3")
    //            },
    //        },
    //        // Turret Movement Magnetism deck
    //        new OldCard
    //        {
    //            Title = "Turret Movement Magnetism 1",
    //            Description = "+25% Magnetism Radius\r\n+10% Movement Speed per active Turret",
    //            Id = 19,
    //            DeckId = 7,
    //            OrderInDeck = 1,
    //            DropWeight = 1 * _multiplierData.GetMultiplier(new[] { CardTag.Turret, CardTag.Movement, CardTag.Magnetism}),
    //            Effects = new[]
    //            {
    //                EffectRepository.Get("TurretMovementMagnetism1")
    //            }
    //        },
    //        new OldCard
    //        {
    //            Title = "Turret Movement Magnetism 2",
    //            Description = "+25% Movement Speed,\r\n+25% Magnetism Radius",
    //            Id = 20,
    //            DeckId = 7,
    //            OrderInDeck = 2,
    //            DropWeight = 1 * _multiplierData.GetMultiplier(new[] { CardTag.Turret, CardTag.Movement, CardTag.Magnetism}),
    //            Effects = new[]
    //            {
    //                EffectRepository.Get("TurretMovementMagnetism2")
    //            },
    //        },
    //        new OldCard
    //        {
    //            Title = "Turret Movement Magnetism 3",
    //            Description = "+1 Turret\r\n+50% Magnetism Radius",
    //            Id = 21,
    //            DeckId = 7,
    //            OrderInDeck = 3,
    //            DropWeight = 1 * _multiplierData.GetMultiplier(new[] { CardTag.Turret, CardTag.Movement, CardTag.Magnetism}),
    //            Effects = new[]
    //            {
    //                EffectRepository.Get("TurretMovementMagnetism3")
    //            },
    //        },
    //        // Vitality Experience deck
    //        new OldCard
    //        {
    //            Title = "Vitality Experience 1",
    //            Description = "+1 Max Health\r\n+25% Experience gain",
    //            Id = 22,
    //            DeckId = 8,
    //            OrderInDeck = 1,
    //            DropWeight = 1 * _multiplierData.GetMultiplier(new[] { CardTag.Vitality, CardTag.Experience}),
    //            Effects = new[]
    //            {
    //                EffectRepository.Get("VitalityExperience1")
    //            }
    //        },
    //        new OldCard
    //        {
    //            Title = "Vitality Experience 2",
    //            Description = "1 Health Point restored per 20 exp taken",
    //            Id = 23,
    //            DeckId = 8,
    //            OrderInDeck = 2,
    //            DropWeight = 1 * _multiplierData.GetMultiplier(new[] { CardTag.Vitality, CardTag.Experience}),
    //            Effects = new[]
    //            {
    //                EffectRepository.Get("VitalityExperience2")
    //            },
    //        },
    //        new OldCard
    //        {
    //            Title = "Vitality Experience 3",
    //            Description = "+100% Experience gain while you have only 1 Health Point",
    //            Id = 24,
    //            DeckId = 8,
    //            OrderInDeck = 3,
    //            DropWeight = 1 * _multiplierData.GetMultiplier(new[] { CardTag.Vitality, CardTag.Experience}),
    //            Effects = new[]
    //            {
    //                EffectRepository.Get("VitalityExperience3")
    //            },
    //        },
    //        // Shield Magnetism deck
    //        new OldCard
    //        {
    //            Title = "Shield Magnetism 1",
    //            Description = "+25% Magnetism Radius\r\n+50% Shield Recharge Rate",
    //            Id = 25,
    //            DeckId = 9,
    //            OrderInDeck = 1,
    //            DropWeight = 1 * _multiplierData.GetMultiplier(new[] { CardTag.Shield, CardTag.Magnetism}),
    //            Effects = new[]
    //            {
    //                EffectRepository.Get("ShieldMagnetism1")
    //            }
    //        },
    //        new OldCard
    //        {
    //            Title = "Shield Magnetism 2",
    //            Description = "+1 Max Rechargeable Shield Layer\r\n+25% Magnetism Radius",
    //            Id = 26,
    //            DeckId = 9,
    //            OrderInDeck = 2,
    //            DropWeight = 1 * _multiplierData.GetMultiplier(new[] { CardTag.Shield, CardTag.Magnetism}),
    //            Effects = new[]
    //            {
    //                EffectRepository.Get("ShieldMagnetism2")
    //            },
    //        },
    //        new OldCard
    //        {
    //            Title = "Shield Magnetism 3",
    //            Description = "+100% Magnetism Radius while Shield is fully charged",
    //            Id = 27,
    //            DeckId = 9,
    //            OrderInDeck = 3,
    //            DropWeight = 1 * _multiplierData.GetMultiplier(new[] { CardTag.Shield, CardTag.Magnetism}),
    //            Effects = new[]
    //            {
    //                EffectRepository.Get("ShieldMagnetism3")
    //            },
    //        },
    //    };

    //    private readonly OldCard[] _cards;

    //    public ArrayCardRepository() : this(DefaultCards)
    //    {
    //    }

    //    public ArrayCardRepository(OldCard[] cards)
    //    {
    //        _cards = cards;
    //    }

    //    /// <summary>
    //    /// Returns a cards by its deck identifier
    //    /// </summary>
    //    /// <param name="i">Id of card</param>
    //    /// <param name="deckId">Deck id</param>
    //    /// <returns>Card</returns>
    //    public List<OldCard> GetCardsByDeckId(int deckId)
    //    {
    //        return _cards.Where(card => card.DeckId == deckId).ToList();
    //    }

    //    /// <summary>
    //    /// Returns a card by its identifier
    //    /// </summary>
    //    /// <param name="id">Card id</param>
    //    /// <returns>Card</returns>
    //    public OldCard GetCardById(int id)
    //    {
    //        return _cards.First(card => card.Id == id);
    //    }

    //    /// <summary>
    //    /// Returns a card by its index in the repository
    //    /// </summary>
    //    /// <param name="i">Card index</param>
    //    /// <returns>Card</returns>
    //    public OldCard GetCardByIndex(int i) => _cards[i];

    //    /// <summary>
    //    /// Returns the sum of the drop weights of all cards in the repository
    //    /// </summary>
    //    /// <returns>Int</returns>
    //    public float GetDropWeightSum() => _cards.Sum(x => x.DropWeight);

    //    /// <summary>
    //    /// Returns the length of the main repository array
    //    /// </summary>
    //    public int CardCount => _cards.Length;
    //}
}
