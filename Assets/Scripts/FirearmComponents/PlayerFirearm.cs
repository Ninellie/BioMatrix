using Assets.Scripts.Core.Variables.References;
using Assets.Scripts.EntityComponents.UnitComponents.EnemyComponents;
using Assets.Scripts.EntityComponents.UnitComponents.Movement;
using Assets.Scripts.GameSession.UIScripts;
using UnityEngine;

namespace Assets.Scripts.FirearmComponents
{
    public class PlayerFirearm : MonoBehaviour
    {
        [SerializeField] private GameObject _ammo; // TODO convert to object pool: get disabled projectile and reuse it
        [SerializeField] private PlayerTargetRuntimeSet _visibleEnemies;
        [SerializeField] private AimMode _aimMode;
        [SerializeField] private MagazineReserve _magazineReserve;
        [SerializeField] private Vector2Reference _selfAimDirection;
        [Space]
        [Header("Stats")]
        [SerializeField] private FloatReference _attackSpeed;
        [SerializeField] private FloatReference _projectilesPerAttack;
        [SerializeField] private FloatReference _maxShootDeflectionAngle;
        public bool CanShoot => _coolDownTimer <= 0
                                && _magazineReserve.Value > 0
                                && !_magazineReserve.OnReload;
        private float _coolDownTimer;
        private PlayerTarget _currentTarget;
        private Transform _myTransform;

        private void Awake()
        {
            if (_myTransform == null) _myTransform = transform;
            if (_magazineReserve == null) _magazineReserve = GetComponent<MagazineReserve>();
        }

        private void FixedUpdate()
        {
            _coolDownTimer -= Time.fixedDeltaTime;
            UpdateTarget();
        }

        public void DoAction()
        {
            if (!CanShoot) return;
            Shoot();
        }

        public void SetAutoAim(bool value)
        {
            if (value)
            {
                _aimMode = AimMode.AutoAim;
                return;
            }
            _aimMode = AimMode.SelfAim;
        }

        private void Shoot()
        {
            _magazineReserve.Pop();

            var projectiles = GetProjectiles();
            var direction = GetShotDirection();
            var projSpread = _maxShootDeflectionAngle;
            var projCount = projectiles.Length;
            var fireAngle = projSpread * (projCount - 1);
            var halfFireAngleRad = fireAngle * 0.5f * Mathf.Deg2Rad;
            var leftDirection = MathFirearm.Rotate(direction, -halfFireAngleRad);
            var actualShotDirection = leftDirection;

            foreach (var projectile in projectiles)
            {
                var proj = projectile.GetComponent<ProjectileMovementController>();
                proj.SetDirection(actualShotDirection);
                var launchAngle = _maxShootDeflectionAngle * Mathf.Deg2Rad;
                actualShotDirection = MathFirearm.Rotate(actualShotDirection, launchAngle);
            }

            _coolDownTimer = 1f / _attackSpeed;
        }

        private GameObject[] GetProjectiles()
        {
            var projectiles = new GameObject[(int)_projectilesPerAttack];
            for (var i = 0; i < projectiles.Length; i++)
            {
                projectiles[i] = Instantiate(_ammo, _myTransform.position, _myTransform.rotation);
            }
            return projectiles;
        }

        private Vector2 GetShotDirection()
        {
            if (_aimMode == AimMode.SelfAim)
            {
                return _selfAimDirection.Value;
            }
            Vector2 direction = _currentTarget.Transform.position - _myTransform.position;
            direction.Normalize();
            return direction;
        }

        private void UpdateTarget()
        {
            if (_aimMode == AimMode.SelfAim)
            {
                if (_currentTarget == null) return;
                _currentTarget.RemoveFromTarget();
                _currentTarget = null;
                return;
            }
            var nearestTarget = GetNearestPlayerTarget();
            TakeAsTarget(nearestTarget);
        }

        private void TakeAsTarget(PlayerTarget target)
        {
            if (_currentTarget == target) return;
            _currentTarget.RemoveFromTarget();
            _currentTarget = target;
            _currentTarget.TakeAsTarget();
        }

        private PlayerTarget GetNearestPlayerTarget()
        {
            var distanceToNearestTarget = Mathf.Infinity;
            var targets = _visibleEnemies.items.ToArray();
            PlayerTarget nearestTarget = null;
            foreach (var target in targets)
            {
                var distance = Vector2.Distance(_myTransform.position, target.transform.position);
                if (!(distance < distanceToNearestTarget)) continue;
                distanceToNearestTarget = distance;
                nearestTarget = target;
            }
            return nearestTarget == null ? null : nearestTarget;
        }
    }
}