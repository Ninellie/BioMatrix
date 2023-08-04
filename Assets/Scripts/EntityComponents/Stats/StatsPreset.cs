using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.Stats
{
    [Serializable]
    public class StatPresetData
    {
        public StatName name;
        public float baseValue;
    }

    [CreateAssetMenu]
    public class StatsPreset : ScriptableObject
    {
        public List<StatPresetData> stats;
        public StatSettings settings;
    }
}