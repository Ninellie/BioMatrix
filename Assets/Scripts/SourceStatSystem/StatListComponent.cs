using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.SourceStatSystem
{
    [DisallowMultipleComponent]
    public class StatListComponent : MonoBehaviour
    {
        [SerializeField] private List<StatData> _stats = new();

        public float GetStatValue(StatId statId)
        {
            return _stats.Find(d => d.Id == statId).Value;
        }

        public float UpdateStat(StatId statId, List<StatSourceData> statSources)
        {
            var stat = _stats.Find(s => s.Id == statId);
            var flatStatSources = statSources.Where(s => s.ImpactType == ImpactType.Flat);
            var multiplierStatSources = statSources.Where(s => s.ImpactType == ImpactType.Percentage);
            var flatSum = flatStatSources.Sum(s => s.Value);
            var multiplierSum = multiplierStatSources.Sum(s => s.Value);
            stat.Value = flatSum * multiplierSum;
            return stat.Value;
        }

        public void ConstructStats(IEnumerable<StatSourceData> statSources)
        {
            _stats = statSources.Where(baseStatSource => baseStatSource.StatId != null).
                GroupBy(statSource => statSource.StatId).
                Select(group => new StatData
                {
                    Id = group.Key,
                    Value = group.Where(statSource => statSource.ImpactType == ImpactType.Flat).Sum(statSource => statSource.Value)
                            * (1 + group.Where(statSource => statSource.ImpactType == ImpactType.Percentage).Sum(statSource => statSource.Value) * 0.01f),
                }).ToList();

            foreach (var statData in _stats)
            {
                statData.inspectorValue = $"{statData.Value} - {statData.Id.Value}";
            }
        }
    }
}