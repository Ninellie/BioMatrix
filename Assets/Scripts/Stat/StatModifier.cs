using System;

[Serializable]
public class StatModifier
{
    public StatModifier(OperationType type, float value) : this(type, value, 0)
    {
        IsTemporary = false;
    }
    public StatModifier(OperationType type, float value, float time)
    {
        Type = type;
        Value = value;
        IsTemporary = true;
        Time = time;
    }

    public OperationType Type { get; }
    public float Value { get; }
    public bool IsTemporary { get; }
    public float Time { get; }
}