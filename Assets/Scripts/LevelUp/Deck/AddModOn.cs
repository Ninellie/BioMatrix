using System;
using System.Collections.Generic;

public class AddModOn : IEffect
{
    public string Name { get; set;  }
    public string Description { get; set; }
    public List<(StatModifier mod, string statName)> Modifiers { get; set; }
    public string TargetName { get; set; } // Player
    public string TriggerName { get; set; } // onValueChanged for example
    public string TriggerTypeName { get; set; } // Resource, Stat or Entity
    public string TriggerPropName { get; set; } // If TypeName == Resource or Stat

    private Stat[] _stats;
    private int[] _addedModsCounts;

    public void Attach(Entity target)
    {
        _stats = new Stat[Modifiers.Count];
        _addedModsCounts = new int[Modifiers.Count];
        int i = 0;

        foreach (var tuple in Modifiers)
        {
            _stats[i] = target.GetStatByName(tuple.statName);
            i++;
        }

        if (TriggerName == nameof(Attach))
        {
            AddMod();
        }
    }

    public void Detach(Entity target)
    {
        int i = 0;
        foreach (var tuple in Modifiers)
        {
            if (tuple.Item1.IsTemporary) return;

            for (int j = 0; j < _addedModsCounts[i]; j++)
            {
                _stats[i].RemoveModifier(tuple.mod);
            }
            _addedModsCounts[i] = 0;
            i++;
        }
    }

    public void Subscribe(Entity target)
    {
        //EventHelper.AddActionByName(EventHelper.GetPropByName(target, TriggerPropName), TriggerName, AddMod);

        switch (TriggerTypeName)
        {
            case nameof(Resource):
                EventHelper.AddActionByName(EventHelper.GetPropByName(target, TriggerPropName), TriggerName, AddMod);
                break;
            case nameof(Stat):
                EventHelper.AddActionByName(EventHelper.GetPropByName(target, TriggerPropName), TriggerName, AddMod);
                break;
            case nameof(Entity):
                EventHelper.AddActionByName(EventHelper.GetPropByName(target, null), TriggerName, AddMod);
                break;
            case nameof(Firearm):
                EventHelper.AddActionByName(EventHelper.GetPropByName(target, TriggerPropName), TriggerName, AddMod);
                break;
        }
    }

    public void Unsubscribe(Entity target)
    {
        Action trigger;
        switch (TriggerTypeName)
        {
            case nameof(Resource):
                trigger = target.GetResourceByName(TriggerPropName).GetActionByName(TriggerName);
                trigger -= AddMod;
                break;
            case nameof(Stat):
                trigger = target.GetStatByName(TriggerPropName).onValueChanged;
                trigger -= AddMod;
                break;
            case nameof(Entity):
                EventHelper.RemoveActionByName(target, TriggerName, AddMod);
                break;
            case nameof(IEffect):
                break;
        }
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