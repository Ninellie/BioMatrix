using UnityEngine;

public interface IChildClass : IParentClass
{
    public void DoChildStuff();
}
public class ChildClass : ParentClass, IChildClass
{
    protected new void Awake()
    {
        base.Awake();
        DoChildStuff();
    }

    public void DoChildStuff()
    {
        Debug.LogWarning(nameof(DoChildStuff));
    }
}