using System;
using System.Collections.Generic;
using Assets.Scripts.EntityComponents.Resources;
using Assets.Scripts.EntityComponents.Stats;
using UnityEngine;

public class EffectAdderEffect : Effect, IRespondingEffect, IEffectAdder
{
    [SerializeReference]
    private IEffect _effect;

    [SerializeField]
    private ResourceName _resourceName;

    [SerializeField]
    private ResourceEventType _event;

    private EffectsList _effectsList;

    public void SetResourceList(ResourceList resourceList)
    {
        resourceList.GetResourceByName(_resourceName).GetEvent(_event).AddListener(() => _effectsList.AddEffect(_effect));
    }

    public void SetEffectsList(EffectsList effectList)
    {
        _effectsList = effectList;
    }
}

public class StackableTemporaryStatModifierEffect : StackableStatModifierEffect, ITemporaryEffect
{
    [SerializeField]
    private float _duration;

    private bool _isDurationUpdates;

    private bool _isDurationStacks;


    public float Duration => _duration;
    public bool IsDurationUpdates { get; }
    public bool IsDurationStacks { get; }
    public string Identifier { get; set; }
}

public class StackableStatModifierEffect : StatModifierEffect, IStackableEffect
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