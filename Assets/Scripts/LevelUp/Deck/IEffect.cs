public interface IEffect
{
    string Name { get; }
    string TargetName { get; }
    bool IsTemporal { get; }
    bool IsProlongable { get; }
    bool IsStackable { get; }
    bool IsUpdatable { get; }
    //bool IsDynamic { get; }
    void Attach(Entity target);
    void Detach();
    void Subscribe(Entity target);
    void Unsubscribe(Entity target);
}