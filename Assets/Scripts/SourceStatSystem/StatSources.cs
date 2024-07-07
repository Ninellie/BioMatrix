using System.Collections.Generic;
using System.Linq;
using Core.Variables;
using UnityEngine;

namespace SourceStatSystem
{
    [CreateAssetMenu(fileName = "New stat sources", menuName = "Source stat system/Stat sources", order = 51)]
    public class StatSources : ScriptableObject
    {
        [SerializeField] private List<StatSourceData> _statSources = new();
        [SerializeField] private bool _clearOnPlay;
        [SerializeField] private StatSourcePack _baseStatSources;
        [SerializeField] [Tooltip("Stat variable assets")] private List<StatVariable> _stats;
        [Header("Preview")]
        [SerializeField] private List<StatData> _preview;

        private void Awake()
        {
            if (_clearOnPlay) _statSources.Clear();
            var statSources = GetStatSources();
            ConstructStats(statSources);
        }

        private void OnValidate()
        {
            var statSources = GetStatSources();
            ConstructStats(statSources);
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
        
        private void ConstructStats(List<StatSourceData> statSources)
        {
            var uniqueStatIdList = statSources.Select(s => s.StatId).ToHashSet();
            foreach (var id in uniqueStatIdList)
            {
                UpdateStat(id, statSources);
            }
        }

        private void UpdateStat(StatId statId, List<StatSourceData> statSources)
        {
            // Находим такие ассеты переменных, к которым подходит источники
            var statAssets = _stats.FindAll(s => s.id == statId);
            // Считаем значение стата
            var updatedStatValue = StatSourcesBuilder.CalculateStatValue(statId, statSources);
            // Устанавливаем им обновлённое значение
            foreach (var statAsset in statAssets)
            {
                StatSourcesBuilder.SetStatValue(statAsset, updatedStatValue);
            }
#if UNITY_EDITOR
            StatSourcesBuilder.UpdateStatData(statId, updatedStatValue, _preview, statSources);
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