public class Toggle : IEffect 
{
    public string Name { get; set; }
    public string TargetName { get; set; }
    public string PathToToggleProp { get; set; }
    public bool Value { get; set; }

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
}