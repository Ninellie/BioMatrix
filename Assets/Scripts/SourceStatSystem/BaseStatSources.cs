using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.SourceStatSystem
{


    [CreateAssetMenu(fileName = "New Base Stats", menuName = "Source Stat System/Base Stats", order = 52)]
    public class BaseStatSources : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField] private string _id;
        [Space]
        [Header("Drop stat here for creating new Base Stat Source")]
        [SerializeField] private Stat _newAddBaseStat;
        [SerializeField] private Stat _newPercentageBaseStat;
        [field: Space]
        [field: SerializeField] public List<StatSourceData> StatSources { get; private set; } = new List<StatSourceData>();

        void ISerializationCallbackReceiver.OnBeforeSerialize() { }
        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            if (_newAddBaseStat != null)
            {
                StatSources.Add(new StatSourceData($"", StatSourceType.Base, _newAddBaseStat, StatImpactType.Flat, 0));
                _newAddBaseStat = null;
            }

            if (_newPercentageBaseStat!= null)
            {
                StatSources.Add(new StatSourceData($"", StatSourceType.Base, _newPercentageBaseStat, StatImpactType.Percentage, 0));
                _newPercentageBaseStat = null;
            }

            foreach (var baseStatSource in StatSources)
            {
                baseStatSource.Type = StatSourceType.Base;
                baseStatSource.Id = $"{_id.ToLower()}_base_{baseStatSource.StatId.Id}_{baseStatSource.ImpactType.ToString().ToLower()}";
            }
        }
    }
}