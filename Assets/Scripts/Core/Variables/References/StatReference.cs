using System;

[Serializable]
public class StatReference
{
    public bool useConstant;
    public float constantValue;
    public StatVariable variable;

    public StatReference()
    { }

    public StatReference(float value)
    {
        useConstant = true;
        constantValue = value;
    }

    public float Value
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
                variable.SetValue(value);
            }
        }
    }

    public static implicit operator float(StatReference reference)
    {
        return reference.Value;
    }
}