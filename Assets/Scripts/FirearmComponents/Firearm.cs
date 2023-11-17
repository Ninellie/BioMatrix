using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Core.Variables.References;
using Assets.Scripts.EntityComponents.Resources;
using Assets.Scripts.EntityComponents.Stats;
using Assets.Scripts.EntityComponents.UnitComponents.EnemyComponents;
using Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents;
using Assets.Scripts.EntityComponents.UnitComponents.ProjectileComponents;
using Assets.Scripts.GameSession.UIScripts;
using UnityEngine;
using Random = UnityEngine.Random;
using Time = UnityEngine.Time;

namespace Assets.Scripts.FirearmComponents
{
    [RequireComponent(typeof(Reload))]
    public class Firearm : MonoBehaviour, ISource, IDerivative, ISlayer, IWeapon
    {
        [SerializeField] private GameObject _ammo;
        [SerializeField] private LayerMask _enemyLayer;
        public bool IsForPlayer { get; private set; }


        public FloatReference Damage;
        public FloatReference ShootForce;
        public FloatReference ShootsPerSecond;
        public FloatReference MaxShootDeflectionAngle;
        public FloatReference MagazineCapacity;
        public FloatReference ReloadSpeed;
        public FloatReference SingleShootProjectile;
        public FloatReference ProjectileSizeMultiplier;
        public FloatReference ProjectilePierceCount;
        public FloatReference AddedProjectileKnockback;
        public FloatReference TurretAimingRadius;
        
        public Resource Magazine { get; private set; }

        public event Action ReloadEndEvent;
        public event Action ReloadEvent;

        public bool IsEnable { get; set; } = true;
        public bool CanShoot => _previousShootTimer <= 0
                                && !Magazine.IsEmpty
                                && !Reload.IsInProcess;

        public Reload Reload { get; private set; }
        private Player _player;
        private float _previousShootTimer;
        private float MinShootInterval => 1f / ShootsPerSecond;

        private ISlayer _source;
        private ResourceList _resources;
        private StatList _stats;

        private PlayerTarget _currentEnemyTarget;

        public PlayerTarget GetCurrentTarget()
        {
            return _currentEnemyTarget;
        }

        private void Awake()
        {
            _stats = GetComponent<StatList>();
            _resources = GetComponent<ResourceList>();
            Reload = GetComponent<Reload>();

            _player = FindObjectOfType<Player>();
        }

        private void Start()
        {
            SetStats(_stats);
            Magazine = _resources.GetResource(ResourceName.Ammo);
            Magazine.Fill();
        }

        private void Update()
        {
            if (!IsEnable) return;
            _previousShootTimer -= Time.deltaTime;
        }

        private void FixedUpdate()
        {
            if (_player.aimMode == AimMode.AutoAim)
            {
                GetDirectionToNearestEnemy();
            }
        }

        private void OnDrawGizmos()
        {
            var stat = _stats.GetStat(StatName.TurretAimingRadius);
            if (stat == null) return;
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, stat.Value);
        }

        public StatList GetStatList() => _stats;

        public void SetStatList(StatList statList)
        {
            _stats = statList;
            SetStats(_stats);
        }

        public void OnReload() => ReloadEvent?.Invoke();

        public void OnReloadEnd() => ReloadEndEvent?.Invoke();

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

        public Resource GetAmmoResource() => _resources.GetResource(ResourceName.Ammo);
        public void DoAction()
        {
            if (CanShoot) Shoot();
        }

