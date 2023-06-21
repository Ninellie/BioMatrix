using UnityEngine;

namespace Assets.Scripts.Entity.Stats
{
    public class StatFactory : MonoBehaviour
    {
        public Stat GetStat(float baseValue)
        {
            return new Stat(baseValue);
        }
    }
}