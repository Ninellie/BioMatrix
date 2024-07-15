using System;
using UnityEngine;

namespace SourceStatSystem
{
    /// <summary>
    /// Contains data about single stat source
    /// </summary>
    [Serializable]
    public class StatSourceData
    {
        public StatSourceData(StatId statId, ImpactType impactType, StatSourceType sourceType = StatSourceType.Effect, string packId = "", int value = 0)
        {
            StatId = statId;
            ImpactType = impactType;
            Value = value;
            Type = sourceType;
            PackId = packId;
            StatSourcesBuilder.SetStatSourceInspectorId(this);
        }

        [field: SerializeField] public string Id { get; set; } = string.Empty;
        [field: SerializeField] public string PackId { get; set; }
        [field: SerializeField] public StatSourceType Type { get; set; }
        [field: SerializeField] public StatId StatId { get; set; }
        [field: SerializeField] public ImpactType ImpactType { get; set; }
        [field: SerializeField] public int Value { get; private set; }
    }
}