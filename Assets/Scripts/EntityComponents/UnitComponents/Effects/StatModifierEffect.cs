using System;
using System.Collections.Generic;
using Assets.Scripts.EntityComponents.Stats;
using UnityEngine;

public class StackableEffectStatModifierEffect : StatModifierEffect, IStackableEffect
{
    [SerializeField] private int _maxStacks;
    [SerializeField] private int _stackCount;
    [SerializeField] private int _initialStacks;

    public int StacksCount => _stackCount;
    public int MaxStacks => _maxStacks;
    public int InitialStacks => _initialStacks;

    public void AddStack()
    {
        if (StacksCount == MaxStacks)
            return;

        _stackCount++;
        AddMods();
    }

    public void RemoveStack()
    {
        if (StacksCount == 0)
            return;

        _stackCount--;
        RemoveMods();
    }

    public new void Activate()
    {
        for (int i = 0; i < _initialStacks; i++)
        {
            AddStack();
        }
    }

    public new void Deactivate()
    {
        while (_stackCount > 0)
        {
            RemoveStack();
        }

        for (int i = 0; i < StacksCount; i++)
        {
            RemoveStack();
        }
    }
}

[Serializable]
public class StatModifierEffect : Effect, IStatModifier
{
    [SerializeField]
    protected List<StatModData> statModList;

    [SerializeField]
    protected StatList statList;
    public List<StatModData> StatModList => statModList;

    public new void Activate()
    {
        foreach (var statModData in statModList)
        {
            statList.GetStatByName(statModData.statName).AddModifier(statModData.mod);
        }
    }

    public new void Deactivate()
    {
        foreach (var statModData in statModList)
        {
            statList.GetStatByName(statModData.statName).RemoveModifier(statModData.mod);
        }
    }

    public void SetStatList(StatList statList)
    {
        this.statList = statList;
    }

    protected void AddMods()
    {
        foreach (var statModData in statModList)
        {
            statList.GetStatByName(statModData.statName).AddModifier(statModData.mod);
        }
    }

    protected void RemoveMods()
    {
        foreach (var statModData in statModList)
        {
            statList.GetStatByName(statModData.statName).RemoveModifier(statModData.mod);
        }
    }
}