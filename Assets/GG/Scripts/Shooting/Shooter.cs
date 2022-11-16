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
