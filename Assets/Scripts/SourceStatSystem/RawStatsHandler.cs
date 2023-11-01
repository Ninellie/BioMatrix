using System;
using System.Collections.Generic;
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
        Addition,
        Percentage
    }

    [Serializable]
    public class StatSourceData
    {
        [field: SerializeField] public string Id { get; private set; } = string.Empty;
        [field: SerializeField] public StatSourceType Type { get; set; } = StatSourceType.None;
        [field: SerializeField] public Stat StatId { get; set; }
        [field: SerializeField] public StatImpactType ImpactType { get; set; } = StatImpactType.None;
        [field: SerializeField] public int Value { get; private set; } = 0;
    }

    public class BaseStatSources : ScriptableObject, ISerializationCallbackReceiver
    {
        [field: SerializeField] public List<StatSourceData> StatSources { get; private set; } = new List<StatSourceData>();

        void ISerializationCallbackReceiver.OnBeforeSerialize() { }
        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            foreach (var baseStatSource in StatSources)
            {
                baseStatSource.Type = StatSourceType.Base;
            }
        }
    }
}