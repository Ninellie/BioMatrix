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
public class Effect : IEffect
{
    [SerializeField] private string _name;
    [SerializeField] private string _description;
    public string Name => _name;
    public string Description => _description;

    public virtual void Activate()
    {
    }

    public virtual void Deactivate()
    {
    }
}

[Serializable]
public class StatModData
{
    [SerializeField]
    public StatMod mod;

    [SerializeField]
    public StatName statName;
}

[Serializable]
public class StatModifierEffect : Effect
{
    [SerializeField]
    private List<StatModData> _statModList;

    [SerializeField]
    private StatList _statList;

    public new void Activate()
    {
        foreach (var statModData in _statModList)
        {
            _statList.GetStatByName(statModData.statName).AddModifier(statModData.mod);
        }
    }

    public new void Deactivate()
    {
        foreach (var statModData in _statModList)
        {
            _statList.GetStatByName(statModData.statName).RemoveModifier(statModData.mod);
        }
    }
}