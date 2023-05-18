using System.Collections.Generic;

public class AddModPerMissingResource : IEffect
{
    public string Name { get; set; }
    public string Description { get; set; }
    public List<(StatModifier mod, string statName)> Modifiers { get; set; } 
    public string TargetName { get; set; }
    public Trigger TriggerStat { get; set; }
    public Trigger TriggerResource { get; set; }

    private Resource _resource;
    private Stat[] _stats;
    private StatModifier[] _mods;

    public void Attach(Entity target)
    {
        _stats = new Stat[Modifiers.Count];
        _mods = new StatModifier[Modifiers.Count];
        _resource = (Resource)EventHelper.GetPropByName(target, TriggerResource.Path);

        int i = 0;
        foreach (var tuple in Modifiers)
        {
            _stats[i] = (Stat)EventHelper.GetPropByName(target, tuple.statName);
            i++;
        }

        AddMods();
    }

    public void Detach()
    {
        RemoveMods();
    }

    public void Subscribe(Entity target)
    {
        EventHelper.AddActionByName(EventHelper.GetPropByName(target, TriggerStat.Path), TriggerStat.Name, UpdateMods);
        EventHelper.AddActionByName(EventHelper.GetPropByName(target, TriggerResource.Path), TriggerResource.Name, UpdateMods);
    }

    public void Unsubscribe(Entity target)
    {
        EventHelper.RemoveActionByName(EventHelper.GetPropByName(target, TriggerResource.Path), TriggerResource.Name, UpdateMods);
        EventHelper.RemoveActionByName(EventHelper.GetPropByName(target, TriggerStat.Path), TriggerStat.Name, UpdateMods);
    }

    private void UpdateMods()
    {
        RemoveMods();
        AddMods();
    }

    private void AddMods()
    {
        int i = 0;
        foreach (var tuple in Modifiers)
        {
            var newValue = tuple.mod.Value * _resource.GetLackValue();
            var isTemp = tuple.mod.IsTemporary;
            var newMod = isTemp switch
            {
                false => new StatModifier(tuple.mod.Type, newValue),
                _ => new StatModifier(tuple.mod.Type, newValue, tuple.mod.Duration)
            };

            _stats[i].AddModifier(newMod);

            if (!isTemp) _mods[i] = newMod;

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
                _stats[i].RemoveModifier(_mods[i]);
            }

            i++;
        }
    }
}