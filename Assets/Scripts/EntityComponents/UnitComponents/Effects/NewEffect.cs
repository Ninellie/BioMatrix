using System;
using System.Collections.Generic;
using Assets.Scripts.EntityComponents;
using Assets.Scripts.EntityComponents.Stats;
using UnityEngine;

public interface IEffect
{
    string Name { get; }
    string Description { get; }
    void Activate();
    void Deactivate();
}

public interface IRespondingEffect : IEffect
{
    void Subscribe(Entity target);
    void Unsubscribe(Entity target);
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
public class EffectData
{
    public string name;
    public string description;
}

[Serializable]
public class StatModData
{
    [SerializeField]
    public StatMod mod;

    [SerializeField]
    public StatName statName;
}

public interface IStatModifier
{
    public List<StatModData> StatModList { get; }

    public void SetStatList(StatList statList);
}