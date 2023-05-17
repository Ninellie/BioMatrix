using System.Collections.Generic;

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