using System;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.Stats
{
    [Serializable]
    public class StatMod
    {
        [SerializeField] private OperationType _type;
        [SerializeField] private float _value;
        public OperationType Type
        {
            get => _type;
            set => _type = value;
        }
        public float Value
        {
            get => _value;
            set => _value = value;
        }
    }
}