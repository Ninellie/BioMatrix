using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.SourceStatSystem
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Source Stat System/Stats Handler")]
    public class StatsHandler : MonoBehaviour, ISerializationCallbackReceiver
    {
        [field: SerializeField] public List<StatSourceData> StatSources { get; private set; } = new List<StatSourceData>();
        [field: SerializeField] public List<StatData> Stats { get; private set; } = new List<StatData>();

        [field: SerializeField] public StatSourcePack BaseStatSources { get; set; }

#if UNITY_EDITOR
        void ISerializationCallbackReceiver.OnBeforeSerialize() { }
        void ISerializationCallbackReceiver.OnAfterDeserialize() => ConstructStats();
#endif
        private void ConstructStats()
        {
            var completeStatSourcesList = new List<StatSourceData>();
            completeStatSourcesList.AddRange(BaseStatSources.StatSources);
            completeStatSourcesList.AddRange(StatSources);

            Stats = completeStatSourcesList.Where(baseStatSource => baseStatSource.StatId != null).
                GroupBy(statSource => statSource.StatId).
                Select(group => new StatData
                {
                    Id = group.Key.Id,
                    Value = group.Where(stat => stat.SourceImpactType == ImpactType.Flat).Sum(stat => stat.Value)
                            * (1 + group.Where(stat => stat.SourceImpactType == ImpactType.Percentage).Sum(stat => stat.Value) * 0.01f),
                }).ToList();

            foreach (var statData in Stats)
            {
                statData.inspectorValue = $"{statData.Value} - {statData.Id}";
            }
        }

    }
}