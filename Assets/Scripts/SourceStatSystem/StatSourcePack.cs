using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.SourceStatSystem
{
    [CreateAssetMenu(fileName = "New Stat Source Pack", menuName = "Source Stat System/Stat Source Pack", order = 52)]
    public class StatSourcePack : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField] private string _id = "";
        [SerializeField] private StatSourceType _type = StatSourceType.Base;
        [Space]
        [Header("Drop stat here for creating new Stat Source")]
        [SerializeField] private StatId _newFlatStatIdSource;
        [SerializeField] private StatId _newPercentageStatIdSource;
        [field: Space]
        [field: SerializeField] public List<StatSourceData> StatSources { get; private set; } = new List<StatSourceData>();
#if UNITY_EDITOR
        void ISerializationCallbackReceiver.OnBeforeSerialize() { }
        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            if (_newFlatStatIdSource != null)
            {
                StatSources.Add(new StatSourceData(_newFlatStatIdSource, ImpactType.Flat));
                _newFlatStatIdSource = null;
            }

            if (_newPercentageStatIdSource!= null)
            {
                StatSources.Add(new StatSourceData(_newPercentageStatIdSource, ImpactType.Percentage));
                _newPercentageStatIdSource = null;
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
#endif
    }
}