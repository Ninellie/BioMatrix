using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.EntityComponents.UnitComponents.Movement;
using Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents;
using UnityEngine;

namespace Assets.Scripts.FirearmComponents
{
    /// <summary>
    /// Выдаёт случайный ProjectileMovementController по принципу суммы весов пулов снарядов из массива ammoData
    /// </summary>
    public class Magazine : MonoBehaviour
    {
        public List<AmmoData> ammoData;

        public ProjectileMovementController GetAmmo()
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

        public void AddAmmoData(ProjectilePool pool, int weight)
        {
            foreach (var currentData in ammoData.Where(currentData => currentData.ammoPool == pool))
            {
                currentData.weight = weight;
                return;
            }

            var data = new AmmoData()
            {
                ammoPool = pool,
                weight = weight
            };

            ammoData.Add(data);
        }
    }
}