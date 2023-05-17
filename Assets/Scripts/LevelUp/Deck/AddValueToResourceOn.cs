public class AddValueToResourceOn : IEffect
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string TargetName { get; set; }
    public string PathToResource { get; set; }
    public int Value { get; set; }
    public Trigger Trigger { get; set; }
    private Resource _resource;

    public void Attach(Entity target)
    {
        _resource = (Resource)EventHelper.GetPropByName(target, PathToResource);
    }

    public void Detach()
    {
    }

    public void Subscribe(Entity target)
    {
        EventHelper.AddActionByName(EventHelper.GetPropByName(target, Trigger.Path), Trigger.Name, AddValue);
    }

    public void Unsubscribe(Entity target)
    {
        EventHelper.RemoveActionByName(EventHelper.GetPropByName(target, Trigger.Path), Trigger.Name, AddValue);
    }

    private void AddValue()
    {
        _resource.Increase(Value);
    }
}