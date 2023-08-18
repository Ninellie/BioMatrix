using System;
using System.Collections.Generic;
using Assets.Scripts.Core;
using Assets.Scripts.EntityComponents.Resources;
using Assets.Scripts.EntityComponents.Stats;
using Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents;
using Assets.Scripts.EntityComponents.UnitComponents.ProjectileComponents;
using UnityEngine;
using Random = UnityEngine.Random;

public interface IWeapon
{
    Resource GetAmmoResource();
}


namespace Assets.Scripts.FirearmComponents
{
    [RequireComponent(typeof(Reload))]
    [RequireComponent(typeof(ProjectileCreator))]
    public class Firearm : MonoBehaviour, ISource, IDerivative, ISlayer, IWeapon
    {
        [SerializeField] private GameObject _ammo;
        [SerializeField] private LayerMask _enemyLayer;
        [SerializeField] private bool _isAimHelperActive;
        //[SerializeField] private Vector2 _currentFireDirection;

        private void OnDrawGizmos()
        {
            var stat = _stats.GetStat(StatName.TurretAimingRadius);
            if (stat == null) return;
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, stat.Value);
        }

        public bool IsForPlayer { get; private set; }
        public Stat Damage { get; private set; }
        public Stat ShootForce { get; private set; }
        public Stat ShootsPerSecond { get; private set; }
        public Stat MaxShootDeflectionAngle { get; private set; }
        public Stat MagazineCapacity { get; private set; }
        public Stat ReloadTime { get; private set; }
        public Stat SingleShootProjectile { get; private set; }
        public Stat ProjectileSizeMultiplier { get; private set; }
        public Stat ProjectilePierceCount { get; private set; }
        public Stat AddedProjectileKnockback { get; private set; }
        public Stat TurretAimingRadius { get; private set; }

        public Resource Magazine { get; private set; }

        public event Action ReloadEndEvent;
        public event Action ReloadEvent;

        public bool IsEnable { get; set; } = true;
        public bool CanShoot => _previousShootTimer <= 0
                                && !Magazine.IsEmpty
                                && !_reload.IsInProcess;

        private ProjectileCreator _projectileCreator;
        private Reload _reload;
        private Player _player;
        private bool IsFireButtonPressed => IsEnable && !IsForPlayer || _player.IsFireButtonPressed;
        private float _previousShootTimer;
        private float MinShootInterval => 1f / ShootsPerSecond.Value;

        private ISlayer _source;
        private ResourceList _resources;
        private StatList _stats;

        private void Awake()
        {
            _stats = GetComponent<StatList>();
            _resources = GetComponent<ResourceList>();
            _reload = GetComponent<Reload>();
            _projectileCreator = GetComponent<ProjectileCreator>();

            _player = FindObjectOfType<Player>();

            SetStats(_stats);

            Magazine = _resources.GetResource(ResourceName.Ammo);
            Magazine.Fill();
        }

        private void Update()
        {
            if (!IsEnable) return;
            _previousShootTimer -= Time.deltaTime;
            if (!IsFireButtonPressed) return;
            if (CanShoot) Shoot();
        }

        public StatList GetStatList()
        {
            return _stats;
        }

        public void SetStatList(StatList statList)
        {
            _stats = statList;
            SetStats(_stats);
        }

        public void OnReload()
        {
            ReloadEvent?.Invoke();
        }

        public void OnReloadEnd()
        {
            ReloadEndEvent?.Invoke();
        }

        //public void SetDirection(Vector2 direction)
        //{
        //    _currentFireDirection = direction;
        //}

        public void SetSource(ISource source)
        {
            IsForPlayer = source is IPlayableCharacter;
            if (source is ISlayer slayer) _source = slayer;
        }

        public void IncreaseKills()
        {
            _resources.GetResource(ResourceName.Kills).Increase();
            _source.IncreaseKills();
        }

        public Resource GetAmmoResource()
        {
            return  _resources.GetResource(ResourceName.Ammo);
        }

        private void SetStats(StatList firearmStats)
        {
            Damage = firearmStats.GetStat(StatName.Damage);
            ShootForce = firearmStats.GetStat(StatName.ShootForce);
            ShootsPerSecond = firearmStats.GetStat(StatName.ShootsPerSecond);
            MaxShootDeflectionAngle = firearmStats.GetStat(StatName.MaxShootDeflectionAngle);
            MagazineCapacity = firearmStats.GetStat(StatName.MagazineCapacity);
            ReloadTime = firearmStats.GetStat(StatName.ReloadTime);
            SingleShootProjectile = firearmStats.GetStat(StatName.Projectiles);
            ProjectileSizeMultiplier = firearmStats.GetStat(StatName.ProjectileSizeMultiplier);
            ProjectilePierceCount = firearmStats.GetStat(StatName.Pierce);
            AddedProjectileKnockback = firearmStats.GetStat(StatName.AddedProjectileKnockback);
            TurretAimingRadius = firearmStats.GetStat(StatName.TurretAimingRadius);
        }

