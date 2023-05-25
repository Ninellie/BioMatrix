public class AddEffectWhileTrue : IEffect
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string TargetName { get; set; }

    public IEffect Effect { get; set; }
    public PropTrigger AddTrigger { get; set; }
    public PropTrigger RemoveTrigger { get; set; }
    public string ResourceConditionPath { get; set; }

    public string Identifier { get; set; }

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
        var isAddCondition = (bool)EventHelper.GetPropByPath(target, ResourceConditionPath);
        if (isAddCondition)
        {
            AddEffect();
        }
    }

    public void Detach()
    {
        RemoveEffect();
    }

    public void Subscribe(Entity target)
    {
        EventHelper.AddActionByName(EventHelper.GetPropByPath(target, AddTrigger.Path), AddTrigger.Name, AddEffect);
        EventHelper.AddActionByName(EventHelper.GetPropByPath(target, RemoveTrigger.Path), RemoveTrigger.Name, RemoveEffect);
    }

    public void Unsubscribe(Entity target)
    {
        EventHelper.RemoveActionByName(EventHelper.GetPropByPath(target, RemoveTrigger.Path), RemoveTrigger.Name, RemoveEffect);
        EventHelper.RemoveActionByName(EventHelper.GetPropByPath(target, AddTrigger.Path), AddTrigger.Name, AddEffect);
    }
    
    private void AddEffect()
    {
        for (int i = 0; i < StacksCount.GetValue(); i++)
        {
            _target.AddEffectStack(Effect);
        }
    }

    private void RemoveEffect()
    {
        for (int i = 0; i < StacksCount.GetValue(); i++)
        {
            _target.RemoveEffectStack(Effect);
        }
    }

    public AddEffectWhileTrue(
        string name,
        string description,
        string targetName,
        IEffect effect,
        PropTrigger addTrigger,
        PropTrigger removeTrigger,
        string resourceConditionPath
    ) : this(
        name,
        description,
        targetName,
        effect,
        addTrigger,
        removeTrigger,
        resourceConditionPath,
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

    public AddEffectWhileTrue(
        string name,
        string description,
        string targetName,
        IEffect effect,
        PropTrigger addTrigger,
        PropTrigger removeTrigger,
        string resourceConditionPath,
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
        Effect = effect;
        AddTrigger = addTrigger;
        RemoveTrigger = removeTrigger;
        ResourceConditionPath = resourceConditionPath;
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