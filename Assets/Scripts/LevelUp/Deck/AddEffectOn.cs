public class AddEffectOn : IEffect
{
    public string Name { get; set; }
    public string Description { get; set; }
    public IEffect Effect { get; set; }
    public string TargetName { get; set; }
    public bool IsTemporal { get; set; }
    public bool IsProlongable { get; set; }
    public bool IsStacking { get; set; }
    public bool IsUpdatable { get; set; }
    public Trigger Trigger { get; set; }
    public Resource StacksCount { get; set; }
    public Stat MaxStackCount { get; set; }
    public Stat Duration { get; set; }

    private Entity _target;
    private string _identifier;

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
    }

    public void Unsubscribe(Entity target)
    {
        EventHelper.RemoveActionByName(EventHelper.GetPropByName(target, Trigger.Path), Trigger.Name, AddEffect);
    }

    private void AddEffect()
    {
        _target.AddEffectStack(Effect);
    }

    private void RemoveEffect()
    {
        _target.RemoveEffectStack(Effect);
    }
}