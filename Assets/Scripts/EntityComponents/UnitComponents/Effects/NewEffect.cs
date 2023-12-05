using System;
using System.Collections.Generic;
using Assets.Scripts.EntityComponents.Resources;
using Assets.Scripts.EntityComponents.Stats;
using Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents;
using Assets.Scripts.SourceStatSystem;
using UnityEngine;

public interface IEffect
{
    string Name { get; }
    string Description { get; }
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
    StatSources StatSources { get; }
    List<StatSourceData> StatSourceList { get; }
}