using UnityEngine;

namespace Assets.Scripts.EntityComponents.Stats
{
    public class StatFactory : MonoBehaviour
    {
        public Stat GetStat(float baseValue)
        {
            return new Stat(baseValue);
        }
    }
}