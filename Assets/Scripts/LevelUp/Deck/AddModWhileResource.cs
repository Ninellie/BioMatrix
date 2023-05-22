using System.Collections.Generic;

public class AddModWhileResource : IEffect
{
    public string Name { get; set; }
    public string Description { get; set; }
    public List<(StatModifier mod, string statName)> Modifiers { get; set; }
    public string TargetName { get; set; }
    public bool IsTemporal { get; }
    public bool IsProlongable { get; }
    public bool IsStacking { get; }
    public bool IsUpdatable { get; }
    public Trigger AddTrigger { get; set; }
    public Trigger RemoveTrigger { get; set; }
    public string ResourceConditionName { get; set; }

    private Stat[] _modifiedStats;
    private StatModifier[] _addedMods;

    public void Attach(Entity target)
    {
        _modifiedStats = new Stat[Modifiers.Count];
        _addedMods = new StatModifier[Modifiers.Count];

        int i = 0;
        foreach (var tuple in Modifiers)
        {
            _modifiedStats[i] = (Stat)EventHelper.GetPropByName(target, tuple.statName);
            i++;
        }

        bool isAddCondition = (bool)EventHelper.GetPropByName(target, ResourceConditionName);
        if (isAddCondition)
        {
            AddMods();
        }
    }

    public void Detach()
    {
        RemoveMods();
    }

    public void Subscribe(Entity target)
    {
        EventHelper.AddActionByName(EventHelper.GetPropByName(target, AddTrigger.Path), AddTrigger.Name, AddMods);
        EventHelper.AddActionByName(EventHelper.GetPropByName(target, RemoveTrigger.Path), RemoveTrigger.Name, RemoveMods);
    }

    public void Unsubscribe(Entity target)
    {
        EventHelper.RemoveActionByName(EventHelper.GetPropByName(target, RemoveTrigger.Path), RemoveTrigger.Name, RemoveMods);
        EventHelper.RemoveActionByName(EventHelper.GetPropByName(target, AddTrigger.Path), AddTrigger.Name, AddMods);
    }
    
    private void AddMods()
    {
        int i = 0;
        foreach (var tuple in Modifiers)
        {
            var isTemp = tuple.mod.IsTemporary;

            _modifiedStats[i].AddModifier(tuple.mod);

            if (!isTemp) _addedMods[i] = tuple.mod;

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
                _modifiedStats[i].RemoveModifier(_addedMods[i]);
            }

            i++;
        }
    }
}