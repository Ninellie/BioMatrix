using System;

[Serializable]
public class FloatReference
{
    public bool useConstant;
    public float constantValue;
    public FloatVariable variable;

    public FloatReference()
    { }

    public FloatReference(float value)
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
                variable.value = value;
            }
        }
    }

    public static implicit operator float(FloatReference reference)
    {
        return reference.Value;
    }
}