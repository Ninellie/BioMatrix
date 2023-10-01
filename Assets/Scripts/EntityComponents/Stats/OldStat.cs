using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.Stats
{
    public class OldStat
    {
        public float Value => GetActualValue();
        private float BaseValue { get; }
        private bool IsModifiable { get; }
        private const float BaseAddedValue = 0;
        private float AddedValue => GetAddedValue();
        private const float BaseMultiplierValue = 100;
        private float MultiplierValue => GetMultiplierValue();
        private readonly List<StatModifier> _modifiers = new();
        public event Action ValueChangedEvent;
        private const float MultiplierDivisor = 100;

        public void AddModifier(StatModifier modifier)
        {
            Debug.Log($"Try to add modifier {modifier.Value} {modifier.Type}");
            var oldValue = Value;
            _modifiers.Add(modifier);
            TryInvokeOnValueChangedEvent(oldValue);
            Debug.Log($"Added mod {modifier.Type} : {modifier.Value}. Is mod temporary?: {modifier.IsTemporary}.");
            Debug.Log($"New stat value: {Value}. Old value: {oldValue}.");
        }

        public void RemoveModifier(StatModifier modifier)
        {
            Debug.LogWarning($"Try to remove {modifier.Value} {modifier.Type}");
            if (!_modifiers.Contains(modifier))
                return;
            var oldValue = Value;
            _modifiers.Remove(modifier);
            TryInvokeOnValueChangedEvent(oldValue);
            Debug.Log($"Removed mod {modifier.Type} : {modifier.Value}. Is mod temporary?: {modifier.IsTemporary}.");
            Debug.Log($"New stat value: {Value}. Old value: {oldValue}.");
        }

        private bool IsModifierListEmpty()
        {
            return _modifiers.Count == 0;
        }

        public void ClearModifiersList()
        {
            if (IsModifierListEmpty()) return;
            var oldValue = Value;
            _modifiers.Clear();
            TryInvokeOnValueChangedEvent(oldValue);
        }

        private float GetAddedValue()
        {
            return BaseAddedValue + _modifiers
                .Where(modifier => modifier.Type == OperationType.Addition)
                .Sum(modifier => modifier.Value);
        }

        private float GetMultiplierValue()
        {
            return BaseMultiplierValue + _modifiers
                .Where(modifier => modifier.Type == OperationType.Multiplication)
                .Sum(modifier => modifier.Value);
        }

        private float GetActualValue()
        {
            return IsModifiable == false
                ? (BaseValue + BaseAddedValue) * (BaseMultiplierValue / MultiplierDivisor)
                : (BaseValue + AddedValue) * (MultiplierValue / MultiplierDivisor);
        }

        private void TryInvokeOnValueChangedEvent(float oldValue)
        {
            if (Value.Equals(oldValue)) return;
            ValueChangedEvent?.Invoke();
        }

        public OldStat() : this(0, true)
        {
        }

        public OldStat(float baseValue) : this(baseValue, true)
        {
        }

        public OldStat(float baseValue, bool isModifiable)
        {
            BaseValue = baseValue;
            IsModifiable = isModifiable;
        }
    }
}