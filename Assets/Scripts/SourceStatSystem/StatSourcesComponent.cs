using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.SourceStatSystem
{
    [DisallowMultipleComponent]
    public class StatSourcesComponent : MonoBehaviour
    {
        [field: SerializeField] public List<StatSourceData> StatSources { get; private set; } = new List<StatSourceData>();
        [field: SerializeField] public StatSourcePack BaseStatSources { get; private set; }

        [HideInInspector, NonSerialized] public UnityEvent<StatId> valueChangedEvent = new();

        public List<StatSourceData> GetStatSources()
        {
            var statSourceDataList = new List<StatSourceData>();
            statSourceDataList.AddRange(StatSources);
            if (BaseStatSources == null)
            {
                Debug.LogWarning($"Base Stat Sources is null");
                return statSourceDataList;
            }
            statSourceDataList.AddRange(BaseStatSources.StatSources);
            return statSourceDataList;
        }

        public void AddStatSource(StatSourceData statSourceData)
        {
            StatSources.Add(statSourceData);
            var statId = statSourceData.StatId;
            valueChangedEvent.Invoke(statId);
        }

        public void RemoveStatSource(StatSourceData statSourceData)
        {
            StatSources.Remove(statSourceData);
            var statId = statSourceData.StatId;
            valueChangedEvent.Invoke(statId);
        }
    }
}