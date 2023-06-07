using System;
using Assets.Scripts.Core;
using Assets.Scripts.Entity;
using Assets.Scripts.Entity.Stat;
using Assets.Scripts.Entity.Unit.Player;
using Assets.Scripts.Entity.Unit.Projectile;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Firearm
{
    [RequireComponent(typeof(FirearmStatsSettings))]
    [RequireComponent(typeof(Reload))]
    [RequireComponent(typeof(ProjectileCreator))]
    public class Firearm : MonoBehaviour
    {
        [SerializeField] private GameObject _ammo;
        [SerializeField] private bool _isForPlayer;
        [SerializeField] private LayerMask _enemyLayer;

        public FirearmStatsSettings Settings => GetComponent<FirearmStatsSettings>();
        public Resource Magazine { get; set; }

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

        public event Action ReloadEndEvent;
        public event Action ReloadEvent;

        public bool IsEnable { get; set; } = true;
        public bool CanShoot => _previousShootTimer <= 0
                                && !Magazine.IsEmpty
                                && !_reload.IsInProcess;

        private StatFactory _statFactory;
        private ProjectileCreator _projectileCreator;
        private Reload _reload;
        private Player _player;
        private bool IsFireButtonPressed => IsEnable && !_isForPlayer || _player.IsFireButtonPressed;
        private float _previousShootTimer = 0;
        private float MinShootInterval => 1f / ShootsPerSecond.Value;
    
        private void Awake() => BaseAwake(Settings);

        private void BaseAwake(FirearmStatsSettings settings)
        {
            _reload = GetComponent<Reload>();
            _projectileCreator = GetComponent<ProjectileCreator>();

            _statFactory = Camera.main.GetComponent<StatFactory>();

            Damage = _statFactory.GetStat(settings.damage);
            ShootForce = _statFactory.GetStat(settings.shootForce);
            ShootsPerSecond = _statFactory.GetStat(settings.shootsPerSecond);
            MaxShootDeflectionAngle = _statFactory.GetStat(settings.maxShootDeflectionAngle);
            MagazineCapacity = _statFactory.GetStat(settings.magazineCapacity);
            ReloadTime = _statFactory.GetStat(settings.reloadTime);
            SingleShootProjectile = _statFactory.GetStat(settings.singleShootProjectile);
            ProjectileSizeMultiplier = _statFactory.GetStat(settings.projectileSizeMultiplier);
            ProjectilePierceCount = _statFactory.GetStat(settings.projectilePierceCount);
            AddedProjectileKnockback = _statFactory.GetStat(settings.addedProjectileKnockback);
        
            Magazine = new Resource(0, MagazineCapacity);
            Magazine.Fill();
            _player = FindObjectOfType<Player>();
        }

        private void Update()
        {
            if (!IsEnable) return;
            _previousShootTimer -= Time.deltaTime;
            if (!IsFireButtonPressed) return;
            if (CanShoot) Shoot();
        }

        public void SetStats(Firearm firearm)
        {
            Damage = firearm.Damage;
            ShootForce = firearm.ShootForce;
            ShootsPerSecond = firearm.ShootsPerSecond;
            MaxShootDeflectionAngle = firearm.MaxShootDeflectionAngle;
            MagazineCapacity = firearm.MagazineCapacity;
            ReloadTime = firearm.ReloadTime;
            SingleShootProjectile = firearm.SingleShootProjectile;
            ProjectileSizeMultiplier = firearm.ProjectileSizeMultiplier;
            ProjectilePierceCount = firearm.ProjectilePierceCount;
        }
    
        public void OnReload()
        {
            ReloadEvent?.Invoke();
        }

        public void OnReloadEnd()
        {
            ReloadEndEvent?.Invoke();
        }

        public bool GetIsForPlayer()
        {
            return _isForPlayer;
        }
    
        private void Shoot()
        {
            Magazine.Decrease();
        
            var projectiles = _projectileCreator.CreateProjectiles((int)SingleShootProjectile.Value, _ammo, gameObject.transform);
        
            var direction = GetShotDirection();
        
            foreach (var projectile in projectiles)
            {
                var proj = projectile.GetComponent<Projectile>();

                ImproveProjectile(proj);

                var actualShotDirection = GetActualShotDirection(direction, MaxShootDeflectionAngle.Value);
                proj.Launch(actualShotDirection, ShootForce.Value);
            }

            _previousShootTimer = MinShootInterval;
        }

        private void ImproveProjectile(Projectile projectile)
        {
            var sizeMod = new StatModifier(OperationType.Multiplication, ProjectileSizeMultiplier.Value);
            projectile.Size.AddModifier(sizeMod);

            var pierceMod = new StatModifier(OperationType.Addition, ProjectilePierceCount.Value);
            if (pierceMod.Value > 0) projectile.MaximumLifePoints.AddModifier(pierceMod);
            projectile.LifePoints.Fill();

            var damageMod = new StatModifier(OperationType.Addition, Damage.Value);
            if (damageMod.Value > 0) projectile.Damage.AddModifier(damageMod);

            var knockbackMod = new StatModifier(OperationType.Addition, AddedProjectileKnockback.Value);
            if (knockbackMod.Value > 0) projectile.KnockbackPower.AddModifier(knockbackMod);

            var trail = projectile.gameObject.GetComponent<TrailRenderer>();
            trail.startWidth = projectile.GetComponent<CircleCollider2D>().radius * 2 * projectile.Size.Value;
        }

        private Vector2 GetShotDirection()
        {
            if (_isForPlayer || _player.TurretHub.IsSameTurretTarget)
            {
                return Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - gameObject.transform.position;
            }

            Camera mainCamera = Camera.main;
            Vector3 cameraPos = mainCamera.transform.position;
            Vector3 cameraTopRight =
                new Vector3(mainCamera.aspect * mainCamera.orthographicSize, mainCamera.orthographicSize, 0f) + cameraPos;
            Vector3 cameraBottomLeft =
                new Vector3(-mainCamera.aspect * mainCamera.orthographicSize, -mainCamera.orthographicSize, 0f) + cameraPos;
            Collider2D[] colliders = Physics2D.OverlapAreaAll(cameraBottomLeft, cameraTopRight, _enemyLayer);

            if (colliders.Length == 0)
            {
                return Random.insideUnitCircle;
            }

            Vector2 nearestEnemyDirection = Vector2.zero;
            float nearestDistance = Mathf.Infinity;

            foreach (Collider2D collider in colliders)
            {
                Vector2 direction = (collider.transform.position - transform.position).normalized;
                float distance = Vector2.Distance(transform.position, collider.transform.position);

                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestEnemyDirection = direction;
                }
            }
            return nearestEnemyDirection;
        }

        private Vector2 GetActualShotDirection(Vector2 direction, float maxShotDeflectionAngle)
        {
            var angleInRad = (float)Mathf.Deg2Rad * maxShotDeflectionAngle;
            var shotDeflectionAngle = Range(-angleInRad, angleInRad);
            return Rotate(direction, shotDeflectionAngle);
        }
    
        private float Range(float minInclusive, float maxInclusive)
        {
            var std = PeterAcklamInverseCDF.NormInv(UnityEngine.Random.value);
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