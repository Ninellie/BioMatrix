public class Toggle : IEffect 
{
    public string Name { get; set; }
    public string TargetName { get; set; }
    public string PathToToggleProp { get; set; }
    public bool Value { get; set; }

    public void Attach(Entity target)
    {
        bool toggleProp = (bool)EventHelper.GetPropByName(target, PathToToggleProp);
        toggleProp = Value;
    }

    public void Detach(Entity target)
    {
        bool toggleProp = (bool)EventHelper.GetPropByName(target, PathToToggleProp);
        toggleProp = !Value;
    }

    public void Subscribe(Entity target)
    {
    }

    public void Unsubscribe(Entity target)
    {
    }
}