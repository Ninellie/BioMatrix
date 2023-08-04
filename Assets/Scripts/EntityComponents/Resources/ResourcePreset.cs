using System;
using System.Collections.Generic;
using Assets.Scripts.EntityComponents.Stats;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.Resources
{
    [Serializable]
    public class ResourcePresetData
    {
        public StatName maxValueStatName;
        public ResourceName name;
        public int baseValue;
        public int minValue;
        public int edgeValue;
    }

    [CreateAssetMenu]
    public class ResourcePreset : ScriptableObject
    {
        [SerializeField]
        public List<ResourcePresetData> resources;
    }
}