        private void Shoot()
        {
            Magazine.Decrease();

            var projectiles = _projectileCreator.CreateProjectiles((int)SingleShootProjectile.Value, _ammo, gameObject.transform);

            var direction = GetShotDirection();

            foreach (var projectile in projectiles)
            {
                var projStats = projectile.GetComponent<StatList>();
                var projResources = projectile.GetComponent<ResourceList>();

                var proj = projectile.GetComponent<Projectile>();

                ImproveProjectile(projStats, projResources);

                proj.SetSource(this);

                var actualShotDirection = GetActualShotDirection(direction, MaxShootDeflectionAngle.Value);
                proj.Launch(actualShotDirection, ShootForce.Value);
            }

            _previousShootTimer = MinShootInterval;
        }

        private void ImproveProjectile(StatList projectileStats, ResourceList projectileResources)
        {
            var statMod = new StatMod(OperationType.Multiplication, ProjectileSizeMultiplier.Value);
            projectileStats.GetStat(StatName.Size).AddModifier(statMod);

            statMod = new StatMod(OperationType.Addition, ProjectilePierceCount.Value);
            projectileStats.GetStat(StatName.MaximumHealth).AddModifier(statMod);

            statMod = new StatMod(OperationType.Addition, Damage.Value);
            projectileStats.GetStat(StatName.Damage).AddModifier(statMod);

            statMod = new StatMod(OperationType.Addition, AddedProjectileKnockback.Value);
            projectileStats.GetStat(StatName.KnockbackPower).AddModifier(statMod);

            projectileResources.GetResource(ResourceName.Health).Fill();
        }

        private Vector2 GetShotDirection()
        {
            return IsForPlayer ? GetShotDirectionForPlayer() : GetShotDirectionForTurret();
        }

        private Vector2 GetShotDirectionForTurret()
        {
            var aimingRadius = _stats.GetStat(StatName.TurretAimingRadius).Value;
            var direction = GetDirectionToNearestEnemy(aimingRadius);

            return direction.Equals(Vector2.zero) ? Random.insideUnitCircle : direction;
        }

        private Vector2 GetShotDirectionForPlayer()
        {
            var playerAimDirection = _player.CurrentAimDirection;
            var isAiming = !playerAimDirection.Equals(Vector2.zero);

            return isAiming switch
            {
                true => playerAimDirection,
                false => TryGetDirectionToNearestEnemy(out playerAimDirection) ? playerAimDirection : Random.insideUnitCircle
            };
        }

        private bool TryGetDirectionToNearestEnemy(out Vector2 direction)
        {
            var directionToNearestEnemy = GetDirectionToNearestEnemy();
            var isNearestEnemyExists = !directionToNearestEnemy.Equals(Vector2.zero);
            direction = directionToNearestEnemy;
            return isNearestEnemyExists;
        }

        private Vector2 GetDirectionToNearestEnemy(float searchRadius)
        {
            var collidersInAimingRadius = Physics2D.OverlapCircleAll(transform.position, searchRadius, _enemyLayer);
            var directionToNearestEnemy = GetDirectionToNearestCollider(collidersInAimingRadius);
            return directionToNearestEnemy;
        }

        private Vector2 GetDirectionToNearestEnemy()
        {
            var nearestEnemies = GetEnemyCollidersInCameraBounds();
            var directionToNearestEnemy = GetDirectionToNearestCollider(nearestEnemies);
            return directionToNearestEnemy;
        }

        private Vector2 GetDirectionToNearestCollider(IEnumerable<Collider2D> colliders)
        {
            var directionToNearestCollider = Vector2.zero;
            var distanceToNearestCollider = Mathf.Infinity;

            foreach (var col2D in colliders)
            {
                var distance = Vector2.Distance(transform.position, col2D.transform.position);

                if (!(distance < distanceToNearestCollider)) continue;

                distanceToNearestCollider = distance;
                Vector2 direction = (col2D.transform.position - transform.position).normalized;
                directionToNearestCollider = direction;
            }
            return directionToNearestCollider;
        }

        private IEnumerable<Collider2D> GetEnemyCollidersInCameraBounds()
        {
            var mainCamera = Camera.main;
            var cameraPos = mainCamera.transform.position;
            var cameraTopRight = new Vector3(mainCamera.aspect * mainCamera.orthographicSize, mainCamera.orthographicSize, 0f) + cameraPos;
            var cameraBottomLeft = new Vector3(-mainCamera.aspect * mainCamera.orthographicSize, -mainCamera.orthographicSize, 0f) + cameraPos;
            var collidersInCameraBounds = Physics2D.OverlapAreaAll(cameraBottomLeft, cameraTopRight, _enemyLayer);
            return collidersInCameraBounds;
        }

        private Vector2 GetActualShotDirection(Vector2 direction, float maxShotDeflectionAngle)
        {
            var angleInRad = Mathf.Deg2Rad * maxShotDeflectionAngle;
            var shotDeflectionAngle = Range(-angleInRad, angleInRad);
            return Rotate(direction, shotDeflectionAngle);
        }
    
        private float Range(float minInclusive, float maxInclusive)
        {
            var std = PeterAcklamInverseCDF.NormInv(Random.value);
            return PeterAcklamInverseCDF.RandomGaussian(std, minInclusive, maxInclusive);
        }
    
        private Vector2 Rotate(Vector2 point, float angle)
        {
            Vector2 rotatedPoint;
            rotatedPoint.x = point.x * Mathf.Cos(angle) - point.y * Mathf.Sin(angle);
            rotatedPoint.y = point.x * Mathf.Sin(angle) + point.y * Mathf.Cos(angle);
            return rotatedPoint;
        }
    }
}