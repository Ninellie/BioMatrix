using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SourceStatSystem
{
    [CreateAssetMenu(fileName = "New stat sources pack", menuName = "Source stat system/Stat source pack", order = 51)]
    public class StatSourcePack : ScriptableObject
    {
        [SerializeField] private string _id = "";
        [SerializeField] private StatSourceType type = StatSourceType.Base;
        [Space]
        [SerializeField] private bool enableValidation;
        [Space]
        [SerializeField] private int value;
        //[SerializeField] private StatSourceType impactType; 
        [Header("Drop statId here to add new Stat Source")]
        [SerializeField] private StatId newFlatStatSource;
        [SerializeField] private StatId newPercentageStatSource;
        [SerializeField] private StatId newLockerStatSource;
        [field: Space]
        [field: SerializeField] public List<StatSourceData> StatSources { get; private set; } = new();
        [field: Space]
        [SerializeField] private List<StatData> preview = new();

        private void OnValidate()
        {
            if (enableValidation)
            {
                ValidateAddedStatSource();
                ValidateStatSources();
                ConstructStatsPreview();
            }
        }

        private void ValidateStatSources()
        {
            foreach (var statSource in StatSources)
            {
                statSource.PackId = _id;
                statSource.Type = type;
                StatSourcesBuilder.SetStatSourceInspectorId(statSource);
            }
        }
        
        private void ValidateAddedStatSource()
        {
            if (newFlatStatSource != null)
            {
                StatSources.Add(new StatSourceData(newFlatStatSource, ImpactType.Flat, type, _id, value));
                newFlatStatSource = null;
            }
            if (newPercentageStatSource != null)
            {
                StatSources.Add(new StatSourceData(newPercentageStatSource, ImpactType.Percentage, type, _id, value));
                newPercentageStatSource = null;
            }
            if (newLockerStatSource != null)
            {
                StatSources.Add(new StatSourceData(newLockerStatSource, ImpactType.Locker, type, _id, value));
                newLockerStatSource = null;
            }
        }

        private void ConstructStatsPreview()
        {
            preview = new List<StatData>();
            var uniqueStatIdList = StatSources.Select(s => s.StatId).ToHashSet();
            foreach (var statId in uniqueStatIdList)
            {
                var statValue = StatSourcesBuilder.CalculateStatValue(statId, StatSources);
                var statIdSources = StatSources.Where(s => s.StatId == statId).ToList();
                preview.Add(new StatData(statId, statValue, statIdSources));
            }
        }
    }
}