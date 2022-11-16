using System;
using System.Collections.Generic;
using UnityEngine;

public class NumericalStat
{
    public float Value => GetActualValue();
    public BaseStat BaseStat => _baseStat;
    public IEnumerable<NumericalStatModifier> Modifiers => _modifiers;

    [SerializeField] private float _value;
    [SerializeField] private readonly BaseStat _baseStat;
    private readonly List<NumericalStatModifier> _modifiers = new();



    public NumericalStat(BaseStat baseStat)
    {
        _baseStat = baseStat;
    }
    public bool TryAddModifier(NumericalStatModifier modifier)
    {
        if (!IsValidModifier(modifier)) 
            return false;
        _modifiers.Add(modifier);
        return true;
    }
    private bool IsValidModifier(NumericalStatModifier modifier)
    {
        return BaseStat.IsModifiable != false && BaseStat.ValidOperationsList.Contains(modifier.Operation);
    }
    private float GetActualValue()
    {
        if (_modifiers.Count == 0)
            return BaseStat.BaseValue;
        float additionValue = 0;
        float multiplicationValue = 0;

        foreach (var modifier in Modifiers)
        {
            switch (modifier.Operation.Type)
            {
                case OperationType.Addition:
                    additionValue += modifier.BaseStat.BaseValue;
                    break;
                case OperationType.Multiplication:
                    multiplicationValue += modifier.BaseStat.BaseValue;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        return (BaseStat.BaseValue + additionValue) * multiplicationValue;
    }
}
