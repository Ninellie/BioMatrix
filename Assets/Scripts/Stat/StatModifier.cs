using System;

[Serializable]
public class StatModifier
{
    public StatModifier(OperationType type, float value) : this(type, value, false, 0, false, "")
    {
    }
    public StatModifier(OperationType type, float value, string triggerName) : this(type, value, false, 0, true,
        triggerName)
    {
    }
    public StatModifier(OperationType type, float value, float duration) : this(type, value, true, duration, false, "")
    {
    }
    public StatModifier(OperationType type, float value, float duration, string triggerName) : this(type, value, true, duration, true, triggerName)
    {
    }
    public StatModifier(OperationType type, float value, bool isTemporary, float duration, bool isTriggered, string triggerName)
    {
        Type = type;
        Value = value;
        IsTemporary = isTemporary;
        Duration = Math.Max(duration, 0);
        IsTriggered = isTriggered;
        TriggerName = triggerName;
    }

    public OperationType Type { get; }
    public float Value { get; }
    public bool IsTemporary { get; }
    public float Duration { get; }
    public bool IsTriggered { get; }
    public string TriggerName { get; }
}