using UnityEngine;

namespace Assets.Scripts.EntityComponents.Stats
{
    public static class StatFactory
    {
        public static Stat GetStat(float baseValue)
        {
            return new Stat(baseValue);
        }
    }
}