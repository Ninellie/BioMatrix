using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.SourceStatSystem
{
    public class StatListsManager : MonoBehaviour
    {
        [field: SerializeField] public UnitStatsHandler UnityStats { get; set; }
        [field: SerializeField] public List<UnitStatsHandler> AbilityStats { get; set; }

        private void AddAbilityStatSources()
        {
            // Для каждой абилки
            foreach (var unitStatsHandler in AbilityStats)
            {
                // сделать лист стат сурсов героя, которые есть
            }
        }


    }

    public class AbilityStatsHandler : MonoBehaviour, ISerializationCallbackReceiver
    {


        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            
        }
    }

    [DisallowMultipleComponent]
    [AddComponentMenu("StatsHandler")]
    public class UnitStatsHandler : MonoBehaviour, ISerializationCallbackReceiver
    {
        [field: SerializeField] public BaseStatSources BaseStatSources { get; private set; }
        [field: SerializeField] public List<StatSourceData> StatSources { get; private set; } = new List<StatSourceData>();
        [field: SerializeField] public List<StatData> Stats { get; private set; } = new List<StatData>();

        void ISerializationCallbackReceiver.OnBeforeSerialize() { }
        void ISerializationCallbackReceiver.OnAfterDeserialize() => ConstructStats();

        private void ConstructStats()
        {
            Stats = StatSources.Where(baseStatSource => baseStatSource.StatId != null).
                GroupBy(statSource => statSource.StatId).
                Select(group => new StatData
                {
                    Id = group.Key.Id,
                    Value = group.Where(stat => stat.ImpactType == StatImpactType.Addition).Sum(stat => stat.Value)
                            * (1 + group.Where(stat => stat.ImpactType == StatImpactType.Percentage).Sum(stat => stat.Value) * 0.01f),
                }).ToList();

            foreach (var statData in Stats)
            {
                statData.inspectorValue = $"{statData.Value} - {statData.Id}";
            }
        }
    }
}