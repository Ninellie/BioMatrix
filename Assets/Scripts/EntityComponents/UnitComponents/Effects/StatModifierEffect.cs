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
        resourceList.GetResourceByName(_resourceName).GetEvent(_event).AddListener(() => _effectsAggregator.AddEffect(_effect));
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
        while (_stackCount != _initialStacks)
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