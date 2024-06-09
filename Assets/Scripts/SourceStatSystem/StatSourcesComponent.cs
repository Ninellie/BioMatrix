using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SourceStatSystem
{
    /// <summary>
    /// Stat sources component for identical entities that require different stat values.
    /// This class don't support scriptable object stat variables. It uses StatData list instead.
    /// </summary>
    public class StatSourcesComponent : MonoBehaviour
    {
        [SerializeField] private List<StatSourceData> _statSources = new();
        [SerializeField] private bool _clearOnPlay;
        [SerializeField] private StatSourcePack _baseStatSources;
        [SerializeField] private List<StatData> _stats;

        private void Awake()
        {
            if (_clearOnPlay)
            {
                _statSources.Clear();
            }
            ConstructStats(GetStatSources());
        }

        private void OnValidate()
        {
            ConstructStats(GetStatSources());
        }

        public float GetStatValue(StatId statId)
        {
            return _stats.Find(s => s.Id == statId).Value;
        }

        public StatData GetStat(StatId statId)
        {
            return _stats.Find(s => s.Id== statId);
        }

        public void AddStatSource(StatSourceData statSourceData)
        {
            _statSources.Add(statSourceData);
            UpdateStat(statSourceData.StatId, GetStatSources());
        }

        public void RemoveStatSource(StatSourceData statSourceData)
        {
            _statSources.Remove(statSourceData);
            UpdateStat(statSourceData.StatId, GetStatSources());
        }

        public void UpdateStat(StatId statId, List<StatSourceData> statSources)
        {
            // Calculate stat value
            var sources = statSources.Where(s => s.StatId == statId);
            var flatStatSources = sources.Where(s => s.ImpactType == ImpactType.Flat);
            var multiplierStatSources = sources.Where(s => s.ImpactType == ImpactType.Percentage);
            var flatSum = flatStatSources.Sum(s => s.Value);
            var multiplierSum = multiplierStatSources.Sum(s => s.Value);
            var updatedStatValue = flatSum * (1 + multiplierSum * 0.01f);

            // Находим такие к которым подходит источник
            var stats = _stats.FindAll(s => s.Id == statId);

            // Устанавливаем им обновлённое значение
            foreach (var stat in stats)
            {
                stat.Value = updatedStatValue;
            }

#if UNITY_EDITOR
            UpdatePreviewStat(statId, updatedStatValue);
#endif
        }

        private void UpdatePreviewStat(StatId id, float updatedValue)
        {
            foreach (var statData in _stats.Where(s => s.Id.Value == id.Value))
            {
                statData.Value = updatedValue;
            }
        }

        private void ConstructStats(IEnumerable<StatSourceData> statSources)
        {
            var statData = statSources.Where(baseStatSource => baseStatSource.StatId != null).
                GroupBy(statSource => statSource.StatId).
                Select(group => new StatData(group.Key,
                    group.Where(statSource => statSource.ImpactType == ImpactType.Flat).Sum(statSource => statSource.Value)
                    * (1 + group.Where(statSource => statSource.ImpactType == ImpactType.Percentage).Sum(statSource => statSource.Value)
                        * 0.01f))).
                ToList();

            foreach (var data in statData)
            {
                // Находим такие к которым подходит источник
                var stats = _stats.FindAll(s => s.Id == data.Id);

                // Устанавливаем им обновлённое значение
                foreach (var stat in stats)
                {
                    stat.Value = data.Value;
                }
            }

#if UNITY_EDITOR
            _stats = statData;
#endif
        }

        private List<StatSourceData> GetStatSources()
        {
            var statSourceDataList = new List<StatSourceData>();
            statSourceDataList.AddRange(_statSources);
            if (_baseStatSources == null)
            {
                Debug.LogWarning($"Base Stat Sources is null");
                return statSourceDataList;
            }
            statSourceDataList.AddRange(_baseStatSources.StatSources);
            return statSourceDataList;
        }
    }
}