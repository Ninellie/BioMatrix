public interface IEffect
{
    string Name { get; }
    string TargetName { get; }
    void Attach(Entity target);
    void Detach();
    void Subscribe(Entity target);
    void Unsubscribe(Entity target);
}