namespace Assets.Scripts.EntityComponents.Effects
{
    public interface IEffectRepository
    {
        IOldEffect Get(string effectName);
    }
}