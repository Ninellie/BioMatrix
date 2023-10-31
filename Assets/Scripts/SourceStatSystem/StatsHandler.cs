using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.SourceStatSystem
{
    public enum StatSourceType
    {
        None,
        Base,
        Effect,
    }

    public enum StatValueType
    {
        None,
        Flat,
        Multiplier,
    }

    [Serializable]
    public class StatData
    {
        [field: SerializeField] public string Id { get; set; } = string.Empty;
    }

    [Serializable]
    public class StatSourceData
    {
        [field: SerializeField] public string Id { get; private set; } = string.Empty;
        [field: SerializeField] public StatSourceType Type { get; private set; } = StatSourceType.None;
        [field: SerializeField] public string StatId { get; private set; } = string.Empty;
        [field: SerializeField] public int Value { get; private set; } = 0;
    }

    public class BaseStats : ScriptableObject
    {

    }

    [DisallowMultipleComponent]
    [AddComponentMenu("StatsHandler")]
    public class StatsHandler : MonoBehaviour, ISerializationCallbackReceiver
    {
        [field: SerializeField] public List<StatSourceData> BaseStats { get; private set; } = new List<StatSourceData>();
        [field: SerializeField] public List<StatData> Stats { get; private set; } = new List<StatData>();


        public void OnBeforeSerialize()
        {
            
        }

        public void OnAfterDeserialize()
        {
            Stats.Clear();

            Stats = BaseStats.GroupBy(stat => stat.Id).Select(group => new StatData
                {
                    Id = group.Key + $"{group.Sum(stat => stat.Value)}"
                }).ToList();
        }
    }
}