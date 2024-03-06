using System;
using Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents;
using UnityEngine;

namespace Assets.Scripts.FirearmComponents
{
    [Serializable]
    public class AmmoData : MonoBehaviour
    {
        public ProjectilePool ammoPool;
        public int weight;
    }
}