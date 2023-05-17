﻿using System.Collections.Generic;

public class AddModOnAttach : IEffect
{
    public string Name { get; }
    public string Description { get; set; }
    public List<(StatModifier mod, string statName)> Modifiers { get; set; }
    public string TargetName { get; }

    private Entity _target;
    public void Attach(Entity target)
    {
        _target = target;
        foreach (var tuple in Modifiers)
        {
            _target.AddStatModifier(tuple.mod, tuple.statName);
        }
    }

    public void Detach()
    {
        foreach (var tuple in Modifiers)
        {
            _target.AddStatModifier(tuple.mod, tuple.statName);
        }
    }

    public void Subscribe(Entity target)
    {
    }

    public void Unsubscribe(Entity target)
    {
    }
}