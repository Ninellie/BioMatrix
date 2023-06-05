﻿namespace Assets.Scripts.Entity.Effects
{
    public interface IEffect
    {
        string Name { get; }
        string Description { get; }
        string TargetName { get; }
        string Identifier { get; set; }

        bool IsTemporal { get; }
        Stat.Stat Duration { get; }
        bool IsDurationStacks { get; } // Can be true only if IsTemporal and not IsStackSeparateDuration
        bool IsDurationUpdates { get; } // Can be true only if IsTemporal and not IsStackSeparateDuration

        bool IsStacking { get; }
        bool IsStackSeparateDuration { get; } // Can be true only if IsTemporal and IsStacking
        Resource StacksCount { get; }
        Stat.Stat MaxStackCount { get; }

        void Attach(Entity target);
        void Detach();
        void Subscribe(Entity target);
        void Unsubscribe(Entity target);
    }
}