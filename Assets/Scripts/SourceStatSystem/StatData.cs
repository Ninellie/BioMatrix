using System;
using UnityEngine;

namespace Assets.Scripts.SourceStatSystem
{
    /// <summary>
    /// Dynamic class for inner use
    /// </summary>
    [Serializable]
    public class StatData
    {
        [HideInInspector]
        [field: SerializeField] private string _inspectorValue;
        [field: SerializeField] public StatId Id { get; private set; }

        [SerializeField] private float _value = 0;

        public float Value
        {
            get => _value;
            set
            {
                _value = value;
                _inspectorValue = $"{Id.Value} - {_value}";
            }
        }

        public StatData(StatId id, float value)
        {
            _inspectorValue = $"{id.Value} - {value}";
            Id = id;
            _value = value;
        }
    }
}