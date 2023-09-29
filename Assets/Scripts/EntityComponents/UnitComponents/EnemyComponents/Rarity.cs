using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.EntityComponents.UnitComponents.EnemyComponents
{
    public class Rarity
    {
        // Rarity preset?
        public RarityEnum Value { get; set; }
        private readonly Color _magic = new(0.8352942f, 0.2352941f, 0.4156863f);
        private readonly Color _rare = new(1f, 0.509804f, 0.454902f);
        private const float NormalOutline = 0.01f;
        private readonly Dictionary<RarityEnum, int> _rarityWeights = new()
        {
            {RarityEnum.Normal, 970},
            {RarityEnum.Magic, 29 },
            {RarityEnum.Rare, 1}
        };

        public float Width =>
            Value switch
            {
                RarityEnum.Normal => NormalOutline,
                RarityEnum.Magic => NormalOutline,
                RarityEnum.Rare => NormalOutline,
                RarityEnum.Unique => NormalOutline,
                _ => throw new ArgumentOutOfRangeException(nameof(Value), Value, null)
            };

        public Color Color =>
            Value switch
            {
                RarityEnum.Normal => _magic,
                RarityEnum.Magic => _rare,
                RarityEnum.Rare => _magic,
                RarityEnum.Unique => Color.red,
                _ => throw new ArgumentOutOfRangeException(nameof(Value), Value, null)
            };

        public float Multiplier =>
            Value switch
            {
                RarityEnum.Normal => 0,
                RarityEnum.Magic => 1000,
                RarityEnum.Rare => 1500,
                RarityEnum.Unique => 2500,
                _ => throw new ArgumentOutOfRangeException(nameof(Value), Value, null)
            };

        //public Rarity(RarityEnum value) => Value = value;

        public Rarity() => Value = RarityEnum.Normal;

        public RarityEnum GetRandomRarity()
        {
            var sum = GetWeightSum();

            var next = Random.Range(0, sum);

            var limit = 0;
            var rarityTypes = _rarityWeights.Keys.ToList();
            var weights = _rarityWeights.Values.ToList();

            for (var i = 0; i < _rarityWeights.Count; i++)
            {
                var groupingMode = rarityTypes[i];
                limit += weights[i];
                if (next < limit)
                {
                    return groupingMode;
                }
            }
            throw new InvalidOperationException("");
        }

        private int GetWeightSum()
        {
            return _rarityWeights.Sum(keyValuePair => keyValuePair.Value);
        }
    }
}