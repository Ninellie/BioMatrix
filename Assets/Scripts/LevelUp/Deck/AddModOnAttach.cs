﻿using System.Collections.Generic;

public class AddModOnAttach : IEffect
{
    public string Name { get; set; }
    public string Description { get; set; }
    public List<(StatModifier mod, string statName)> Modifiers { get; set; }
    public string TargetName { get; set; }
    public bool IsTemporal { get; set; }
    public bool IsProlongable { get; set; }
    public bool IsStacking { get; set; }
    public bool IsUpdatable { get; set; }
    public Resource StacksCount { get; set; }
    public Stat MaxStackCount { get; set; }
    public Stat Duration { get; set; }

    private Entity _target;

    public void Attach(Entity target)
    {
        _target = target;
        AddMods();
    }

    public void Detach()
    {
        while (StacksCount.IsEmpty)
        {
            RemoveMods();
            StacksCount.Decrease();
        }
    }

    public void Subscribe(Entity target)
    {
        StacksCount.IncrementEvent += AddMods;
    }

    public void Unsubscribe(Entity target)
    {
        StacksCount.DecrementEvent += RemoveMods;
    }

    private void AddMods()
    {
        foreach (var tuple in Modifiers)
        {
            _target.AddStatModifier(tuple.mod, tuple.statName);
        }
    }

    private void RemoveMods()
    {
        foreach (var tuple in Modifiers)
        {
            _target.RemoveStatModifier(tuple.mod, tuple.statName);
        }
    }
}