using System;

[Serializable]
public class IntReference
{
    public bool useConstant;
    public int constantValue;
    public IntVariable variable;

    public IntReference()
    { }

    public IntReference(int value)
    {
        useConstant = true;
        constantValue = value;
    }

    public int Value
    {
        get => useConstant ? constantValue : variable.value;
        set
        {
            if (useConstant)
            {
                constantValue = value;
            }
            else
            {
                variable.value = value;
            }
        }
    }

    public static implicit operator int(IntReference reference)
    {
        return reference.Value;
    }
}