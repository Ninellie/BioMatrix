using System.Collections.Generic;
using System.Linq;
using EntityComponents.UnitComponents.Movement;
using UnityEngine;

namespace FirearmComponents
{
    /// <summary>
    /// Выдаёт объекты снарядов по весовому алгоритму из массива боеприпасов
    /// </summary>
    public class Magazine : MonoBehaviour
    {
        public List<AmmoData> ammoData;

        public ProjectileMovementController Get()
        {
            var weightSum = ammoData.Sum(data => data.weight);
            var rand = Random.Range(0, weightSum);
            var limit = 0;

            foreach (var data in ammoData)
            {
                limit += data.weight;
                if (limit < rand) continue;
                return data.ammoPool.Get();
            }

            return null;
        }

        public void AddAmmoData(AmmoData ammo)
        {
            ammoData.Add(ammo);
        }

        public void AddAmmoData(IEnumerable<AmmoData> ammo)
        {
            ammoData.AddRange(ammo);
        }

        public void RemoveAmmoData(AmmoData ammo)
        {
            ammoData.Remove(ammo);
        }

        public void RemoveAmmoData(IEnumerable<AmmoData> ammo)
        {
            foreach (var data in ammo)
            {
                ammoData.Remove(data);
            }
        }
    }
}