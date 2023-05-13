public interface IEffect
{
    string Name { get; }
    void Attach(Entity target);
    void Detach(Entity target);
    void Subscribe(Entity target);
    void Unsubscribe(Entity target);
}