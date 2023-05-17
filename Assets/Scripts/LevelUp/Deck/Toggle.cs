public class Toggle : IEffect 
{
    public string Name { get; set; }
    public string TargetName { get; set; }
    public string PathToToggleProp { get; set; }
    public bool Value { get; set; }
    private bool ToggleProp { get; set; }

    public void Attach(Entity target)
    {
        ToggleProp = (bool)EventHelper.GetPropByName(target, PathToToggleProp);
        ToggleProp = Value;
    }

    public void Detach()
    {
        ToggleProp = !Value;
    }

    public void Subscribe(Entity target)
    {
    }

    public void Unsubscribe(Entity target)
    {
    }
}