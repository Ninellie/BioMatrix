using System;

namespace Assets.Scripts.Entity.Stats
{
    [Serializable]
    public class StatModifier
    {
        public StatModifier(OperationType type, float value) : this(type, value, false, 0)
        {
        }
        public StatModifier(OperationType type, float value, float duration) : this(type, value, true, duration)
        {
        }
        public StatModifier(OperationType type, float value, bool isTemporary, float duration)
        {
            Type = type;
            Value = value;
            IsTemporary = isTemporary;
            Duration = Math.Max(duration, 0);
        }

        public OperationType Type { get; }
        public float Value { get; }
        public bool IsTemporary { get; }
        public float Duration { get; }
    }
}