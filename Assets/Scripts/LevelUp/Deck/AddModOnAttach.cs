using System.Collections.Generic;

public class AddModOnAttach : IEffect
{
    public string Name { get; set; }
    public string Description { get; set; }
    public List<(StatModifier mod, string statName)> Modifiers { get; set; }
    public string TargetName { get; set; }
    public bool IsTemporal { get; set; }
    public bool IsProlongable { get; set; }
    public bool IsStackable { get; set; }
    public bool IsUpdatable { get; set; }
    public Resource StacksCount { get; set; }
    public Stat MaxStackCount { get; set; }

    private Entity _target;
    private string _identifier;

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
            _target.RemoveStatModifier(tuple.mod, tuple.statName);
        }
    }

    public void Subscribe(Entity target)
    {
    }

    public void Unsubscribe(Entity target)
    {
    }
}