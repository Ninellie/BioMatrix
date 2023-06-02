namespace Assets.Scripts.Entity.Effects
{
    public interface IEffectRepository
    {
        IEffect Get(string effectName);
    }
}