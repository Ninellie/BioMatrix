using System;
using UnityEngine;

namespace SourceStatSystem
{
    /// <summary>
    /// Contains data about single stat
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
                _inspectorValue = $"{_value}_{Id.Value}";
            }
        }

        public StatData(StatId id, float value)
        {
            _inspectorValue = $"{value}_{id.Value}";
            Id = id;
            _value = value;
        }
    }
}