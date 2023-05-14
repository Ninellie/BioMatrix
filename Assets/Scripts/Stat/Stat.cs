using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Stat
{
    public float Value => GetActualValue();
    public event Action ValueChangedEvent;
    private float BaseValue { get; }
    private bool IsModifiable { get; }
    private float BaseAddedValue { get; }
    private float AddedValue => GetAddedValue();
    private float BaseMultiplierValue { get; }
    private float MultiplierValue => GetMultiplierValue();
    private readonly List<StatModifier> _modifiers = new List<StatModifier>();
    private const float MultiplierDivisor = 100;
    private readonly GameTimeScheduler _gameTimeScheduler;
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
    public Stat(GameTimeScheduler gameTimeScheduler, float baseValue) : this(gameTimeScheduler, baseValue, true, 100, 0)
    {
    }
    public Stat(GameTimeScheduler gameTimeScheduler, float baseValue, bool isModifiable, float baseMultiplierValue, float baseAddedValue)
    {
        _gameTimeScheduler = gameTimeScheduler;
        BaseValue = baseValue;
        IsModifiable = isModifiable;
        BaseMultiplierValue = baseMultiplierValue;
        BaseAddedValue = baseAddedValue;
    }
    public void AddModifier(StatModifier modifier)
    {
        
        var oldValue = Value;
        _modifiers.Add(modifier);
        OnValueChanged(oldValue);

        Debug.Log($"Added mod {modifier.Type} : {modifier.Value}. Is mod temporary?: {modifier.IsTemporary}.");
        Debug.Log($"New stat value: {Value}. Old value: {oldValue}.");

        if (modifier.IsTemporary)
        {
            Debug.Log($"Scheduled to remove modifier {modifier.Type} : {modifier.Value}. Will be removed after {modifier.Duration} secs.");
            
            _gameTimeScheduler.Schedule(() => RemoveModifier(modifier), modifier.Duration);
        }
    }
    public bool RemoveModifier(StatModifier modifier)
    {
        Debug.LogWarning($"Try to remove {modifier.Value} {modifier.Type}");
        if (!_modifiers.Contains(modifier))
            return false;
        var oldValue = Value;
        _modifiers.Remove(modifier);
        OnValueChanged(oldValue);
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
        OnValueChanged(oldValue);
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
    private void OnValueChanged(float oldValue)
    {
        if (Value.Equals(oldValue)) return;
        ValueChangedEvent?.Invoke();
    }
}