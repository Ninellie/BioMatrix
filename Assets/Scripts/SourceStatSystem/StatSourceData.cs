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
        public StatSourceData(StatId statId, ImpactType impactType)
        {
            StatId = statId;
            ImpactType = impactType;
            Value = 0;
        }

        [field: SerializeField] public string Id { get; set; } = string.Empty;
        public StatSourceType Type { get; set; } = StatSourceType.Base;
        [field: SerializeField] public StatId StatId { get; set; }
        [field: SerializeField] public ImpactType ImpactType { get; set; }
        [field: SerializeField] public int Value { get; private set; }
    }
}