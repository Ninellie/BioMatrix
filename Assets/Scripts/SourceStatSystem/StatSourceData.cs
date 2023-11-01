using System;
using UnityEngine;

namespace Assets.Scripts.SourceStatSystem
{
    [Serializable]
    public class StatSourceData
    {
        public StatSourceData(Stat statId, ImpactType sourceImpactType)
        {
            StatId = statId;
            SourceImpactType = sourceImpactType;
            Value = 0;
        }

        public StatSourceData(string id, StatSourceType type, Stat statId, ImpactType sourceImpactType, int value)
        {
            Id = id;
            Type = type;
            StatId = statId;
            SourceImpactType = sourceImpactType;
            Value = value;
        }

        [field: SerializeField] public string Id { get; set; } = string.Empty;
        [field: SerializeField] public StatSourceType Type { get; set; } = StatSourceType.Base;
        [field: SerializeField] public Stat StatId { get; set; }
        [field: SerializeField] public ImpactType SourceImpactType { get; set; }
        [field: SerializeField] public int Value { get; private set; }
    }
}