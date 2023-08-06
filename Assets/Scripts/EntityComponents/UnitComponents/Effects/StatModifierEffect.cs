using System;
using System.Collections.Generic;
using Assets.Scripts.EntityComponents.Stats;
using UnityEngine;

public class StackableStatModifierEffect : StatModifierEffect, IStackable
{
    public int StacksCount { get; set; }
    public int MaxStacks { get; }


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