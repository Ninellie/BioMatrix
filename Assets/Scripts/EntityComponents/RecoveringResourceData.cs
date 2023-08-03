using System;
using UnityEngine;

namespace Assets.Scripts.EntityComponents
{
    [Serializable]
    public class RecoveringResourceData
    {
        public string resourceName;
        public StatName recoverLimiterStatName;
        public StatName recoverSpeedStatName; // in resource value per second
    }
}