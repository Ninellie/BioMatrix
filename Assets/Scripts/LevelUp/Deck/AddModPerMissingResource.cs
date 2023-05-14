using System;
using System.Collections.Generic;

public class AddModPerMissingResource : IEffect
{
    /* Следит сразу за двумя событиями: изменение текущего значения и изменение максимального значения,
     * После чего обновляет модификатор к стату, умножая его на разница между максимальным и текущим
     *
     */
    public string Name { get; set; }
    public string Description { get; set; }
    public List<(StatModifier mod, string statName)> Modifiers { get; set; } 
    public string TargetName { get; set; } // Player
    public Trigger TriggerStat { get; set; } // maxLife
    public Trigger TriggersResource { get; set; } // currentLife

    private Resource _resource;
    private Stat[] _stats;

    public void Attach(Entity target)
    {
        _stats = new Stat[Modifiers.Count];
        _resource = target.GetResourceByName(TriggersResource.Name);

        int i = 0;
        foreach (var tuple in Modifiers)
        {
            _stats[i] = target.GetStatByName(tuple.statName);
            i++;
        }

        AddMods();
    }

    public void Detach(Entity target)
    {
        RemoveMods();
    }
    public void Subscribe(Entity target)
    {
        EventHelper.AddActionByName(EventHelper.GetPropByName(target, TriggerStat.PropName), TriggerStat.Name, AddMods);
        EventHelper.AddActionByName(EventHelper.GetPropByName(target, TriggersResource.PropName), TriggersResource.Name, AddMods);
    }

    public void Unsubscribe(Entity target)
    {
        EventHelper.RemoveActionByName(EventHelper.GetPropByName(target, TriggersResource.PropName), TriggersResource.Name, AddMods);
        EventHelper.RemoveActionByName(EventHelper.GetPropByName(target, TriggerStat.PropName), TriggerStat.Name, AddMods);
    }

    private void AddMods()
    {
        RemoveMods();

        int i = 0;
        foreach (var tuple in Modifiers)
        {
            var newValue = tuple.mod.Value * _resource.GetLackValue();

            var newMod = tuple.mod.IsTemporary switch
            {
                false => new StatModifier(tuple.mod.Type, newValue),
                _ => new StatModifier(tuple.mod.Type, newValue, tuple.mod.Duration)
            };

            _stats[i].AddModifier(newMod);
            i++;
        }
    }

    private void RemoveMods()
    {
        int i = 0;
        foreach (var tuple in Modifiers)
        {
            if (!tuple.mod.IsTemporary)
            {
                _stats[i].RemoveModifier(tuple.mod);
            }

            i++;
        }
    }
}