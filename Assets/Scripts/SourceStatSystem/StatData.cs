using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace SourceStatSystem
{
    /// <summary>
    /// Contains data about single stat and used only for unity inspector preview 
    /// </summary>
    [Serializable]
    public class StatData
    {
        [FormerlySerializedAs("_inspectorValue")]
        [HideInInspector]
        [SerializeField] private string inspectorValue;
        [field: SerializeField] public StatId Id { get; private set; }
        [SerializeField] private float value = 0;
        [field: SerializeField] public List<StatSourceData> Sources { get; set; }

        public float Value
        {
            get => value;
            set
            {
                this.value = value;
                inspectorValue = $"{this.value}_{Id.Value}";
            }
        }

        public StatData(StatId id, float value, List<StatSourceData> sources)
        {
            this.Sources = sources;
            inspectorValue = $"{value}_{id.Value}";
            Id = id;
            this.value = value;
        }
    }
}