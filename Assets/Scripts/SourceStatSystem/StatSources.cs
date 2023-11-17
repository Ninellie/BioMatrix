using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Core.Variables;
using UnityEngine;

namespace Assets.Scripts.SourceStatSystem
{
    [CreateAssetMenu(fileName = "New stat sources", menuName = "Source stat system/Stat sources", order = 51)]
    public class StatSources : ScriptableObject
    {
        [SerializeField] private List<StatSourceData> _statSources = new();
        [SerializeField] private StatSourcePack _baseStatSources;
        [SerializeField] private List<StatVariable> _stats;

        private void Awake()
        {
            ConstructStats(GetStatSources());
        }

        private void OnValidate()
        {
            ConstructStats(GetStatSources());
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
            // Считаем значение стата
            var flatStatSources = statSources.Where(s => s.ImpactType == ImpactType.Flat);
            var multiplierStatSources = statSources.Where(s => s.ImpactType == ImpactType.Percentage);
            var flatSum = flatStatSources.Sum(s => s.Value);
            var multiplierSum = multiplierStatSources.Sum(s => s.Value);
            var updatedStatValue = flatSum * (1 + multiplierSum * 0.01f);

            // Находим такие к которым подходит источник
            var stat = _stats.FindAll(s => s.id == statId);

            // Устанавливаем им обновлённое значение
            foreach (var statVariable in stat)
            { 
                statVariable.SetValue(updatedStatValue);
            }
        }

        public void ConstructStats(IEnumerable<StatSourceData> statSources)
        {
            var statDatas = statSources.Where(baseStatSource => baseStatSource.StatId != null).
                GroupBy(statSource => statSource.StatId).
                Select(group => new StatData(group.Key,
                    group.Where(statSource => statSource.ImpactType == ImpactType.Flat).Sum(statSource => statSource.Value)
                    * (1 + group.Where(statSource => statSource.ImpactType == ImpactType.Percentage).Sum(statSource => statSource.Value)
                        * 0.01f))).
                ToList();

            foreach (var statData in statDatas)
            {
                // Находим такие к которым подходит источник
                var stat = _stats.FindAll(s => s.id == statData.Id);

                // Устанавливаем им обновлённое значение
                foreach (var statVariable in stat)
                {
                    statVariable.SetValue(statData.Value);
                }
            }
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