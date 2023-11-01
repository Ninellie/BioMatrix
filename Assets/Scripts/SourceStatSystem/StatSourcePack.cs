using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.SourceStatSystem
{
    [CreateAssetMenu(fileName = "New Stat Source Pack", menuName = "Source Stat System/Stat Source Pack", order = 53)]
    public class StatSourcePack : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField] private string _id = "";
        [SerializeField] private StatSourceType _type = StatSourceType.Base;
        [Space]
        [Header("Drop stat here for creating new Stat Source")]
        [SerializeField] private Stat _newFlatStatSource;
        [SerializeField] private Stat _newPercentageStatSource;
        [field: Space]
        [field: SerializeField] public List<StatSourceData> StatSources { get; private set; } = new List<StatSourceData>();
#if UNITY_EDITOR
        void ISerializationCallbackReceiver.OnBeforeSerialize() { }
        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            if (_newFlatStatSource != null)
            {
                StatSources.Add(new StatSourceData(_newFlatStatSource, StatImpactType.Flat));
                _newFlatStatSource = null;
            }

            if (_newPercentageStatSource!= null)
            {
                StatSources.Add(new StatSourceData(_newPercentageStatSource, StatImpactType.Percentage));
                _newPercentageStatSource = null;
            }

            foreach (var baseStatSource in StatSources)
            {
                baseStatSource.Type = _type;
                baseStatSource.Id = $"{_id.ToLower()}_base_{baseStatSource.StatId.Id}_{baseStatSource.ImpactType.ToString().ToLower()}";
            }
        }
    }
#endif
}