using System;
using System.Collections.Generic;
using Assets.Scripts.EntityComponents.Stats;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.Resources
{
    [Serializable]
    public class ResourcePresetData
    {
        [HideInInspector]
        public string stringName;
        public ResourceName name;
        public StatName maxValueStatName;
        public int baseValue;
        public int minValue;
        public int edgeValue;
    }

    [CreateAssetMenu]
    public class ResourcePreset : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField]
        public List<ResourcePresetData> resources;

        public void OnBeforeSerialize() { }
        public void OnAfterDeserialize()
        {
            foreach (var resourcePresetData in resources)
            {
                resourcePresetData.stringName = resourcePresetData.name.ToString();
            }
        }
    }
}