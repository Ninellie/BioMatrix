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

        private void UpdateStat(StatId statId, IEnumerable<StatSourceData> statSources)
        {
            var updatedStatValue = 0f;
            
            // Находим такие ассеты переменных, к которым подходит источники
            var statAssets = _stats.FindAll(s => s.id == statId);
            
            // Считаем значение стата
            var sources = statSources.Where(s => s.StatId == statId);
            var sourcesList = sources.ToList();
            if (sourcesList.FirstOrDefault(s=>s.ImpactType == ImpactType.Locker) is { } firstStatLocker)
            {
                updatedStatValue = firstStatLocker.Value;
            }
            else
            {
                updatedStatValue = CalculateStatValue(sourcesList);
            }
            
            // Устанавливаем им обновлённое значение
            SetStatValue(statAssets, updatedStatValue);

#if UNITY_EDITOR
            UpdatePreviewStat(statId, updatedStatValue);
#endif
        }

        private static float CalculateStatValue(IReadOnlyCollection<StatSourceData> sourcesList)
        {
            var flatStatSources = sourcesList.Where(s => s.ImpactType == ImpactType.Flat);
            var multiplierStatSources = sourcesList.Where(s => s.ImpactType == ImpactType.Percentage);
            var flatSum = flatStatSources.Sum(s => s.Value);
            var multiplierSum = multiplierStatSources.Sum(s => s.Value);
            var updatedStatValue = flatSum * (1 + multiplierSum * 0.01f);
            return updatedStatValue;
        }
        
        private static void SetStatValue(IEnumerable<FloatVariable> statAssets, float value)
        {
            foreach (var statVariableAsset in statAssets)
            { 
                statVariableAsset.SetValue(value);
            }
        }

        private void UpdatePreviewStat(StatId id, float updatedValue)
        {
            foreach (var statData in _preview.Where(s => s.Id == id))
            {
                statData.Value = updatedValue;
            }
        }

        private static IEnumerable<StatId> GetUniqueStatIdList(IEnumerable<StatSourceData> statSources)
        {
            var idList = new List<StatId>();
            foreach (var statData in statSources)
            {
                if (!idList.Contains(statData.StatId))
                {
                    idList.Add(statData.StatId);
                }
            }
            return idList;
        }
        
        private void ConstructStats(IEnumerable<StatSourceData> statSources)
        {
            var idList = GetUniqueStatIdList(statSources);
            foreach (var id in idList)
            {
                UpdateStat(id, statSources);
            }
            
            // var groupedById = statSourceList.GroupBy(statSource => statSource.StatId);
            // var select = groupedById.Select(group => 
            //     new StatData(group.Key, group.Where(statSource => statSource.ImpactType == ImpactType.Flat).Sum(statSource => statSource.Value)
            //                             * (1 + group.Where(statSource => statSource.ImpactType == ImpactType.Percentage).Sum(statSource => statSource.Value)
            //                                 * 0.01f)));
//             
//             var selectList = select.ToList();
//             
//             foreach (var statData in statSourceList)
//             {
//                 // Находим такие к которым подходит источник
//                 var stat = _stats.FindAll(s => s.id == statData.StatId);
//
//                 // Устанавливаем им обновлённое значение
//                 foreach (var statVariable in stat)
//                 {
//                     statVariable.SetValue(statData.Value);
//                 }
//             }
//
// #if UNITY_EDITOR
//             _preview = selectList;
// #endif
        }

        private IEnumerable<StatSourceData> GetStatSources()
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