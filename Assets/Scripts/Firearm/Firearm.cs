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
    public FirearmStatsSettings Settings => GetComponent<FirearmStatsSettings>();
    public bool CanShoot => _previousShootStopwatch.Elapsed >= MinShootInterval
                            && !Magazine.IsEmpty
                            && !Reload.IsInProcess;
    public Magazine Magazine => GetComponent<Magazine>();
    private bool IsFireButtonPressed => _player.isFireButtonPressed;
    private Player _player;
    private Reload Reload => GetComponent<Reload>();
    private ProjectileCreator ProjectileCreator => GetComponent<ProjectileCreator>();
    private readonly Stopwatch _previousShootStopwatch = new();
    private TimeSpan MinShootInterval => TimeSpan.FromSeconds(1d / ShootsPerSecond.Value);
    private void Awake() => BaseAwake(Settings);
    private void BaseAwake(FirearmStatsSettings settings)
    {
        Damage = new Stat(settings.damage);
        ShootForce = new Stat(settings.shootForce);
        ShootsPerSecond = new Stat(settings.shootsPerSecond);
        MaxShootDeflectionAngle = new Stat(settings.maxShootDeflectionAngle);
        MagazineSize = new Stat(settings.magazineSize);
        ReloadSpeed = new Stat(settings.reloadSpeed);
        SingleShootProjectile = new Stat(settings.singleShootProjectile);
        _previousShootStopwatch.Start();
        _player = FindObjectOfType<Player>();
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
            var actualShotDirection = GetActualShotDirection(direction, MaxShootDeflectionAngle.Value);
            projectile.GetComponent<Projectile>().Launch(actualShotDirection, ShootForce.Value);
        }
        _previousShootStopwatch.Restart();
    }
    private Vector2 GetShotDirection()
    {
        return Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - gameObject.transform.position;
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