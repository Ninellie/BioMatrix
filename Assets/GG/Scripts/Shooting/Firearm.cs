using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Diagnostics;

public class Firearm : MonoBehaviour
{
    private float ShootForce => GetComponent<FirearmSettings>().shootForce;
    private float BulletsPerSecond => GetComponent<FirearmSettings>().bulletsPerSecond;
    private int PierceNumber => GetComponent<FirearmSettings>().pierceNumber;
    private int MagazineSize => GetComponent<FirearmSettings>().magazineSize;
    private float ReloadSpeed => GetComponent<FirearmSettings>().reloadSpeed;
    private float MaxShotDeflectionAngle => GetComponent<FirearmSettings>().maxShotDeflectionAngle;

    [Header("Used weapon")]
    [SerializeField] private GameObject weaponPrefabOrigin;
    [Header("Used bullet")]
    [SerializeField] private GameObject bulletPrefab;

    private GameObject reloadLable;
    private Transform firePoint;
    private GameObject bullet;
    public int MagazineCurrent { get; private set; }

    private bool isReloadInProcess;
    
    private Stopwatch previousShootStopwatch = new();
    private Stopwatch reloadStopwatch = new();

    public static Action OnMagazineEmpty;

    private TimeSpan MinShootInterval => TimeSpan.FromSeconds(1d / BulletsPerSecond);
    private bool IsMagazineEmpty => MagazineCurrent == 0;
    private static bool IsFireButtonPressed => GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Player>().isFireButtonPressed;
    private Vector2 GetRawShotDirection => Lib2DMethods.DirectionToTarget(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()), firePoint.position);
    void Start()
    {
        MagazineCurrent = MagazineSize;
        reloadLable = GameObject.FindWithTag("Canvas").GetComponent<ReloadLabel>().reloadLabel;
        reloadLable.SetActive(false);
        firePoint = GetComponentInParent<WeaponManager>().transform;
        
        previousShootStopwatch.Start();
        reloadStopwatch.Stop();
    }
    private void Update()
    {
        if (!isReloadInProcess)
        {
            if (IsMagazineEmpty)
            {
                OnMagazineEmpty?.Invoke();
                UnityEngine.Debug.Log("Magazine is empty");
            }
        }

        if (IsFireButtonPressed)
        {
            if (CanShoot())
                DoShoot();
        }
    }
    private void OnEnable()
    {
        Firearm.OnMagazineEmpty += Reload;
    }
    private void OnDisable()
    {
        Firearm.OnMagazineEmpty -= Reload;
    }
    private void Reload()
    {
        isReloadInProcess = true;
        reloadLable.SetActive(true);
        Invoke("ReloadOff", 1 / ReloadSpeed);
    }
    private void ReloadOff()
    {
        isReloadInProcess = false;
        reloadLable.SetActive(false);
        MagazineCurrent = MagazineSize;
    }
    private bool IsBuckshot(GameObject projectile)
    {
        if (projectile is null)
        {
            throw new ArgumentNullException(nameof(projectile));
        }
        if (projectile.GetComponent<BulletSettings>() is null)
        {
            throw new ArgumentNullException(nameof(projectile));
        }
        return projectile.GetComponent<BulletSettings>().isBuckshot;
    }
    private bool CanShoot()
    {
        return previousShootStopwatch.Elapsed >= MinShootInterval && !IsMagazineEmpty;
    }
    private void DoShoot()
    {
        MagazineCurrent--;
        if (IsBuckshot(bulletPrefab))
        {
            UnityEngine.Debug.Log("Buckshot");
            ReleaseBuckshot();
        }
        else
        {
            UnityEngine.Debug.Log("Bullet");
            FireBullet();
        }        
        previousShootStopwatch.Restart();
    }
    private void FireBullet()
    {
        bullet = CreateProjectile(bulletPrefab);
        Vector2 actualShotDirection = GetActualShotDirection(GetRawShotDirection, MaxShotDeflectionAngle);
        AccelerateProjectileInDirection(bullet, ShootForce, actualShotDirection);
    }
    private void ReleaseBuckshot()
    {
        int secondaryProjectilesCount = bulletPrefab.GetComponent<BulletSettings>().secondaryProjectilesCount;
        UnityEngine.Debug.Log("secondaryProjectilesCount: " + secondaryProjectilesCount);
        GameObject secondaryProjectile = bulletPrefab.GetComponent<BulletSettings>().secondaryProjectile;
        UnityEngine.Debug.Log("secondaryProjectile: " + secondaryProjectile.name);

        GameObject[] shots = FillBuckshot(secondaryProjectile, secondaryProjectilesCount);
        Vector2[] shotDirections = GetActualShotDirections(GetRawShotDirection, MaxShotDeflectionAngle, shots.Length);
        AccelerateProjectilesInDirections(shots, ShootForce, shotDirections);
    }
    private GameObject[] FillBuckshot(GameObject shot, int amount)
    {
        GameObject[] buckshot = new GameObject[amount];
        for (int i = 0; i < buckshot.Length; i++)
        {
            UnityEngine.Debug.Log(i + " Success!");
            buckshot[i] = CreateProjectile(shot);
        }
        return buckshot;
    }
    private GameObject CreateProjectile(GameObject projectile)
    {
        UnityEngine.Debug.Log("CreateProjectile " + projectile.name);
        GameObject createdProjectile = Instantiate(projectile, firePoint.position, firePoint.rotation);
        createdProjectile.GetComponent<BulletSettings>().pierceCount = PierceNumber;
        return createdProjectile;
    }
    private Vector2 GetActualShotDirection (Vector2 direction, float MaxShotDeflectionAngle)
    {
        UnityEngine.Debug.Log("GetActualShotDirection");
        float angleInRad = (float)Mathf.Deg2Rad * MaxShotDeflectionAngle;
        float shotDeflectionAngle = Range(-angleInRad, angleInRad);
        return Rotate(direction, shotDeflectionAngle);
    }
    private Vector2[] GetActualShotDirections(Vector2 direction, float MaxShotDeflectionAngle, int directionsCount)
    {
        Vector2[] directions = new Vector2[directionsCount];
        for (int i = 0; i < directions.Length; i++)
        {
            directions[i] = GetActualShotDirection(direction, MaxShotDeflectionAngle);
        }
        return directions;
    }
    private void AccelerateProjectileInDirection(GameObject projectile, float force, Vector2 direction)
    {
        UnityEngine.Debug.Log("AccelerateProjectileInDirection " + projectile.name);
        Rigidbody2D rb2D = projectile.GetComponent<Rigidbody2D>();
        rb2D.AddForce(direction.normalized * force, ForceMode2D.Impulse);
    }
    private void AccelerateProjectilesInDirections(GameObject[] projectiles, float force, Vector2[] directions)
    {
        for (int i = 0; i < projectiles.Length; i++)
        {
            AccelerateProjectileInDirection(projectiles[i], force, directions[i]);
        }
    }
    private float Range(float minInclusive, float maxInclusive)
    {
        float std = PeterAcklamInverseCDF.NormInv(UnityEngine.Random.value);
        return PeterAcklamInverseCDF.RandomGaussian(std, minInclusive, maxInclusive);
    }
    private float Range1(float minInclusive, float maxInclusive)
    {
        return UnityEngine.Random.Range(minInclusive, maxInclusive);
    }
    private float Range2(float minInclusive, float maxInclusive)
    {
        return PeterAcklamInverseCDF.RandomGaussian(minInclusive, maxInclusive);
    }
    private Vector2 Rotate(Vector2 point, float angle)
    {
        Vector2 rotated_point;
        rotated_point.x = point.x * Mathf.Cos(angle)
                        - point.y * Mathf.Sin(angle);
        rotated_point.y = point.x * Mathf.Sin(angle)
                        + point.y * Mathf.Cos(angle);
        return rotated_point;
    }
}
