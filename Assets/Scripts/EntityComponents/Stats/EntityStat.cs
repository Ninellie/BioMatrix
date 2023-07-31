using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.Stats
{
    [Serializable]
    public class EntityStat
    {
        public event Action ValueChangedEvent;

        [SerializeField]
        private string _name;
        public string Name => _name;

        [SerializeField]
        private float _baseValue;

        [SerializeField]
        private StatSettings _settings;

        [ReadOnly]
        [SerializeField]
        private float _value;
        public float Value => _value;

        [ReadOnly]
        [SerializeField]
        private float _addedValue;

        [ReadOnly]
        [SerializeField]
        private float _multiplierValue;

        [SerializeField]
        private List<StatMod> _modifiers;

        public void AddModifier(StatMod modifier)
        {
            var oldValue = _value;
            _modifiers.Add(modifier);
            UpdateActualValue();
            TryInvokeOnValueChangedEvent(oldValue);
        }

        public bool RemoveModifier(StatMod modifier)
        {
            if (!_modifiers.Contains(modifier))
                return false;
            var oldValue = _value;
            _modifiers.Remove(modifier);
            UpdateActualValue();
            TryInvokeOnValueChangedEvent(oldValue);
            return true;
        }

        public void ClearModifiersList()
        {
            if (_modifiers.Count == 0) return;
            var oldValue = Value;
            _modifiers.Clear();
            UpdateActualValue();
            TryInvokeOnValueChangedEvent(oldValue);
        }

        private void UpdateAddedValue()
        {
            _addedValue = _settings.BaseAddedValue + _modifiers
                .Where(modifier => modifier.Type == OperationType.Addition)
                .Sum(modifier => modifier.Value);
        }

        private void UpdateMultiplierValue()
        {
            _multiplierValue = _settings.BaseMultiplierValue + _modifiers
                .Where(modifier => modifier.Type == OperationType.Multiplication)
                .Sum(modifier => modifier.Value);
        }

        private void UpdateActualValue()
        {
            UpdateAddedValue();
            UpdateMultiplierValue();

            _value = _settings.IsModifiable == false
                ? (_baseValue + _settings.BaseAddedValue) * (_settings.BaseMultiplierValue / _settings.MultiplierDivisor)
                : (_baseValue + _addedValue) * (_multiplierValue / _settings.MultiplierDivisor);
        }

        private void TryInvokeOnValueChangedEvent(float oldValue)
        {
            if (Value.Equals(oldValue)) return;
            ValueChangedEvent?.Invoke();
        }
    }
}