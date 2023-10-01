using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.Stats
{
    [Serializable]
    public class StatPresetData
    {
        [HideInInspector]
        public string stringName;
        public StatName name;
        public float baseValue;
    }

    [CreateAssetMenu]
    public class StatsPreset : ScriptableObject, ISerializationCallbackReceiver
    {
        public List<StatPresetData> stats;
        public StatSettings settings;

        public void OnBeforeSerialize() { }
        public void OnAfterDeserialize()
        {
            foreach (var statPresetData in stats)
            {
                statPresetData.stringName = statPresetData.name.ToString();
            }
        }
    }
}