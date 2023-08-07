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

public interface IResponding
{
    void Subscribe(Entity target);
    void Unsubscribe(Entity target);
}

public interface IStackableTemporary
{
    bool IsStackSeparateDuration { get; }
}

public interface ITemporary
{
    float Duration { get; }
}

public interface IStackable
{
    int StacksCount { get; set; }
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