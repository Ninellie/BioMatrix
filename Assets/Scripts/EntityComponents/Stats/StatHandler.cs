using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.Stats
{
    public class StatHandler
    {
        [SerializeField] private bool _usePreset;
        [SerializeField] private StatsPreset _preset;
        [SerializeField] private List<EntityStat> _stats;

        public EntityStat GetStatByName(string statName)
        {
            return _stats.FirstOrDefault(stat => stat.Name.Equals(statName));
        }
    }
}