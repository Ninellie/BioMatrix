using UnityEngine;

namespace Assets.Scripts.EntityComponents.Stats
{
    [CreateAssetMenu]
    public class StatSettings : ScriptableObject
    {
        [SerializeField]
        private bool _isModifiable;
        public bool IsModifiable => _isModifiable; // true

        [SerializeField]
        private float _multiplierDivisor; // 100
        public float MultiplierDivisor => _multiplierDivisor;

        [SerializeField]
        private float _baseAddedValue; // 0
        public float BaseAddedValue => _baseAddedValue;

        [SerializeField]
        private float _baseMultiplierValue; // 100
        public float BaseMultiplierValue => _baseMultiplierValue;
    }
}