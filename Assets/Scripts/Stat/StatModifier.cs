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
    public StatModifier(OperationType type, float value, float time) : this(type, value, true, time, false, "")
    {
    }
    public StatModifier(OperationType type, float value, float time, string triggerName) : this(type, value, true, time, true, triggerName)
    {
    }
    public StatModifier(OperationType type, float value, bool isTemporary, float time, bool isTriggered, string triggerName)
    {
        Type = type;
        Value = value;
        IsTemporary = isTemporary;
        if (time > 0)
        {
            Time = time;
        }
        else
        {
            Time = 0;
        }

        IsTriggered = isTriggered;
        TriggerName = triggerName;
    }

    public OperationType Type { get; }
    public float Value { get; }
    public bool IsTemporary { get; }
    public float Time { get; }
    public bool IsTriggered { get; }
    public string TriggerName { get; }
}