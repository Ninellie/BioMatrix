using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.Stats
{
    [Serializable]
    public class StatPresetData
    {
        public string name;
        public float baseValue;
    }

    [CreateAssetMenu]
    public class StatsPreset : ScriptableObject
    {
        [SerializeField]
        public List<StatPresetData> stats;

        [SerializeField]
        public StatSettings settings;
    }
}