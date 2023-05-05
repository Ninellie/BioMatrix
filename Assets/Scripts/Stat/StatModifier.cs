using System;

[Serializable]
public class StatModifier
{
    public StatModifier(OperationType type, float value, string statName) : this(type, value, false, 0, false, "", statName)
    {
    }
    public StatModifier(OperationType type, float value, string triggerName, string statName) : this(type, value, false, 0, true,
        triggerName, statName)
    {
    }
    public StatModifier(OperationType type, float value, float duration, string statName) : this(type, value, true, duration, false, "", statName)
    {
    }
    public StatModifier(OperationType type, float value, float duration, string triggerName, string statName) : this(type, value, true, duration, true, triggerName, statName)
    {
    }
    public StatModifier(OperationType type, float value, bool isTemporary, float duration, bool isTriggered, string triggerName, string statName)
    {
        Type = type;
        Value = value;
        IsTemporary = isTemporary;
        Duration = Math.Max(duration, 0);
        IsTriggered = isTriggered;
        TriggerName = triggerName;
        StatName = statName;
    }

    public OperationType Type { get; }
    public float Value { get; }
    public bool IsTemporary { get; }
    public float Duration { get; }
    public bool IsTriggered { get; }
    public string TriggerName { get; }
    public string StatName { get; }
}