using System.Collections.Generic;

public class AttachModAdderEffect : IEffect
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string TargetName { get; set; }
    public string Identifier { get; set; }

    public List<(StatModifier mod, string statPath)> Modifiers { get; set; }

    public bool IsTemporal { get; set; }
    public Stat Duration { get; set; }
    public bool IsDurationStacks { get; set; }
    public bool IsDurationUpdates { get; set; }

    public bool IsStacking { get; set; }
    public bool IsStackSeparateDuration { get; set; }
    public Stat MaxStackCount { get; set; }
    public Resource StacksCount { get; set; }

    private Entity _target;

    public void Attach(Entity target)
    {
        _target = target;
        AddMods();
    }

    public void Detach()
    {
        while (!StacksCount.IsEmpty)
        {
            RemoveMods();
            StacksCount.Decrease();
        }
    }

    public void Subscribe(Entity target)
    {
        StacksCount.IncrementEvent += AddMods;
        StacksCount.DecrementEvent += RemoveMods;
    }

    public void Unsubscribe(Entity target)
    {
        StacksCount.IncrementEvent -= AddMods;
        StacksCount.DecrementEvent -= RemoveMods;
    }

    private void AddMods()
    {
        foreach (var tuple in Modifiers)
        {
            _target.AddStatModifier(tuple.mod, tuple.statPath);
        }
    }

    private void RemoveMods()
    {
        foreach (var tuple in Modifiers)
        {
            _target.RemoveStatModifier(tuple.mod, tuple.statPath);
        }
    }

    public AttachModAdderEffect(
        string name,
        string description,
        string targetName,
        List<(StatModifier mod, string statPath)> modifiers
        ) : this(
        name,
        description,
        targetName,
        modifiers,
        false,
        new Stat(0, false),
        false,
        false,
        false,
        false,
        new Stat(1, false)
    )
    {
    }

    public AttachModAdderEffect(
        string name,
        string description,
        string targetName,
        List<(StatModifier mod, string statPath)> modifiers,
        bool isTemporal,
        Stat duration,
        bool isDurationStacks,
        bool isDurationUpdates,
        bool isStacking,
        bool isStackSeparateDuration,
        Stat maxStackCount
        )
    {
        Name = name;
        Description = description;
        TargetName = targetName;
        Modifiers = modifiers;
        IsTemporal = isTemporal;
        Duration = duration;
        IsDurationStacks = isDurationStacks;
        IsDurationUpdates = isDurationUpdates;
        IsStacking = isStacking;
        IsStackSeparateDuration = isStackSeparateDuration;
        MaxStackCount = maxStackCount;
        StacksCount = new Resource(MaxStackCount);
    }
}