using System;

namespace Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents
{
    [Serializable]
    public class ShieldStatsSettings
    {
        public float maxLayers;
        public float maxRechargeableLayers;
        public float rechargeRatePerMinute;
        public float repulseRadius;
        public float repulseForce;
    }
}