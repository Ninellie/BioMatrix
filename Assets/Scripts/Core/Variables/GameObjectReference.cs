using System;
using UnityEngine;

[Serializable]
public class GameObjectReference
{
    public bool useConstant;
    public GameObject constantValue;
    public GameObjectVariable variable;

    public GameObjectReference()
    { }

    public GameObjectReference(GameObject value)
    {
        useConstant = true;
        constantValue = value;
    }

    public GameObject Value => useConstant ? constantValue : variable.value;

    public static implicit operator GameObject(GameObjectReference reference)
    {
        return reference.Value;
    }
}