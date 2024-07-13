using System.Collections;
using Core.Sets;
using Core.Variables.References;
using UnityEngine;

namespace EntityComponents.UnitComponents.PlayerComponents
{
    public class TurretShootDelaySetter : MonoBehaviour
    {
        [SerializeField] private FloatReference turretAttackSpeed;
        [SerializeField] private ShooterRuntimeSet turretShooters;
        [SerializeField] private float timeBetweenShoots;
        
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
            foreach (var shooter in turretShooters.items)
            {
                timeBetweenShoots = 1f / turretAttackSpeed / turretShooters.items.Count;
                yield return new WaitForSeconds(timeBetweenShoots);
                shooter.Shoot();
            }
        }
    }
}