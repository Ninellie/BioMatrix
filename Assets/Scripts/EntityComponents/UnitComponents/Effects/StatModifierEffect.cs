using System;
using System.Collections.Generic;
using Assets.Scripts.EntityComponents.Stats;
using UnityEngine;

public class StackableStatModifierEffect : StatModifierEffect, IStackable
{
    [SerializeField] private int _maxStacks;
    [SerializeField] private int _stackCount;
    [SerializeField] private int _initialStacks;

    public int StacksCount
    {
        get => _stackCount;
        set => _stackCount = value;
    }
    public int MaxStacks => _maxStacks;
    public int InitialStacks => _initialStacks;

    public void AddStack()
    {
        if (StacksCount == MaxStacks)
            return;

        StacksCount++;
        AddMods();
    }

    public void RemoveStack()
    {
        if (StacksCount == 0)
            return;

        StacksCount--;
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
        for (int i = 0; i < StacksCount; i++)
        {
            RemoveStack();
        }
    }

    private void AddMods()
    {
        foreach (var statModData in statModList)
        {
            statList.GetStatByName(statModData.statName).AddModifier(statModData.mod);
        }
    }

    private void RemoveMods()
    {
        foreach (var statModData in statModList)
        {
            statList.GetStatByName(statModData.statName).RemoveModifier(statModData.mod);
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
}