public interface IEffect
{
    string Name { get; set; }
    string TargetName { get; set; }
    void Attach(Entity target);
    void Detach();
    void Subscribe(Entity target);
    void Unsubscribe(Entity target);
}