using Assets.Scripts.Core.Variables.References;
using Assets.Scripts.EntityComponents.UnitComponents.Movement;
using Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents;
using UnityEngine;

namespace Assets.Scripts.FirearmComponents
{
    public class Shooter : MonoBehaviour
    {
        [Header("Ammo")]
        [SerializeField] private ProjectilePool _ammoPool;
        [Space]
        [Header("Stats")]
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

        public void Shoot()
        {
            var shootDirection = Aim.GetDirection();
            var projCount = (int)_projectilesPerAttack;
            var fireAngle = _projectileSpread * (projCount - 1);
            var halfFireAngleRad = fireAngle * 0.5f * Mathf.Deg2Rad;
            var leftDirection = MathFirearm.Rotate(shootDirection, -halfFireAngleRad);
            var actualShotDirection = leftDirection;

            for (int i = 0; i < projCount; i++)
            {
                var projectile = _ammoPool.Get();
                projectile.transform.SetPositionAndRotation(Transform.position, Transform.rotation);
                //var projectileMovementController = projectile.GetComponent<ProjectileMovementController>();
                projectile.SetDirection(actualShotDirection);
                var launchAngle = _projectileSpread * Mathf.Deg2Rad;
                actualShotDirection = MathFirearm.Rotate(actualShotDirection, launchAngle);
            }
        }
    }
}