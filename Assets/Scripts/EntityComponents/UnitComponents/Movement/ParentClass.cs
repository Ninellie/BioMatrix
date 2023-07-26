using UnityEngine;

public interface IParentClass
{
    public void ParentStuff();
}
public class ParentClass : MonoBehaviour, IParentClass
{
    protected void Awake()
    {
        ParentStuff();
    }

    public void ParentStuff()
    {
        Debug.LogWarning(nameof(ParentStuff));
    }
}