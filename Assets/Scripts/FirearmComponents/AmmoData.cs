using System;
using Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents;

namespace Assets.Scripts.FirearmComponents
{
    [Serializable]
    public class AmmoData
    {
        public ProjectilePool ammoPool;
        public int weight;
    }
}