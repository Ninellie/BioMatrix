using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.CustomAttributes;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.Stats
{
    [Serializable]
    public class Stat
    {
        [HideInInspector]
        public string _strName;

        public event Action ValueChangedEvent;

        [ReadOnly]
        [SerializeField]
        private StatName _name;
        public StatName Name => _name;

        [ReadOnly]
        [SerializeField]
        private float _baseValue;

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
        private List<StatMod> _modifiers = new();

        public void AddModifier(StatMod modifier)
        {
            _modifiers.Add(modifier);
            UpdateActualValue();
        }

        public bool RemoveModifier(StatMod modifier)
        {
            if (!_modifiers.Contains(modifier))
                return false;
            _modifiers.Remove(modifier);
            UpdateActualValue();
            return true;
        }

        public void ClearModifiersList()
        {
            if (_modifiers.Count == 0) return;
            _modifiers.Clear();
            UpdateActualValue();
        }

        public void SetBaseValue(float value)
        {
            _baseValue = value;
            UpdateActualValue();
        }

        public void SetName(StatName name) => _name = name;

        public void SetSettings(StatSettings settings)
        {
            _settings = settings;
            UpdateActualValue();
        }

        public void SetSettings()
        {
            _settings = (StatSettings)ScriptableObject.CreateInstance(nameof(StatSettings));
            UpdateActualValue();
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
            var oldValue = _value;
            UpdateAddedValue();
            UpdateMultiplierValue();

            _value = _settings.IsModifiable == false
                ? (_baseValue + _settings.BaseAddedValue) * (_settings.BaseMultiplierValue / _settings.MultiplierDivisor)
                : (_baseValue + _addedValue) * (_multiplierValue / _settings.MultiplierDivisor);

            TryInvokeOnValueChangedEvent(oldValue);
        }

        private void TryInvokeOnValueChangedEvent(float oldValue)
        {
            if (Value.Equals(oldValue)) return;
            ValueChangedEvent?.Invoke();
        }
    }
}