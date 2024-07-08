using System;
using System.Collections.Generic;
using System.Linq;
using Core.Variables;
using UnityEngine;

namespace SourceStatSystem
{
    public static class StatSourcesBuilder
    {
        private const float BaseMultiplierCoefficientValue = 1;
        private const float BaseFlatValue = 0;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="statSources">The collection can contain any sources</param>
        /// <param name="statId">Stat ID according to which the value from the collection will be obtained</param>
        /// <returns>Calculated stat value</returns>
        public static float CalculateStatValue(StatId statId, IEnumerable<StatSourceData> statSources)
        {
            var sources = statSources.Where(s => s.StatId == statId).ToList();
            if (sources.FirstOrDefault(s=>s.ImpactType == ImpactType.Locker) is { } firstStatLocker)
            {
                return firstStatLocker.Value;
            }
            var flatStatSources = sources.Where(s => s.ImpactType == ImpactType.Flat);
            var multiplierStatSources = sources.Where(s => s.ImpactType == ImpactType.Percentage);
            var flatSum = flatStatSources.Sum(s => s.Value) + BaseFlatValue;
            float addedPercentageMultiplierSum = multiplierStatSources.Sum(s => s.Value);
            var multiplierCoefficient = BaseMultiplierCoefficientValue + PercentageToCoefficient(addedPercentageMultiplierSum);
            var updatedStatValue = flatSum * multiplierCoefficient;
            return updatedStatValue;
        }

        public static void SetStatValue(FloatVariable statAsset, float value)
        {
            if (statAsset.GetType() == typeof(StatVariable))
            {
                var stat = statAsset as StatVariable;
                if (stat != null) stat.SetValue(value);
            }
            statAsset.SetValue(value);
        }

        public static void UpdateStatData(StatId id, float value, List<StatData> statData, List<StatSourceData> sources)
        {
            var selectedStatData = statData.FindAll(s => s.Id == id);
            if (selectedStatData.Count == 0)
            {
                statData.Add(new StatData(id, value, sources));
                return;
            }
            var idSources = sources.Where(s => s.StatId == id).ToList();
            foreach (var data in selectedStatData)
            {
                data.Value = value;
                data.Sources = idSources;
            }
        }
        
        public static void SetStatSourceInspectorId(StatSourceData statSource)
        {
            var sourceId = statSource.PackId.ToLower();
            var statId = "nullStat";
            if (statSource.StatId != null)
            {
                statId = statSource.StatId.Value.ToLower();
            }
            var sourceType = statSource.Type.ToString().ToLower();
            switch (statSource.ImpactType)
            {
                case ImpactType.None:
                    statSource.Id = $"{sourceId}_{sourceType}_{statId}_has_no_impact_{statSource.Value}";
                    break;
                case ImpactType.Flat:

                    if (Mathf.Sign(statSource.Value).Equals(1))
                    { 
                        statSource.Id = $"{sourceId}_{sourceType}_{statId}_+{statSource.Value}";
                        break;
                    }
                    statSource.Id = $"{sourceId}_{sourceType}_{statId}_{statSource.Value}";
                    break;
                case ImpactType.Percentage:

                    if (Mathf.Sign(statSource.Value).Equals(1))
                    { 
                        statSource.Id = $"{sourceId}_{sourceType}_{statId}_+{statSource.Value}%";
                        break;
                    }
                    statSource.Id = $"{sourceId}_{sourceType}_{statId}_{statSource.Value}%";
                    break;
                case ImpactType.Locker:
                    statSource.Id = $"{sourceId}_{sourceType}_{statId}_lock_at_{statSource.Value}";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
        }

        private static float PercentageToCoefficient(float percentageValue)
        {
            return percentageValue / 100;
        }
    }
}