using System;
using EntityComponents.UnitComponents.PlayerComponents;

namespace FirearmComponents
{
    /// <summary>
    /// Содержит данные о снаряде для магазина. Пул снарядов, а также вес выдачи в магазине.
    /// </summary>
    [Serializable]
    public class AmmoData
    {
        public ProjectilePool ammoPool;
        public int weight;
    }
}