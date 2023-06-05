using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Entity.Effects;

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
                Description = "+50% proj dmg, +2 magazine capacity",
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
                Description = "+50% firerate for 2 sec after reloading",
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
                Description = "+2 proj pierce, +50% proj speed",
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
                Description = "+ 1 turret",
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
                Description = "Turret shoots where the player shoots, +50 turret firerate",
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
                Description = "While magazine full, turrets get +2 proj",
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
                Description = "+ 1 max hp, +50% projectile size",
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
                Description = "+ 1 hp regeneration per minute",
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
                Description = "+100% firerate, +50% Dmg while you have only 1 hp",
                Id = 9,
                DeckId = 3,
                OrderInDeck = 3,
                DropWeight = 1,
                Effects = new[]
                {
                    EffectRepository.Get("GunVitality3")
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
