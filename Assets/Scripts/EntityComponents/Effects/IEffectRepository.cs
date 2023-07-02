namespace Assets.Scripts.EntityComponents.Effects
{
    public interface IEffectRepository
    {
        IEffect Get(string effectName);
    }
}