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
        [field: SerializeField]
        public string inspectorValue;
        [field: SerializeField] public StatId Id { get; set; }
        [field: SerializeField] public float Value { get; set; } = 0;
    }
}