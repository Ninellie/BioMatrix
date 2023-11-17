using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.SourceStatSystem
{
    public class StatSourcesComponent : MonoBehaviour
    {
        [SerializeField] private List<StatSourceData> _statSources = new();
        [SerializeField] private StatSourcePack _baseStatSources;

        public List<StatSourceData> GetStatSources()
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

        public void AddStatSource(StatSourceData statSourceData)
        {
            _statSources.Add(statSourceData);
        }

        public void RemoveStatSource(StatSourceData statSourceData)
        {
            _statSources.Remove(statSourceData);
        }
    }
}