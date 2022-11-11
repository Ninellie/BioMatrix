using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Diagnostics;

public class Shooter : MonoBehaviour
{
    private FirearmSettings FirearmSettings => GetComponent<FirearmSettings>();
    private ProjectileCreater ProjectileCreator => GetComponent<ProjectileCreater>();
    private Magazine Magazine => GetComponent<Magazine>();
    private readonly Stopwatch _previousShootStopwatch = new();
    private TimeSpan MinShootInterval => TimeSpan.FromSeconds(1d / FirearmSettings.ShotsPerSecond);
    public bool CanShoot => _previousShootStopwatch.Elapsed >= MinShootInterval && !Magazine.IsEmpty;
    void Start()
    {
        _previousShootStopwatch.Start();
    }
    public void Shoot()
    {
        Magazine.Pop();
        GameObject[] projectiles = ProjectileCreator.CreateProjectiles(FirearmSettings);
        Vector2 direction = GetShotDirection();
        foreach (var projectile in projectiles)
        {
            projectile.GetComponent<Ammo>().Launch(direction, FirearmSettings);
        }
        _previousShootStopwatch.Restart();
    }
    private Vector2 GetShotDirection()
    {
        return Lib2DMethods.DirectionToTarget(
            Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()),
            gameObject.transform.position);
    }
}

//public class Shooter : MonoBehaviour
//{
//    private Transform FiringPoint => GetComponentInParent<WeaponManager>().transform;
//    private Magazine Magazine => GetComponent<Magazine>();
//    private FirearmSettings FirearmSettings => GetComponent<FirearmSettings>();
//    //private Ammo Ammo => FirearmSettings.Ammo.GetComponent<Ammo>();
//    public bool CanShoot => previousShootStopwatch.Elapsed >= MinShootInterval && !Magazine.IsEmpty;
//    private Stopwatch previousShootStopwatch = new();
//    private TimeSpan MinShootInterval => TimeSpan.FromSeconds(1d / FirearmSettings.ShotsPerSecond);

//    void Start()
//    {
//        previousShootStopwatch.Start();
//    }
//    public void Shoot()
//    {
//        Magazine.CurrentAmount--;

//        //Ammo.Launch();

//        if (IsShot(FirearmSettings.Ammo))
//        {
//            ReleaseBuckshot();
//        }
//        else
//        {
//            FireBullet();
//        }
//        previousShootStopwatch.Restart();
//    }
//    private void FireBullet()
//    {
//        GameObject projectile = CreateProjectile(FirearmSettings.Ammo, FiringPoint);
//        Vector2 actualShotDirection = GetActualShotDirection(GetRawShotDirection(), FirearmSettings.MaxShotDeflectionAngle);
//        AccelerateProjectileInDirection(projectile, FirearmSettings.ShootForce, actualShotDirection);
//    }
//    private void ReleaseBuckshot()
//    {
//        int shotsAmount = FirearmSettings.SingleShotProjectiles;

//        GameObject projectile = FirearmSettings.Ammo;

//        GameObject[] shots = CreateProjectiles(projectile, shotsAmount);
//        Vector2[] shotDirections = GetActualShotDirections(GetRawShotDirection(), FirearmSettings.MaxShotDeflectionAngle, shotsAmount);
//        AccelerateProjectilesInDirections(shots, FirearmSettings.ShootForce, shotDirections);
//    }
//    private bool IsShot(GameObject projectile)
//    {
//        if (projectile is null)
//        {
//            throw new ArgumentNullException(nameof(projectile));
//        }
//        if (projectile.GetComponent<ProjectileSettings>() is null)
//        {
//            throw new ArgumentNullException(nameof(projectile));
//        }
//        return projectile.GetComponent<ProjectileSettings>().isShot;
//    }
//    private Vector2 GetRawShotDirection()
//    {
//        Vector2 rawShotDirection;
//        if (FiringPoint != null)
//        {
//            return rawShotDirection = Lib2DMethods.DirectionToTarget(
//                Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()),
//                FiringPoint.position);
//        }

//        throw new NotImplementedException();
//    }
//    private GameObject[] CreateProjectiles(GameObject projectile, int amount)
//    {
//        GameObject[] projectiles = new GameObject[amount];
//        for (int i = 0; i < projectiles.Length; i++)
//        {
//            projectiles[i] = CreateProjectile(projectile, FiringPoint);
//        }
//        return projectiles;
//    }
//    //Projectile
//    private GameObject CreateProjectile(GameObject projectile, Transform launchPoint)
//    {
//        UnityEngine.Debug.Log("CreateProjectile " + projectile.name);
//        GameObject createdProjectile = Instantiate(projectile, launchPoint.position, launchPoint.rotation);

//        return createdProjectile;
//    }
//    //Shooting - aiming - direction
//    private Vector2 GetActualShotDirection(Vector2 direction, float maxShotDeflectionAngle)
//    {
//        float angleInRad = (float)Mathf.Deg2Rad * maxShotDeflectionAngle;
//        float shotDeflectionAngle = Lib2DMethods.Range(-angleInRad, angleInRad);
//        return Lib2DMethods.Rotate(direction, shotDeflectionAngle);
//    }
//    //Shooting - aiming - direction
//    private Vector2[] GetActualShotDirections(Vector2 direction, float MaxShotDeflectionAngle, int directionsCount)
//    {
//        Vector2[] directions = new Vector2[directionsCount];
//        for (int i = 0; i < directions.Length; i++)
//        {
//            directions[i] = GetActualShotDirection(direction, MaxShotDeflectionAngle);
//        }
//        return directions;
//    }
//    //Shooting - acceleration - projectile - direction
//    private void AccelerateProjectileInDirection(GameObject projectile, float force, Vector2 direction)
//    {
//        UnityEngine.Debug.Log("AccelerateProjectileInDirection " + projectile.name);
//        Rigidbody2D rb2D = projectile.GetComponent<Rigidbody2D>();
//        rb2D.AddForce(direction.normalized * force, ForceMode2D.Impulse);
//    }
//    //Shooting - acceleration - projectile - direction
//    private void AccelerateProjectilesInDirections(GameObject[] projectiles, float force, Vector2[] directions)
//    {
//        for (int i = 0; i < projectiles.Length; i++)
//        {
//            AccelerateProjectileInDirection(projectiles[i], force, directions[i]);
//        }
//    }
//}
