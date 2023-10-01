using System;
using System.Collections.Generic;
using Assets.Scripts.EntityComponents.Resources;
using Assets.Scripts.EntityComponents.Stats;
using Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents;
using UnityEngine;

public enum TargetName
{
    Player,
    Shield,
    TurretHub,
    TurretHubWeapon,
    Firearm,
}

public interface IEffect
{
    string Name { get; }
    string Description { get; }
    TargetName TargetName { get; }
    void Activate();
    void Deactivate();
}

public interface IEffectAdder : IEffect
{
    void SetEffectsManager(OverUnitDataAggregator effectsAggregator);
    //void SetEffectsRepository(EffectsRepository effectsRepository);
}

public interface IResourceOperator : IEffect
{
    void SetResourceList(ResourceList resourceList);
}

public interface IRespondingEffect : IEffect
{
    void Subscribe();
}

public interface IStackableTemporaryEffect : IStackableEffect, ITemporaryEffect
{
    bool IsStackSeparateDuration { get; }
}

public interface ITemporaryEffect : IEffect
{
    float Duration { get; }
    bool IsDurationUpdates { get; }
    bool IsDurationStacks { get; }
    string Identifier { get; set; }
}

public interface IStackableEffect : IEffect
{
    int StacksCount { get; }
    int MaxStacks { get; }
    int InitialStacks { get; }

    void AddStack();
    void RemoveStack();
}

[Serializable]
public class StatModData
{
    [HideInInspector] public string strName;
    [SerializeField] public StatMod mod;
    [SerializeField] public StatName statName;
}

public interface IStatModifier
{
    public List<StatModData> StatModList { get; }

    public void SetStatList(StatList statList);
}