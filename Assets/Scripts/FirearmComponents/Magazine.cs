using System.Linq;
using Assets.Scripts.EntityComponents.UnitComponents.Movement;
using UnityEngine;

namespace Assets.Scripts.FirearmComponents
{
    public class Magazine : MonoBehaviour
    {
        public AmmoData[] ammoData;

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
    }
}