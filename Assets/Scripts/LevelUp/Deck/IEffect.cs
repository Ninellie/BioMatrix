public interface IEffect
{
    string Name { get; }
    string TargetName { get; }

    bool IsTemporal { get; }
    bool IsProlongable { get; } // Can be true if IsTemporal and not IsStackSeparateDuration
    bool IsUpdatable { get; } // Can be true if IsTemporal and not IsStackSeparateDuration
    Stat Duration { get; }
    string Identifier { get; set; }

    bool IsStacking { get; }
    bool IsStackSeparateDuration { get; } // Can be true if IsTemporal and IsStacking
    Resource StacksCount { get; }
    Stat MaxStackCount { get; }

    void Attach(Entity target);
    void Detach();
    void Subscribe(Entity target);
    void Unsubscribe(Entity target);
}