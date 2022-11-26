using System;
using System.Diagnostics;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(Reload))]
[RequireComponent(typeof(Magazine))]
[RequireComponent(typeof(ProjectileCreator))]
public class Firearm : MonoBehaviour
{
    public Stat damage;
    public Stat shootForce;
    public Stat shootsPerSecond;
    public Stat maxShootDeflectionAngle;
    public Stat magazineSize;
    public Stat reloadSpeed;
    public Stat singleShootProjectile;
    public GameObject ammo;
    public bool CanShoot => _previousShootStopwatch.Elapsed >= MinShootInterval
                            && !Magazine.IsEmpty
                            && !Reload.IsInProcess;
    private FirearmStatsSettings Settings => GlobalStatsSettingsRepository.ShotgunSettings;
    private bool IsFireButtonPressed =>
        GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Player>().isFireButtonPressed;
    private Reload Reload => GetComponent<Reload>();
    private Magazine Magazine => GetComponent<Magazine>();
    private ProjectileCreator ProjectileCreator => GetComponent<ProjectileCreator>();
    private readonly Stopwatch _previousShootStopwatch = new();
    private TimeSpan MinShootInterval => TimeSpan.FromSeconds(1d / shootsPerSecond.Value);
    
    private void Awake()
    {
        //gameObject.AddComponent<Reload>();
        //gameObject.AddComponent<Magazine>();
        //gameObject.AddComponent<ProjectileCreator>();
        SetStats(Settings);
        _previousShootStopwatch.Start();
    }
    private void Update()
    {
        if (!IsFireButtonPressed) return;
        if (CanShoot) Shoot();
    }
    public void Shoot()
    {
        Magazine.Pop();
        var projectiles = ProjectileCreator.CreateProjectiles((int)singleShootProjectile.Value, ammo, gameObject.transform);
        Vector2 direction = GetShotDirection();
        foreach (var projectile in projectiles)
        {
            projectile.GetComponent<Projectile>().Launch(direction, maxShootDeflectionAngle.Value, shootForce.Value);
        }
        _previousShootStopwatch.Restart();
    }
    private Vector2 GetShotDirection()
    {
        return Lib2DMethods.DirectionToTarget(
            Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()),
            gameObject.transform.position);
    }
    protected virtual void SetStats([NotNull] FirearmStatsSettings settings)
    {
        if (settings == null) throw new ArgumentNullException(nameof(settings));
        damage = new Stat(settings.Damage);
        shootForce = new Stat(settings.ShootForce);
        shootsPerSecond = new Stat(settings.ShootsPerSecond);
        maxShootDeflectionAngle = new Stat(settings.MaxShootDeflectionAngle);
        magazineSize = new Stat(settings.MagazineSize);
        reloadSpeed = new Stat(settings.ReloadSpeed);
        singleShootProjectile = new Stat(settings.SingleShootProjectile);
    }
}