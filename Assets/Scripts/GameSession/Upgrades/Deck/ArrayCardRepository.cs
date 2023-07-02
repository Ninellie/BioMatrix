using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.EntityComponents.Effects;

namespace Assets.Scripts.GameSession.Upgrades.Deck
{
    [Serializable]
    public class ArrayCardRepository : ICardRepository
    {
        private static readonly IEffectRepository EffectRepository = new EffectRepository();

        private static readonly Card[] DefaultCards = new[]
        {
            // Gun deck
            new Card
            {
                Title = "Gun 1",
                Description = "+50% Firearm Damage, +2 magazine capacity",
                DropWeight = 1,
                Id = 1,
                DeckId = 1,
                OrderInDeck = 1,
                Effects = new []
                {
                    EffectRepository.Get("Gun1")
                },
            },
            new Card
            {
                Title = "Gun 2",
                Description = "+50% Firerate for 2 sec after reloading",
                Id = 2,
                DeckId = 1,
                OrderInDeck = 2,
                DropWeight = 1,
                Effects = new[]
                {
                    EffectRepository.Get("Gun2")
                },
            },
            new Card
            {
                Title = "Gun 3",
                Description = "+2 Projectile pierce, +50% Projectile speed",
                Id = 3,
                DeckId = 1,
                OrderInDeck = 3,
                DropWeight = 1,
                Effects = new[]
                {
                    EffectRepository.Get("Gun3")

                },
            },
            // Gun Turret deck
            new Card
            {
                Title = "Gun Turret 1",
                Description = "+ 1 Turret",
                Id = 4,
                DeckId = 2,
                OrderInDeck = 1,
                DropWeight = 1,
                Effects = new[]
                {
                    EffectRepository.Get("GunTurret1")
                }
            },
            new Card
            {
                Title = "Gun Turret 2",
                Description = "Turret shoots where the player shoots, +50 turret Firerate",
                Id = 5,
                DeckId = 2,
                OrderInDeck = 2,
                DropWeight = 1,
                Effects = new[]
                {
                    EffectRepository.Get("GunTurret2")
                },
            },
            new Card
            {
                Title = "Gun Turret 3",
                Description = "While magazine full, turrets get +2 projectiles",
                Id = 6,
                DeckId = 2,
                OrderInDeck = 3,
                DropWeight = 1,
                Effects = new[]
                {
                    EffectRepository.Get("GunTurret3")
                },
            },
            // Gun Vitality deck
            new Card
            {
                Title = "Gun Vitality 1",
                Description = "+ 1 Max HP, +100% Projectile size",
                Id = 7,
                DeckId = 3,
                OrderInDeck = 1,
                DropWeight = 1,
                Effects = new[]
                {
                    EffectRepository.Get("GunVitality1")
                }
            },
            new Card
            {
                Title = "Gun Vitality 2",
                Description = "+ 1 HP Regeneration per minute",
                Id = 8,
                DeckId = 3,
                OrderInDeck = 2,
                DropWeight = 1,
                Effects = new[]
                {
                    EffectRepository.Get("GunVitality2")
                },
            },
            new Card
            {
                Title = "Gun Vitality 3",
                Description = "+100% Firerate, +50% Firearm Damage while you have only 1 hp",
                Id = 9,
                DeckId = 3,
                OrderInDeck = 3,
                DropWeight = 1,
                Effects = new[]
                {
                    EffectRepository.Get("GunVitality3")
                },
            },
            // Gun Movement Experience deck
            new Card
            {
                Title = "Gun Movement Experience 1",
                Description = "Gain stack of Adrenalin per exp taken. Adrenalin is the effect, that gives you +1% Movement speed and +1% Firerate per stack. Maximum 50 stacks. Duration is updated when a new stack is received.",
                Id = 10,
                DeckId = 4,
                OrderInDeck = 1,
                DropWeight = 1,
                Effects = new[]
                {
                    EffectRepository.Get("GunMovementExperience1")
                }
            },
            new Card
            {
                Title = "Gun Movement Experience 2",
                Description = "+25% Movement speed multiplier, +25% Projectile speed multiplier",
                Id = 11,
                DeckId = 4,
                OrderInDeck = 2,
                DropWeight = 1,
                Effects = new[]
                {
                    EffectRepository.Get("GunMovementExperience2")
                },
            },
            new Card
            {
                Title = "Gun Movement Experience 3",
                Description = "+100% Experience gain multiplier",
                Id = 12,
                DeckId = 4,
                OrderInDeck = 3,
                DropWeight = 1,
                Effects = new[]
                {
                    EffectRepository.Get("GunMovementExperience3")
                },
            },
            // Turret deck
            new Card
            {
                Title = "Turret 1",
                Description = "+1 turret",
                Id = 13,
                DeckId = 5,
                OrderInDeck = 1,
                DropWeight = 1,
                Effects = new[]
                {
                    EffectRepository.Get("Turret1")
                }
            },
            new Card
            {
                Title = "Turret 2",
                Description = "Turrets gets + 2 projectiles on kill for 2 sec",
                Id = 14,
                DeckId = 5,
                OrderInDeck = 2,
                DropWeight = 1,
                Effects = new[]
                {
                    EffectRepository.Get("Turret2")
                },
            },
            new Card
            {
                Title = "Turret 3",
                Description = "+1 turret proj pierce\r\n+ 50% turret firerate",
                Id = 15,
                DeckId = 5,
                OrderInDeck = 3,
                DropWeight = 1,
                Effects = new[]
                {
                    EffectRepository.Get("Turret3")
                },
            },
            // Turret Shield deck
            new Card
            {
                Title = "Turret Shield 1",
                Description = "+1 rechargeable shield layer",
                Id = 16,
                DeckId = 6,
                OrderInDeck = 1,
                DropWeight = 1,
                Effects = new[]
                {
                    EffectRepository.Get("TurretShield1")
                }
            },
            new Card
            {
                Title = "Turret Shield 2",
                Description = "+ 100% shield regeneration rate",
                Id = 17,
                DeckId = 6,
                OrderInDeck = 2,
                DropWeight = 1,
                Effects = new[]
                {
                    EffectRepository.Get("TurretShield2")
                },
            },
            new Card
            {
                Title = "Turret Shield 3",
                Description = "+50% turret damage while has shield\r\n+1 turret while has no shield",
                Id = 18,
                DeckId = 6,
                OrderInDeck = 3,
                DropWeight = 1,
                Effects = new[]
                {
                    EffectRepository.Get("TurretShield3")
                },
            },
            // Turret Movement Magnetism deck
            new Card
            {
                Title = "Turret Movement Magnetism 1",
                Description = "+25% magnetism radius, \r\n+10% movement speed per active turret",
                Id = 19,
                DeckId = 7,
                OrderInDeck = 1,
                DropWeight = 1,
                Effects = new[]
                {
                    EffectRepository.Get("TurretMovementMagnetism1")
                }
            },
            new Card
            {
                Title = "Turret Movement Magnetism 2",
                Description = "+ 25% movement speed,\r\n+25% magnetism radius",
                Id = 20,
                DeckId = 7,
                OrderInDeck = 2,
                DropWeight = 1,
                Effects = new[]
                {
                    EffectRepository.Get("TurretMovementMagnetism2")
                },
            },
            new Card
            {
                Title = "Turret Movement Magnetism 3",
                Description = "+1 turret\r\n+ 50% magnetism radius",
                Id = 21,
                DeckId = 7,
                OrderInDeck = 3,
                DropWeight = 1,
                Effects = new[]
                {
                    EffectRepository.Get("TurretMovementMagnetism3")
                },
            },
            // Vitality Experience deck
            new Card
            {
                Title = "Vitality Experience 1",
                Description = "+1 max hp\r\n+25% exp gained",
                Id = 22,
                DeckId = 8,
                OrderInDeck = 1,
                DropWeight = 1,
                Effects = new[]
                {
                    EffectRepository.Get("VitalityExperience1")
                }
            },
            new Card
            {
                Title = "Vitality Experience 2",
                Description = "Restore 1 hp per 20 exp taken",
                Id = 23,
                DeckId = 8,
                OrderInDeck = 2,
                DropWeight = 1,
                Effects = new[]
                {
                    EffectRepository.Get("VitalityExperience2")
                },
            },
            new Card
            {
                Title = "Vitality Experience 3",
                Description = "+100% exp gained while on 1 hp",
                Id = 24,
                DeckId = 8,
                OrderInDeck = 3,
                DropWeight = 1,
                Effects = new[]
                {
                    EffectRepository.Get("VitalityExperience3")
                },
            },
            // Shield Magnetism deck
            new Card
            {
                Title = "Shield Magnetism 1",
                Description = "+25% magnetism radius\r\n+ 50% shield recharge rate",
                Id = 25,
                DeckId = 9,
                OrderInDeck = 1,
                DropWeight = 100,
                Effects = new[]
                {
                    EffectRepository.Get("ShieldMagnetism1")
                }
            },
            new Card
            {
                Title = "Shield Magnetism 2",
                Description = "+1 max rechargeable shield layer\r\n+ 25% magnetism radius",
                Id = 26,
                DeckId = 9,
                OrderInDeck = 2,
                DropWeight = 100,
                Effects = new[]
                {
                    EffectRepository.Get("ShieldMagnetism2")
                },
            },
            new Card
            {
                Title = "Shield Magnetism 3",
                Description = "+100% magnetism radius while shield is fully charged",
                Id = 27,
                DeckId = 9,
                OrderInDeck = 3,
                DropWeight = 100,
                Effects = new[]
                {
                    EffectRepository.Get("ShieldMagnetism3")
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
        /// Returns a cards by its deck identifier
        /// </summary>
        /// <param name="i">Id of card</param>
        /// <param name="deckId">Deck id</param>
        /// <returns>Card</returns>
        public List<Card> GetCardsByDeckId(int deckId)
        {
            return _cards.Where(card => card.DeckId == deckId).ToList();
        }

        /// <summary>
        /// Returns a card by its identifier
        /// </summary>
        /// <param name="id">Card id</param>
        /// <returns>Card</returns>
        public Card GetCardById(int id)
        {
            return _cards.First(card => card.Id == id);
        }

        /// <summary>
        /// Returns a card by its index in the repository
        /// </summary>
        /// <param name="i">Card index</param>
        /// <returns>Card</returns>
        public Card GetCardByIndex(int i) => _cards[i];

        /// <summary>
        /// Returns the sum of the drop weights of all cards in the repository
        /// </summary>
        /// <returns>Int</returns>
        public int GetDropWeightSum() => _cards.Sum(x => x.DropWeight);

        /// <summary>
        /// Returns the length of the main repository array
        /// </summary>
        public int CardCount => _cards.Length;
    }
}
