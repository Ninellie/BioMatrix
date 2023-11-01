using System;
using UnityEngine;

namespace Assets.Scripts.SourceStatSystem
{
    public enum StatSourceType
    {
        None,
        Base,
        Effect,
    }

    public enum StatImpactType
    {
        None,
        Flat,
        Percentage
    }

    [Serializable]
    public class StatSourceData
    {
        public StatSourceData(Stat statId, StatImpactType impactType)
        {
            StatId = statId;
            ImpactType = impactType;
            Value = 0;
        }

        public StatSourceData(string id, StatSourceType type, Stat statId, StatImpactType impactType, int value)
        {
            Id = id;
            Type = type;
            StatId = statId;
            ImpactType = impactType;
            Value = value;
        }

        [field: SerializeField] public string Id { get; set; } = string.Empty;
        [field: SerializeField] public StatSourceType Type { get; set; } = StatSourceType.None;
        [field: SerializeField] public Stat StatId { get; set; }
        [field: SerializeField] public StatImpactType ImpactType { get; set; }
        [field: SerializeField] public int Value { get; private set; }
    }
}