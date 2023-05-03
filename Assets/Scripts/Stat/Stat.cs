using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Stat
{
    public float Value => GetActualValue();
    public Action onValueChanged;
    private float BaseValue { get; }
    private bool IsModifiable { get; }
    private float BaseAddedValue { get; }
    private float AddedValue => GetAddedValue();
    private float BaseMultiplierValue { get; }
    private float MultiplierValue => GetMultiplierValue();
    private readonly List<StatModifier> _modifiers = new List<StatModifier>();
    private const float MultiplierDivisor = 100;
    private GameTimeScheduler _gameTimeScheduler;
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
    public Stat(float baseValue) : this(baseValue, true, 100, 0)
    {
    }
    public Stat(float baseValue, bool isModifiable, float baseMultiplierValue, float baseAddedValue)
    {
        this.BaseValue = baseValue;
        this.IsModifiable = isModifiable;
        BaseMultiplierValue = baseMultiplierValue;
        this.BaseAddedValue = baseAddedValue;
    }
    public void AddModifier(StatModifier modifier)
    {
        var oldValue = Value;
        _modifiers.Add(modifier);
        OnValueChanged(oldValue);
        var time = Time.time + modifier.time;
        _gameTimeScheduler?.Schedule(() => RemoveModifier(modifier),time);
    }
    public bool RemoveModifier(StatModifier modifier)
    {
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
        onValueChanged?.Invoke();
    }
}