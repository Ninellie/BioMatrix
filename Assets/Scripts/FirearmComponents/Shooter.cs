using Assets.Scripts.Core.Variables.References;
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
        [SerializeField] private bool _roundShoot;
        [SerializeField] private bool _randomAimDirection;

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

        private Vector2 _shootDirection;

        public void Shoot(int projectileNumber)
        {
            _shootDirection = _randomAimDirection ? Random.onUnitSphere : Aim.GetDirection();
            var projSpread = _roundShoot ? 360f : _projectileSpread.Value;
            var fireAngle = projSpread * (projectileNumber - 1);
            var halfFireAngleRad = fireAngle * 0.5f * Mathf.Deg2Rad;
            var leftDirection = MathFirearm.Rotate(_shootDirection, -halfFireAngleRad);
            var actualShotDirection = leftDirection;
            var launchAngle = _roundShoot ? 360f / projectileNumber : _projectileSpread;
            launchAngle *= Mathf.Deg2Rad;

            for (int i = 0; i < projectileNumber; i++)
            {
                var projectile = _ammoPool.Get();
                projectile.transform.SetPositionAndRotation(Transform.position, Transform.rotation);
                projectile.Trail.Clear();
                projectile.SetDirection(actualShotDirection);
                actualShotDirection = MathFirearm.Rotate(actualShotDirection, launchAngle);
            }
        }

        public void Shoot()
        {
            Shoot(_projectilesPerAttack);
        }
    }
}