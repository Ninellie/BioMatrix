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
        [field: SerializeField] public string Id { get; set; } = string.Empty;
        [field: SerializeField] public float Value { get; set; } = 0;
    }
}