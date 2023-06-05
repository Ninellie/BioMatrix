using UnityEngine;

namespace Assets.Scripts.Entity.Stat
{
    public class StatFactory : MonoBehaviour
    {
        public Stat GetStat(float baseValue)
        {
            return new Stat(baseValue);
        }
    }
}