        private void SetStats(StatList firearmStats)
        {
            Damage = firearmStats.GetStat(StatName.Damage);
            ShootForce = firearmStats.GetStat(StatName.ShootForce);
            ShootsPerSecond = firearmStats.GetStat(StatName.ShootsPerSecond);
            MaxShootDeflectionAngle = firearmStats.GetStat(StatName.MaxShootDeflectionAngle);
            MagazineCapacity = firearmStats.GetStat(StatName.MagazineCapacity);
            ReloadSpeed = firearmStats.GetStat(StatName.ReloadSpeed);
            SingleShootProjectile = firearmStats.GetStat(StatName.Projectiles);
            ProjectileSizeMultiplier = firearmStats.GetStat(StatName.ProjectileSizeMultiplier);
            ProjectilePierceCount = firearmStats.GetStat(StatName.Pierce);
            AddedProjectileKnockback = firearmStats.GetStat(StatName.AddedProjectileKnockback);
            TurretAimingRadius = firearmStats.GetStat(StatName.TurretAimingRadius);
        }

        private void Shoot()
        {
            Magazine.Decrease();

            var projectiles = CreateProjectiles((int)SingleShootProjectile.Value, _ammo, gameObject.transform);
            var direction = GetShotDirection();
            var projSpread = _stats.GetStat(StatName.MaxShootDeflectionAngle).Value;
            var projCount = projectiles.Length;
            var fireAngle = projSpread * (projCount - 1);
            var halfFireAngleRad = fireAngle * 0.5f * Mathf.Deg2Rad;
            var leftDirection = MathFirearm.Rotate(direction, -halfFireAngleRad);
            var actualShotDirection = leftDirection;

            foreach (var projectile in projectiles)
            {
                var projStats = projectile.GetComponent<StatList>();
                var proj = projectile.GetComponent<Projectile>();

                ImproveProjectile(projStats);

                proj.SetSource(this);

                proj.Launch(actualShotDirection, ShootForce.Value);
                actualShotDirection = MathFirearm.Rotate(actualShotDirection, projSpread * Mathf.Deg2Rad);
            }

            _previousShootTimer = MinShootInterval;
        }

        public GameObject[] CreateProjectiles(int singleShotProjectiles, GameObject ammo, Transform firingPoint)
        {
            var projectiles = new GameObject[singleShotProjectiles];

            for (var i = 0; i < projectiles.Length; i++)
            {
                projectiles[i] = Instantiate(ammo, firingPoint.position, firingPoint.rotation);
            }
            return projectiles;
        }

        private void ImproveProjectile(StatList projectileStats)
        {
            var statMod = new StatMod(OperationType.Multiplication, ProjectileSizeMultiplier.Value);
            projectileStats.GetStat(StatName.Size).AddModifier(statMod);

            statMod = new StatMod(OperationType.Addition, ProjectilePierceCount.Value);
            projectileStats.GetStat(StatName.MaximumHealth).AddModifier(statMod);

            statMod = new StatMod(OperationType.Addition, Damage.Value);
            projectileStats.GetStat(StatName.Damage).AddModifier(statMod);

            statMod = new StatMod(OperationType.Addition, AddedProjectileKnockback.Value);
            projectileStats.GetStat(StatName.KnockbackPower).AddModifier(statMod);
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
            var collider2Ds = colliders as Collider2D[] ?? colliders.ToArray();
            Collider2D nearestCol = null;
            if (collider2Ds.Length > 0)
            {
                nearestCol = collider2Ds[0];
            }

            foreach (var col2D in collider2Ds)
            {
                var distance = Vector2.Distance(transform.position, col2D.transform.position);

                if (!(distance < distanceToNearestCollider)) continue;

                distanceToNearestCollider = distance;
                Vector2 direction = (col2D.transform.position - transform.position).normalized;
                directionToNearestCollider = direction;
                nearestCol = col2D;
            }

            if (!IsForPlayer || nearestCol == null) return directionToNearestCollider;

            var target = nearestCol.GetComponent<PlayerTarget>();

            if (_currentEnemyTarget == null)
            {
                _currentEnemyTarget = target;
            }
            else if (_currentEnemyTarget == target)
            {
                return directionToNearestCollider;
            }

            _currentEnemyTarget.RemoveFromTarget();
            _currentEnemyTarget = target;
            _currentEnemyTarget.TakeAsTarget();

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
    }
}