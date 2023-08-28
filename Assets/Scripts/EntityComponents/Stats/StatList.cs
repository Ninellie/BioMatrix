using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.Stats
{
    [AddComponentMenu("Entity/StatList")]
    public class StatList : MonoBehaviour, ISerializationCallbackReceiver
    {
        [SerializeField] private bool _usePreset;
        [SerializeField] private StatsPreset _preset;
        [SerializeField] private List<Stat> _stats;

        public void Awake()
        {
            if(!_usePreset) return;
            FillListFromPreset();
        }
        // ???
        public float GetPresetStatValue(StatName statName)
        {
            return _preset.stats.FirstOrDefault(stat => stat.name.Equals(statName))!.baseValue;
        }

        public Stat GetStat(StatName statName)
        {
            return _stats.FirstOrDefault(stat => stat.Name.Equals(statName));
        }

        [ContextMenu("Read Preset")]
        private void FillListFromPreset()
        {
            _stats.Clear();
            foreach (var statPresetData in _preset.stats)
            {
                var stat = new Stat();
                stat.SetSettings(_preset.settings);
                stat.SetBaseValue(statPresetData.baseValue);
                stat.SetName(statPresetData.name);
                //stat._strName = $"{statPresetData.name}: {stat.Value}";
                //stat._strName = statPresetData.name.ToString();
                _stats.Add(stat);
            }
        }

        public void OnBeforeSerialize()
        {
            if (_stats is null)
                return;

            if (_stats.Count == 0)
                return;

            foreach (var stat in _stats)
            {
                stat._strName = $"{stat.Name}: {stat.Value}";
            }
        }

        public void OnAfterDeserialize()
        {
            foreach (var stat in _stats)
            {
                stat._strName = $"{stat.Name}: {stat.Value}";
            }
        }
    }
}