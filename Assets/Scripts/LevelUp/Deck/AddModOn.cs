﻿using System.Collections.Generic;

public class AddModOnAttach : IEffect
{
    public string Name { get; }
    public string Description { get; set; }
    public List<(StatModifier mod, string statName)> Modifiers { get; set; }
    public string TargetName { get; }
    public void Attach(Entity target)
    {
        foreach (var tuple in Modifiers)
        {
            target.AddStatModifier(tuple.mod, tuple.statName);
        }
    }

    public void Detach()
    {
        throw new System.NotImplementedException();
    }

    public void Subscribe(Entity target)
    {
    }

    public void Unsubscribe(Entity target)
    {
    }
}

public class AddModOn : IEffect
{
    public string Name { get; set;  }
    public string Description { get; set; }
    public List<(StatModifier mod, string statName)> Modifiers { get; set; }
    public string TargetName { get; set; }
    public Trigger Trigger { get; set; }

    private Stat[] _stats;
    private int[] _addedModsCounts;

    public void Attach(Entity target)
    {
        _stats = new Stat[Modifiers.Count];
        _addedModsCounts = new int[Modifiers.Count];
        int i = 0;

        foreach (var tuple in Modifiers)
        {
            _stats[i] = (Stat)EventHelper.GetPropByName(target, tuple.statName);
            i++;
        }

        if (Trigger.Name == nameof(Attach))
        {
            AddMod();
        }
    }

    public void Detach()
    {
        int i = 0;
        foreach (var tuple in Modifiers)
        {
            if (!tuple.mod.IsTemporary)
            {
                for (int j = 0; j < _addedModsCounts[i]; j++)
                {
                    _stats[i].RemoveModifier(tuple.mod);
                }

                _addedModsCounts[i] = 0;
            }

            i++;
        }

        if (Trigger.Name == nameof(Detach))
        {
            AddMod();
        }
    }

    public void Subscribe(Entity target)
    {
        EventHelper.AddActionByName(EventHelper.GetPropByName(target, Trigger.Path), Trigger.Name, AddMod);
    }

    public void Unsubscribe(Entity target)
    {
        EventHelper.RemoveActionByName(EventHelper.GetPropByName(target, Trigger.Path), Trigger.Name, AddMod);
    }

    private void AddMod()
    {
        int i = 0;

        foreach (var tuple in Modifiers)
        {
            _stats[i].AddModifier(tuple.mod);

            if (!tuple.mod.IsTemporary)
            {
                _addedModsCounts[i]++;
            }

            i++;
        }
    }
}