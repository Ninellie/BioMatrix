using System;
using System.Collections.Generic;
using System.Linq;

public class Stat
{
    public float Value => GetActualValue();
    public IEnumerable<StatModifier> Modifiers => _modifiers;
    public Action onValueChanged;
    public readonly float baseValue;
    public readonly bool isModifiable;
    public readonly float baseAddedValue;
    private float AddedValue => GetAddedValue();
    private readonly float _baseMultiplierValue;
    private float MultiplierValue => GetMultiplierValue();
    private readonly List<StatModifier> _modifiers;
    private float _oldValue;

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
    public Stat(float baseValue) : this(baseValue, true, 1, 0)
    {
        _modifiers = new List<StatModifier>();
    }
    public Stat(float baseValue, bool isModifiable, float baseMultiplierValue, float baseAddedValue)
    {
        this.baseValue = baseValue;
        this.isModifiable = isModifiable;
        _baseMultiplierValue = baseMultiplierValue;
        this.baseAddedValue = baseAddedValue;
    }
    public void AddModifier(StatModifier modifier)
    {
        RememberCurrentValue();
        _modifiers.Add(modifier);
        OnValueChanged();
    }
    public bool RemoveModifier(StatModifier modifier)
    {
        RememberCurrentValue();
        if (_modifiers.Contains(modifier))
        {
            _modifiers.Remove(modifier);
            OnValueChanged();
            return true;
        };
        return false;
    }
    public bool ClearModifiersList()
    {
        RememberCurrentValue();
        if (_modifiers.Count == 0)
        {
            return false;
        }
        _modifiers.Clear();
        OnValueChanged();
        return true;
    }
    private float GetAddedValue()
    {
        return _modifiers.Where(modifier => modifier.Type == OperationType.Addition).Sum(modifier => modifier.Value) + baseAddedValue;
    }
    private float GetMultiplierValue()
    {
        return _modifiers.Where(modifier => modifier.Type == OperationType.Multiplication).Sum(modifier => modifier.Value) + _baseMultiplierValue;
    }
    private float GetActualValue()
    {
        if (isModifiable == false)
        {
            return (baseValue + baseAddedValue) * _baseMultiplierValue;
        }
        return (baseValue + AddedValue) * MultiplierValue;
    }
    private void RememberCurrentValue()
    {
        _oldValue = GetActualValue();
    }
    private void OnValueChanged()
    {
        if (Value.Equals(_oldValue))
        {
            onValueChanged?.Invoke();
        }
    }
}