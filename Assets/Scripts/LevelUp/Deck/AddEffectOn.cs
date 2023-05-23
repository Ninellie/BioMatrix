public class AddEffectOn : IEffect
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string TargetName { get; set; }
    public Trigger Trigger { get; set; }
    public string Identifier { get; set; }

    public IEffect Effect { get; set; }

    public bool IsTemporal { get; set; }
    public Stat Duration { get; set; }
    public bool IsDurationStacks { get; set; }
    public bool IsDurationUpdates { get; set; }

    public bool IsStacking { get; set; }
    public bool IsStackSeparateDuration { get; set; }
    public Resource StacksCount { get; set; }
    public Stat MaxStackCount { get; set; }

    

    private Entity _target;

    public void Attach(Entity target)
    {
        _target = target;
        AddEffect();
    }

    public void Detach()
    {
        while (StacksCount.IsEmpty)
        {
            RemoveEffect();
            StacksCount.Decrease();
        }
    }

    public void Subscribe(Entity target)
    {
        EventHelper.AddActionByName(EventHelper.GetPropByName(target, Trigger.Path), Trigger.Name, AddEffect);
        StacksCount.IncrementEvent += AddEffect;
        StacksCount.DecrementEvent += RemoveEffect;
    }

    public void Unsubscribe(Entity target)
    {
        EventHelper.RemoveActionByName(EventHelper.GetPropByName(target, Trigger.Path), Trigger.Name, AddEffect);
        StacksCount.IncrementEvent -= AddEffect;
        StacksCount.DecrementEvent -= RemoveEffect;
    }

    private void AddEffect()
    {
        _target.AddEffectStack(Effect);
    }

    private void RemoveEffect()
    {
        _target.RemoveEffectStack(Effect);
    }

    public AddEffectOn(
        string name,
        string description,
        string targetName,
        Trigger trigger,
        IEffect effect
        ) : this(
        name,
        description,
        targetName,
        trigger,
        effect,
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
    public AddEffectOn(
        string name,
        string description,
        string targetName,
        Trigger trigger,
        IEffect effect,
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
        Trigger = trigger;
        Effect = effect;
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