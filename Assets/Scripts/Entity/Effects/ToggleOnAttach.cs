public class ToggleOnAttach : IEffect
{
    public string Name { get; set; }
    public string Description { get; }
    public string TargetName { get; set; }

    public string TogglePropPath { get; set; }
    public bool Value { get; set; }

    public string Identifier { get; set; }

    public bool IsTemporal { get; }
    public Stat Duration { get; }
    public bool IsDurationStacks { get; }
    public bool IsDurationUpdates { get; }

    public bool IsStacking { get; }
    public bool IsStackSeparateDuration { get; }
    public Resource StacksCount { get; }
    public Stat MaxStackCount { get; }

    private Entity _target;

    public void Attach(Entity target)
    {
        _target = target;
        EventHelper.SetPropValueByPath(target, TogglePropPath, Value);
    }

    public void Detach()
    {
        EventHelper.SetPropValueByPath(_target, TogglePropPath, !Value);
    }

    public void Subscribe(Entity target)
    {
    }

    public void Unsubscribe(Entity target)
    {
    }


    public ToggleOnAttach(
        string name,
        string description,
        string targetName,
        string togglePropPath,
        bool value
        ) : this(
        name,
        description,
        targetName, 
        togglePropPath, 
        value, 
        false,
        new Stat(0, false),
        false,
        false
        )
    {
    }

    public ToggleOnAttach(
        string name,
        string description,
        string targetName,
        string togglePropPath,
        bool value,
        bool isTemporal,
        Stat duration,
        bool isDurationStacks,
        bool isDurationUpdates
        )
    {
        Name = name;
        Description = description;
        TargetName = targetName;
        TogglePropPath = togglePropPath;
        Value = value;
        IsTemporal = isTemporal;
        Duration = duration;
        IsDurationStacks = isDurationStacks;
        IsDurationUpdates = isDurationUpdates;
        IsStacking = false;
        IsStackSeparateDuration = false;
        MaxStackCount = new Stat(1, false);
        StacksCount = new Resource(MaxStackCount);
    }
}