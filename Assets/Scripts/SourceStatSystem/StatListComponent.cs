using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.SourceStatSystem
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(StatSourcesComponent))]
    [AddComponentMenu("Source Stat System/Stats Handler")]
    public class StatListComponent : MonoBehaviour
        , ISerializationCallbackReceiver
    {
        [field: SerializeField] public List<StatData> Stats { get; private set; } = new List<StatData>();

        [SerializeField] private StatSourcesComponent _statSourcesComponent;

        private void Awake()
        {
            _statSourcesComponent = GetComponent<StatSourcesComponent>();
        }

        private void Start()
        {
            ConstructStats();
        }

        private void OnEnable()
        {
            _statSourcesComponent.valueChangedEvent.AddListener(UpdateStat);
        }

        private void OnDisable()
        {
            _statSourcesComponent.valueChangedEvent.RemoveListener(UpdateStat);
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize() { }
        void ISerializationCallbackReceiver.OnAfterDeserialize() => ConstructStats();

        private void UpdateStat(StatId statId)
        {
            if (_statSourcesComponent == null) return;
            var statSources = _statSourcesComponent.GetStatSources();
            var stat = Stats.Find(s => s.Id == statId);
            var flatStatSources = statSources.Where(s => s.ImpactType == ImpactType.Flat);
            var multiplierStatSources = statSources.Where(s => s.ImpactType == ImpactType.Percentage);
            var flatSum = flatStatSources.Sum(s => s.Value);
            var multiplierSum = multiplierStatSources.Sum(s => s.Value);
            stat.Value = flatSum * multiplierSum;
        }

        private void ConstructStats()
        {
            if (_statSourcesComponent == null) return;
            var completeStatSourcesList = _statSourcesComponent.GetStatSources();
            Stats = completeStatSourcesList.Where(baseStatSource => baseStatSource.StatId != null).
                GroupBy(statSource => statSource.StatId).
                Select(group => new StatData
                {
                    Id = group.Key,
                    Value = group.Where(statSource => statSource.ImpactType == ImpactType.Flat).Sum(statSource => statSource.Value)
                            * (1 + group.Where(statSource => statSource.ImpactType == ImpactType.Percentage).Sum(statSource => statSource.Value) * 0.01f),
                }).ToList();

            foreach (var statData in Stats)
            {
                statData.inspectorValue = $"{statData.Value} - {statData.Id.Value}";
            }
        }
    }
}