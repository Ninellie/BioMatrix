using System;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.Stats
{
    [Serializable]
    public class StatMod
    {
        [SerializeField]
        private OperationType _type;
        public OperationType Type
        {
            get => _type;
            set => _type = value;
        }

        [SerializeField]
        private float _value;
        public float Value
        {
            get => _value;
            set => _value = value;
        }
    }
}