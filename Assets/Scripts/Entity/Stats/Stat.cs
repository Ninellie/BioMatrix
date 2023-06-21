using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Entity.Stats
{
    public class Stat
    {
        public float Value => GetActualValue();
        public event Action ValueChangedEvent;
        private float BaseValue { get; }
        private bool IsModifiable { get; }
        private readonly float _baseAddedValue = 0;
        private float AddedValue => GetAddedValue();
        private readonly float _baseMultiplierValue = 100;
        private float MultiplierValue => GetMultiplierValue();
        private readonly List<StatModifier> _modifiers = new();
        private const float MultiplierDivisor = 100;
        //private readonly GameTimeScheduler _gameTimeScheduler;
        //Without multiplierValue (with multiplierValue = 1)
        //public Stat(float baseValue, bool isModifiable) : this(baseValue, isModifiable, 1)
        //{
        //}
        //public Stat(float baseValue, List<StatModifier> modifiers) : this(baseValue, 1, modifiers)
        //{
        //}
        //Without addedValue (with addedValue = 0)
        //public Stat(float baseValue, float baseMultiplierValue, List<StatModifier> modifiers) : this(baseValue, baseMultiplierValue, 0, modifiers)
        //{
        //}
        //public Stat(float baseValue, bool isModifiable, float baseMultiplierValue) : this(baseValue, isModifiable, baseMultiplierValue, 0)
        //{
        //}
        //Base constructors
        //public Stat(float baseValue, float baseMultiplierValue, float baseAddedValue, List<StatModifier> modifiers) : this(baseValue, true, baseMultiplierValue, baseAddedValue)
        //{
        //    _modifiers = modifiers;
        //}
        public Stat() : this(0, true)
        {
        }
        public Stat(float baseValue) : this(baseValue, true)
        {
        }
        public Stat(float baseValue, bool isModifiable)
        {
            BaseValue = baseValue;
            IsModifiable = isModifiable;
        }
        public void AddModifier(StatModifier modifier)
        {
            Debug.Log($"Try to add modifier {modifier.Value} {modifier.Type}");
            var oldValue = Value;
            _modifiers.Add(modifier);
            TryInvokeOnValueChangedEvent(oldValue);
            Debug.Log($"Added mod {modifier.Type} : {modifier.Value}. Is mod temporary?: {modifier.IsTemporary}.");
            Debug.Log($"New stat value: {Value}. Old value: {oldValue}.");
            //if (modifier.IsTemporary)
            //{
            //    Debug.Log($"Scheduled to remove modifier {modifier.Type} : {modifier.Value}. Will be removed after {modifier.Duration} secs.");
            //    _gameTimeScheduler.Schedule(() => RemoveModifier(modifier), modifier.Duration);
            //}
        }

        public bool RemoveModifier(StatModifier modifier)
        {
            Debug.LogWarning($"Try to remove {modifier.Value} {modifier.Type}");
            if (!_modifiers.Contains(modifier))
                return false;
            var oldValue = Value;
            _modifiers.Remove(modifier);
            TryInvokeOnValueChangedEvent(oldValue);
            Debug.Log($"Removed mod {modifier.Type} : {modifier.Value}. Is mod temporary?: {modifier.IsTemporary}.");
            Debug.Log($"New stat value: {Value}. Old value: {oldValue}.");
            return true;
        }

        public bool IsModifierListEmpty()
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
            return _baseAddedValue + _modifiers
                .Where(modifier => modifier.Type == OperationType.Addition)
                .Sum(modifier => modifier.Value);
        }
        private float GetMultiplierValue()
        {
            return _baseMultiplierValue + _modifiers
                .Where(modifier => modifier.Type == OperationType.Multiplication)
                .Sum(modifier => modifier.Value);
        }
        private float GetActualValue()
        {
            return IsModifiable == false
                ? (BaseValue + _baseAddedValue) * (_baseMultiplierValue / MultiplierDivisor)
                : (BaseValue + AddedValue) * (MultiplierValue / MultiplierDivisor);
        }
        private void TryInvokeOnValueChangedEvent(float oldValue)
        {
            if (Value.Equals(oldValue)) return;
            ValueChangedEvent?.Invoke();
        }
    }
}