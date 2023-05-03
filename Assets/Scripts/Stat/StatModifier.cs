using System;

[Serializable]
public class StatModifier
{
    public StatModifier(OperationType type, float value)
    {
        Type = type;
        Value = value;
    }

    public float time;
    public bool isTemporary;
    public OperationType Type { get; }
    public float Value { get; }
}