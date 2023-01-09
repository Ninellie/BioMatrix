using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(Reload))]
[RequireComponent(typeof(Magazine))]
[RequireComponent(typeof(ProjectileCreator))]
public class Firearm : MonoBehaviour
{
    public Stat Damage { get; private set; }
    public Stat ShootForce { get; private set; }
    public Stat ShootsPerSecond { get; private set; }
    public Stat MaxShootDeflectionAngle { get; private set; }
    public Stat MagazineSize { get; private set; }
    public Stat ReloadSpeed { get; private set; }
    public Stat SingleShootProjectile { get; private set; }
    
    [SerializeField] private GameObject _ammo;
    public bool CanShoot => _previousShootStopwatch.Elapsed >= MinShootInterval
                            && !Magazine.IsEmpty
                            && !Reload.IsInProcess;
    public Magazine Magazine => GetComponent<Magazine>();
    private bool IsFireButtonPressed =>
        GameObject.FindGameObjectsWithTag("Player")[0]
            .GetComponent<Player>()
            .isFireButtonPressed;
    private Reload Reload => GetComponent<Reload>();
    private ProjectileCreator ProjectileCreator => GetComponent<ProjectileCreator>();
    private readonly Stopwatch _previousShootStopwatch = new();
    private TimeSpan MinShootInterval => TimeSpan.FromSeconds(1d / ShootsPerSecond.Value);
    private void Awake() => BaseAwake(GlobalStatsSettingsRepository.ShotgunStats);
    private void BaseAwake(FirearmStatsSettings settings)
    {
        Damage = new Stat(settings.Damage);
        ShootForce = new Stat(settings.ShootForce);
        ShootsPerSecond = new Stat(settings.ShootsPerSecond);
        MaxShootDeflectionAngle = new Stat(settings.MaxShootDeflectionAngle);
        MagazineSize = new Stat(settings.MagazineSize);
        ReloadSpeed = new Stat(settings.ReloadSpeed);
        SingleShootProjectile = new Stat(settings.SingleShootProjectile);
        _previousShootStopwatch.Start();
    }
    private void Update()
    {
        if (!IsFireButtonPressed) return;
        if (CanShoot) Shoot();
    }
    public void AddStatModifier(string statName, StatModifier statModifier)
    {
        switch (statName)
        {
            case "firearmDamage":
                Damage.AddModifier(statModifier);
                break;
            case "projectileSpeed":
                ShootForce.AddModifier(statModifier);
                break;
            case "fireRate":
                ShootsPerSecond.AddModifier(statModifier);
                break;
            case "bulletsSpread":
                MaxShootDeflectionAngle.AddModifier(statModifier);
                break;
            case "magazineSize":
                MagazineSize.AddModifier(statModifier);
                break;
            case "reloadSpeed":
                ReloadSpeed.AddModifier(statModifier);
                break;
            case "projectileNumber":
                SingleShootProjectile.AddModifier(statModifier);
                break;
        }
    }
    private void Shoot()
    {
        Magazine.Pop();
        var projectiles = ProjectileCreator.CreateProjectiles((int)SingleShootProjectile.Value, _ammo, gameObject.transform);
        var direction = GetShotDirection();
        foreach (var projectile in projectiles)
        {
            projectile.GetComponent<Projectile>().Launch(direction, MaxShootDeflectionAngle.Value, ShootForce.Value);
        }
        _previousShootStopwatch.Restart();
    }

    private Vector2 GetShotDirection()
    {
        return Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - gameObject.transform.position;
    }
}