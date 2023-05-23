public class ToggleOnAttach : IEffect
{
    public string Name { get; set; }
    public string Description { get; }
    public string TargetName { get; set; }
    public string Identifier { get; set; }

    public string PathToToggleProp { get; set; }
    public bool Value { get; set; }

    public bool IsTemporal { get; }
    public Stat Duration { get; }
    public bool IsDurationStacks { get; }
    public bool IsDurationUpdates { get; }

    public bool IsStacking { get; }
    public bool IsStackSeparateDuration { get; }
    public Resource StacksCount { get; }
    public Stat MaxStackCount { get; }

    private bool _toggleProp;

    public void Attach(Entity target)
    {
        _toggleProp = (bool)EventHelper.GetPropByName(target, PathToToggleProp);
        _toggleProp = Value;
    }

    public void Detach()
    {
        _toggleProp = !Value;
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
        string pathToToggleProp,
        bool value
        ) : this(
        name,
        description,
        targetName, 
        pathToToggleProp, 
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
        string pathToToggleProp,
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
        PathToToggleProp = pathToToggleProp;
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