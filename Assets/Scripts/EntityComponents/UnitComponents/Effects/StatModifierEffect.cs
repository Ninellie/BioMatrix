using System;
using System.Collections.Generic;
using Assets.Scripts.EntityComponents.Resources;
using Assets.Scripts.EntityComponents.Stats;
using Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents;
using UnityEngine;

[Serializable]
public class EffectAdderEffect : Effect, IRespondingEffect, IEffectAdder
{
    [SerializeField]
    private StackableTemporaryStatModifierEffect _effect;

    [SerializeField]
    private ResourceName _resourceName;

    [SerializeField]
    private ResourceEventType _event;

    private OverUnitDataAggregator _effectsAggregator;

    public void SetResourceList(ResourceList resourceList)
    {
        resourceList.GetResource(_resourceName).AddListenerToEvent(_event, () => _effectsAggregator.AddEffect(_effect));
    }

    public void SetEffectsManager(OverUnitDataAggregator effectsAggregator)
    {
        _effectsAggregator = effectsAggregator;
    }
}

[Serializable]
public class StackableTemporaryStatModifierEffect : StackableStatModifierEffect, ITemporaryEffect
{
    [SerializeField] private float _duration;
    [SerializeField] private bool _isDurationUpdates;
    [SerializeField] private bool _isDurationStacks;

    public float Duration => _duration;
    public bool IsDurationUpdates { get; }
    public bool IsDurationStacks { get; }
    public string Identifier { get; set; }
}

[Serializable]
public class StackableStatModifierEffect : StatModifierEffect, IStackableEffect
{
    [SerializeField] private int _maxStacks;
    [SerializeField] private int _currentStackCount;
    [SerializeField] private int _initialStacks;

    public int StacksCount => _currentStackCount;
    public int MaxStacks => _maxStacks;
    public int InitialStacks => _initialStacks;

    public void AddStack()
    {
        if (StacksCount == MaxStacks)
            return;

        _currentStackCount++;
        AddMods();
    }

    public void RemoveStack()
    {
        if (StacksCount == 0)
            return;

        _currentStackCount--;
        RemoveMods();
    }

    public new void Activate()
    {
        while (_currentStackCount != _initialStacks)
        {
            AddStack();
        }
    }

    public new void Deactivate()
    {
        while (_currentStackCount > 0)
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
            statList.GetStat(statModData.statName).AddModifier(statModData.mod);
        }
    }

    public new void Deactivate()
    {
        foreach (var statModData in statModList)
        {
            statList.GetStat(statModData.statName).RemoveModifier(statModData.mod);
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
            statList.GetStat(statModData.statName).AddModifier(statModData.mod);
        }
    }

    protected void RemoveMods()
    {
        foreach (var statModData in statModList)
        {
            statList.GetStat(statModData.statName).RemoveModifier(statModData.mod);
        }
    }
}