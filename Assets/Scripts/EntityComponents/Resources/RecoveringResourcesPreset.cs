using System;
using System.Collections.Generic;
using Assets.Scripts.EntityComponents.Stats;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.Resources
{
    [Serializable]
    public class RecoveringResourceData
    {
        public ResourceName resourceName;
        public StatName recoverLimiterStatName;
        public StatName recoverSpeedStatName; // in resource value per second
    }

    [CreateAssetMenu]
    public class RecoveringResourcesPreset : ScriptableObject
    {
        public List<RecoveringResourceData> recoveringResourceDataList;
    }
}