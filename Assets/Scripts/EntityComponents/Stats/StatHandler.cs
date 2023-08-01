using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.Stats
{
    public class StatHandler : MonoBehaviour
    {
        [SerializeField] private bool _usePreset;
        [SerializeField] private StatsPreset _preset;
        [SerializeField] private List<Stat> _stats;

        public void Awake()
        {
            if(!_usePreset) return;
            _stats.Clear();
            foreach (var statPresetData in _preset.stats)
            {
                var stat = new Stat();
                stat.SetSettings(_preset.settings);
                stat.SetBaseValue(statPresetData.baseValue);
                stat.SetName(statPresetData.name);
                _stats.Add(stat);
            }
        }

        public Stat GetStatByName(string statName)
        {
            return _stats.FirstOrDefault(stat => stat.Name.Equals(statName));
        }
    }
}