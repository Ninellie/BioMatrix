namespace Assets.Scripts.EntityComponents.Stats
{
    public static class StatFactory
    {
        public static OldStat GetStat(float baseValue)
        {
            return new OldStat(baseValue);
        }
    }
}