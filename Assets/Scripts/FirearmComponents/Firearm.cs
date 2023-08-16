using System;
using System.Collections.Generic;
using Assets.Scripts.Core;
using Assets.Scripts.EntityComponents;
using Assets.Scripts.EntityComponents.Resources;
using Assets.Scripts.EntityComponents.Stats;
using Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents;
using Assets.Scripts.EntityComponents.UnitComponents.ProjectileComponents;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.FirearmComponents
{
    [RequireComponent(typeof(Reload))]
    [RequireComponent(typeof(ProjectileCreator))]
    public class Firearm : MonoBehaviour
    {
        [SerializeField] private GameObject _ammo;
        [SerializeField] private LayerMask _enemyLayer;
        //[SerializeField] private Vector2 _currentFireDirection;
        [SerializeField] private bool _isAimHelperActive;

        private void OnDrawGizmos()
        {
            var stat = _statList.GetStatByName(StatName.TurretAimingRadius);
            if (stat == null) return;
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, stat.Value);
        }

        public Entity Holder { get; private set; }
        //public FirearmStatsSettings Settings => GetComponent<FirearmStatsSettings>();
        //public OldResource Magazine { get; set; }
        
        public bool IsForPlayer { get; private set; }

        //public OldStat Damage { get; private set; }
        //public OldStat ShootForce { get; private set; }
        //public OldStat ShootsPerSecond { get; private set; }
        //public OldStat MaxShootDeflectionAngle { get; private set; }
        //public OldStat MagazineCapacity { get; private set; }
        //public OldStat ReloadTime { get; private set; }
        //public OldStat SingleShootProjectile { get; private set; }
        //public OldStat ProjectileSizeMultiplier { get; private set; }
        //public OldStat ProjectilePierceCount { get; private set; }
        //public OldStat AddedProjectileKnockback { get; private set; }
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

        private ResourceList _resourceList;
        public Resource Magazine { get; private set; }

        private StatList _statList;

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
        //private float MinShootInterval => 1f / ShootsPerSecond.Value;
        private float MinShootInterval => 1f / ShootsPerSecond.Value;
    

        //private void Awake() => BaseAwake(Settings);
        private void Awake() => BaseAwake();
            
        private void Update()
        {
            if (!IsEnable) return;
            _previousShootTimer -= Time.deltaTime;
            if (!IsFireButtonPressed) return;
            if (CanShoot) Shoot();
        }

        //private void BaseAwake(FirearmStatsSettings settings)
        private void BaseAwake()
        {
            _reload = GetComponent<Reload>();
            _projectileCreator = GetComponent<ProjectileCreator>();

            _statList = GetComponent<StatList>();
            _resourceList = GetComponent<ResourceList>();

            SetStats(_statList);

            //Damage = StatFactory.GetStat(settings.damage);
            //ShootForce = StatFactory.GetStat(settings.shootForce);
            //ShootsPerSecond = StatFactory.GetStat(settings.shootsPerSecond);
            //MaxShootDeflectionAngle = StatFactory.GetStat(settings.maxShootDeflectionAngle);
            //MagazineCapacity = StatFactory.GetStat(settings.magazineCapacity);
            //ReloadTime = StatFactory.GetStat(settings.reloadTime);
            //SingleShootProjectile = StatFactory.GetStat(settings.singleShootProjectile);
            //ProjectileSizeMultiplier = StatFactory.GetStat(settings.projectileSizeMultiplier);
            //ProjectilePierceCount = StatFactory.GetStat(settings.projectilePierceCount);
            //AddedProjectileKnockback = StatFactory.GetStat(settings.addedProjectileKnockback);
        
            //Magazine = new OldResource(0, MagazineCapacity);
            Magazine = _resourceList.GetResourceByName(ResourceName.Ammo);
            Magazine.Fill();
            _player = FindObjectOfType<Player>();
        }

        public StatList GetStatList()
        {
            return _statList;
        }

        public void SetStatList(StatList statList)
        {
            _statList = statList;
            SetStats(_statList);
        }

        private void SetStats(StatList firearmStats)
        {
            Damage = firearmStats.GetStatByName(StatName.Damage);
            ShootForce = firearmStats.GetStatByName(StatName.ShootForce);
            ShootsPerSecond = firearmStats.GetStatByName(StatName.ShootsPerSecond);
            MaxShootDeflectionAngle = firearmStats.GetStatByName(StatName.MaxShootDeflectionAngle);
            MagazineCapacity = firearmStats.GetStatByName(StatName.MagazineCapacity);
            ReloadTime = firearmStats.GetStatByName(StatName.ReloadTime);
            SingleShootProjectile = firearmStats.GetStatByName(StatName.Projectiles);
            ProjectileSizeMultiplier = firearmStats.GetStatByName(StatName.ProjectileSizeMultiplier);
            ProjectilePierceCount = firearmStats.GetStatByName(StatName.Pierce);
            AddedProjectileKnockback = firearmStats.GetStatByName(StatName.AddedProjectileKnockback);
            TurretAimingRadius = firearmStats.GetStatByName(StatName.TurretAimingRadius);
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
    
        private void Shoot()
        {
            Magazine.Decrease();
        
            var projectiles = _projectileCreator.CreateProjectiles((int)SingleShootProjectile.Value, _ammo, gameObject.transform);
        
            var direction = GetShotDirection();
        
            foreach (var projectile in projectiles)
            {
                var proj = projectile.GetComponent<Projectile>();

                ImproveProjectile(proj);

                proj.SetSource(Holder);

                var actualShotDirection = GetActualShotDirection(direction, MaxShootDeflectionAngle.Value);
                proj.Launch(actualShotDirection, ShootForce.Value);
            }

            _previousShootTimer = MinShootInterval;
        }

        private void ImproveProjectile(Projectile projectile)
        {
            var statMod = new StatModifier(OperationType.Multiplication, ProjectileSizeMultiplier.Value);
            projectile.Size.AddModifier(statMod);

            statMod = new StatModifier(OperationType.Addition, ProjectilePierceCount.Value);
            if (statMod.Value > 0) projectile.MaximumLifePoints.AddModifier(statMod);
            projectile.LifePoints.Fill();

            statMod = new StatModifier(OperationType.Addition, Damage.Value);
            if (statMod.Value > 0) projectile.Damage.AddModifier(statMod);

            statMod = new StatModifier(OperationType.Addition, AddedProjectileKnockback.Value);
            if (statMod.Value > 0) projectile.KnockbackPower.AddModifier(statMod);
        }

        public void SetHolder(Entity entity)
        {
            Holder = entity;
            IsForPlayer = entity is Player;
        }

        private Vector2 GetShotDirection()
        {
            return IsForPlayer ? GetShotDirectionForPlayer() : GetShotDirectionForTurret();
        }

        private Vector2 GetShotDirectionForTurret()
        {
            var aimingRadius = _statList.GetStatByName(StatName.TurretAimingRadius).Value;
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

        private Collider2D[] GetEnemyCollidersInCameraBounds()
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