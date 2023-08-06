namespace Assets.Scripts.EntityComponents.Effects
{
    public interface IOldEffect
    {
        string Name { get; }
        string Description { get; }
        string TargetName { get; }
        string Identifier { get; set; }

        bool IsTemporal { get; }
        Stats.OldStat Duration { get; }
        bool IsDurationStacks { get; } // Can be true only if IsTemporal and not IsStackSeparateDuration
        bool IsDurationUpdates { get; } // Can be true only if IsTemporal and not IsStackSeparateDuration

        bool IsStacking { get; }
        bool IsStackSeparateDuration { get; } // Can be true only if IsTemporal and IsStacking
        OldResource StacksCount { get; }
        Stats.OldStat MaxStackCount { get; }

        void Attach(Entity target);
        void Detach();
        void Subscribe(Entity target);
        void Unsubscribe(Entity target);
    }
}