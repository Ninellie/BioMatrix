using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Core.Variables.References;
using Assets.Scripts.EntityComponents.Resources;
using Assets.Scripts.EntityComponents.Stats;
using Assets.Scripts.EntityComponents.UnitComponents.ProjectileComponents;
using UnityEngine;

namespace Assets.Scripts.FirearmComponents
{
    public class BombardierFirearm : MonoBehaviour, IAbility
    {
        [Header("Ammo prefab")]
        [SerializeField] private GameObject _ammo;
        [Space]
        [Header("Inner References")]
        [SerializeField] private ResourceList _resourceList;
        [SerializeField] private StatList _statList;
        [Space]
        [Header("Outer References")]
        [SerializeField] private GameObjectReference _target;
        [SerializeField] private Transform _firePoint;
        [Space]
        [Header("Info Indication")]
        [SerializeField] private bool _isOnReload;
        [SerializeField] private bool _isOnCooldown;
        
        public bool Ready => !_isOnCooldown
                             && !_ammoSupply.IsEmpty
                             && !_isOnReload;

        private Resource _ammoSupply;

        private void Awake()
        {
            _ammoSupply = _resourceList.GetResource(ResourceName.Ammo);
        }

        public void TryDoAction()
        {
            if (!Ready) return;
            _ammoSupply.Decrease();
            Shoot();
            StartCoroutine(WaitForCoolDown());
            if (!_ammoSupply.IsEmpty) return;
            StartCoroutine(WaitForReload());
        }

        private void Shoot()
        {
            var projectiles = CreateProjectiles();
            var direction = GetShootDirection();
            LaunchProjectiles(projectiles, direction);
        }

        private GameObject[] CreateProjectiles()
        {
            var projectileCount = (int)_statList.GetStat(StatName.Projectiles).Value;
            var projectiles = new GameObject[projectileCount];
            for (var i = 0; i < projectiles.Length; i++)
            {
                projectiles[i] = Instantiate(_ammo, _firePoint.position, _firePoint.rotation);
            }
            return projectiles;
        }

        private Vector2 GetShootDirection()
        {
            return _target.Value.transform.position - transform.position;
        }

        private void LaunchProjectiles(IReadOnlyCollection<GameObject> projectiles, Vector2 direction)
        {
            var projSpread = _statList.GetStat(StatName.MaxShootDeflectionAngle).Value;
            var fireAngle = projSpread * (projectiles.Count - 1);
            var halfFireAngleRad = fireAngle * 0.5f * Mathf.Deg2Rad;
            var leftDirection = MathFirearm.Rotate(direction, -halfFireAngleRad);
            var actualShotDirection = leftDirection;
            var shootForce = _statList.GetStat(StatName.ShootForce);
            foreach (var projectile in projectiles)
            {
                var projStats = projectile.GetComponent<StatList>();
                var proj = projectile.GetComponent<Projectile>();
                ImproveProjectile(projStats);
                proj.Launch(actualShotDirection, shootForce.Value);
                actualShotDirection = MathFirearm.Rotate(actualShotDirection, projSpread * Mathf.Deg2Rad);
            }
        }

        private void ImproveProjectile(StatList projectileStats)
        {
            var statMod = new StatMod(OperationType.Multiplication, _statList.GetStat(StatName.ProjectileSizeMultiplier).Value);
            projectileStats.GetStat(StatName.Size).AddModifier(statMod);

            statMod = new StatMod(OperationType.Addition, _statList.GetStat(StatName.Pierce).Value);
            projectileStats.GetStat(StatName.MaximumHealth).AddModifier(statMod);

            statMod = new StatMod(OperationType.Addition, _statList.GetStat(StatName.Damage).Value);
            projectileStats.GetStat(StatName.Damage).AddModifier(statMod);

            statMod = new StatMod(OperationType.Addition, _statList.GetStat(StatName.AddedProjectileKnockback).Value);
            projectileStats.GetStat(StatName.KnockbackPower).AddModifier(statMod);
        }

        private IEnumerator WaitForCoolDown()
        {
            _isOnCooldown = true;
            var minShootInterval = 1f / _statList.GetStat(StatName.ShootsPerSecond).Value;
            var isInstant = !(minShootInterval > 0);
            if (!isInstant)
            {
                yield return new WaitForSeconds(minShootInterval);
                _isOnCooldown = false;
            }
            else
            {
                _isOnCooldown = false;
            }
        }

        private IEnumerator WaitForReload()
        {
            _isOnReload = true;
            var reloadTime = 1 / _statList.GetStat(StatName.ReloadSpeed).Value;
            var isInstant = !(reloadTime > 0);
            if (isInstant)
            {
                CompleteReload();
            }
            else
            {
                yield return new WaitForSeconds(reloadTime);
                CompleteReload();
            }
        }

        private void CompleteReload()
        {
            _isOnReload = false;
            _resourceList.GetResource(ResourceName.Ammo).Fill();
        }
    }
}