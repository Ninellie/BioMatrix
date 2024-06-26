using System.Collections;
using System.Collections.Generic;
using Core.Sets;
using Core.Variables.References;
using FirearmComponents;
using UnityEngine;

namespace EntityComponents.UnitComponents.PlayerComponents
{
    public class TurretShootDelaySetter : MonoBehaviour
    {
        public FloatReference turretAttackSpeed;
        public ShooterRuntimeSet turretShooters;
        public float timeBetweenShoots;


        private void OnDisable()
        {
            StopAllCoroutines();
        }

        public void ReShoot()
        {
            StopAllCoroutines();

            StartCoroutine(Co_Shoot());
        }

        public void Shoot()
        {
            StartCoroutine(Co_Shoot());
        }

        private IEnumerator Co_Shoot()
        {
            var shooterList = new List<Shooter>();

            foreach (var turretShootersItem in turretShooters.items)
            {
                shooterList.Add(turretShootersItem);
            }

            foreach (var shooter in shooterList)
            {
                timeBetweenShoots = 1f / turretAttackSpeed / turretShooters.items.Count;
                yield return new WaitForSeconds(timeBetweenShoots);
                shooter.Shoot();
            }
        }
    }
}