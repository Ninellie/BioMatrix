using System.Collections;
using Assets.Scripts.Core.Variables.References;
using Assets.Scripts.EntityComponents.UnitComponents.Movement;
using UnityEngine;

namespace Assets.Scripts.FirearmComponents
{
    public class TurretFirearm : MonoBehaviour
    {
        [Header("Ammo")]
        [SerializeField] private GameObject _ammo;
        [Space]
        [Header("Stats")]
        [SerializeField] private FloatReference _attackSpeed;
        [SerializeField] private FloatReference _projectilesPerAttack;
        [SerializeField] private FloatReference _projectileSpread;

        private Transform Transform
        {
            get
            {
                if (_transform != null) return _transform;
                _transform = transform;
                return _transform;
            }
        }
        private Aim Aim
        {
            get
            {
                if(_aim != null) return _aim;
                _aim = GetComponent<Aim>();
                return _aim;
            }
        }

        private Transform _transform;
        private Aim _aim;

        private void OnEnable()
        {
            StartCoroutine(Co_Shoot());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private IEnumerator Co_Shoot()
        {
            while (true)
            {
                yield return new WaitForSeconds(1f / _attackSpeed);
                var projectiles = GetProjectiles();
                var shootDirection = Aim.GetDirection();
                var projCount = projectiles.Length;
                var fireAngle = _projectileSpread * (projCount - 1);
                var halfFireAngleRad = fireAngle * 0.5f * Mathf.Deg2Rad;
                var leftDirection = MathFirearm.Rotate(shootDirection, -halfFireAngleRad);
                var actualShotDirection = leftDirection;

                foreach (var projectile in projectiles)
                {
                    var proj = projectile.GetComponent<ProjectileMovementController>();
                    proj.SetDirection(actualShotDirection);
                    var launchAngle = _projectileSpread * Mathf.Deg2Rad;
                    actualShotDirection = MathFirearm.Rotate(actualShotDirection, launchAngle);
                }
            }
        }

        private GameObject[] GetProjectiles()
        {
            var projectiles = new GameObject[(int)_projectilesPerAttack];
            for (var i = 0; i < projectiles.Length; i++)
            {
                projectiles[i] = Instantiate(_ammo, Transform.position, Transform.rotation);
            }
            return projectiles;
        }
    }
}