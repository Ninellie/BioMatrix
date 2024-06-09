using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SourceStatSystem
{
    [CreateAssetMenu(fileName = "New stat sources pack", menuName = "Source stat system/Stat source pack", order = 51)]
    public class StatSourcePack : ScriptableObject
    {
        [SerializeField] private string _id = "";
        [SerializeField] private StatSourceType _type = StatSourceType.Base;
        [Space]
        [Header("Drop statId here to add new Stat Source")]
        [SerializeField] private StatId _newFlatStatSource;
        [SerializeField] private StatId _newPercentageStatSource;
        [field: Space]
        [field: SerializeField] public List<StatSourceData> StatSources { get; private set; } = new();
        [field: Space]
        [SerializeField] private List<StatData> _statsPreview = new();

        private void OnValidate()
        {
            ValidateStatSource();
            ConstructStats();
        }

        private void ValidateStatSource()
        {
            if (_newFlatStatSource != null)
            {
                StatSources.Add(new StatSourceData(_newFlatStatSource, ImpactType.Flat));
                _newFlatStatSource = null;
            }

            if (_newPercentageStatSource != null)
            {
                StatSources.Add(new StatSourceData(_newPercentageStatSource, ImpactType.Percentage));
                _newPercentageStatSource = null;
            }

            foreach (var baseStatSource in StatSources)
            {
                baseStatSource.Type = _type;
                var sourceId = _id.ToLower();
                var baseStatSourceStatId = "nullStat";
                if (baseStatSource.StatId != null)
                {
                    baseStatSourceStatId = baseStatSource.StatId.Value.ToLower();
                }
                var sourceImpactType = baseStatSource.ImpactType.ToString().ToLower();
                var sourceType = _type.ToString().ToLower();
                baseStatSource.Id = $"{sourceId}_{sourceType}_{baseStatSourceStatId}_{sourceImpactType}_{baseStatSource.Value}";
            }
        }

        private void ConstructStats()
        {
            _statsPreview = StatSources.Where(baseStatSource => baseStatSource.StatId != null).
                GroupBy(statSource => statSource.StatId).
                Select(group => new StatData(group.Key,
                    group.Where(statSource => statSource.ImpactType == ImpactType.Flat).Sum(statSource => statSource.Value)
                        * (1 + group.Where(statSource => statSource.ImpactType == ImpactType.Percentage).Sum(statSource => statSource.Value) 
                        * 0.01f))).
                ToList();
        }
    }
}