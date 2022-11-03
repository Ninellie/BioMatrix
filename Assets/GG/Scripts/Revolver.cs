using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Diagnostics;

public class Revolver : MonoBehaviour
{
    public GameObject reloadLable;
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float shootForce;    
    public float bulletsPerSecond;
    public int pierceNumber;
    public int magazineMax;
    public int magazineCurrent;
    public float reloadSpeed;
    public float MaxShotDeflectionAngle;
    private bool isReloadInProcess;

    private Stopwatch previousShootStopwatch = new();
    private Stopwatch reloadStopwatch = new();

    public static Action OnMagazineEmpty;
    private GameObject bullet;

    private TimeSpan MinShootInterval => TimeSpan.FromSeconds(1d / bulletsPerSecond);
    private bool IsMagazineEmpty => magazineCurrent == 0;
    private static bool IsFireButtonPressed => GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Player>().isFireButtonPressed;
    private Vector2 GetRawShotDirection => Lib2DMethods.DirectionToTarget(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()), firePoint.position);

    // Start is called before the first frame update
    void Start()
    {
        //reloadSpeed = 0.8f;
        //pierceNumber = 1;
        //magazineMax = 6;
        magazineCurrent = magazineMax;
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
        Revolver.OnMagazineEmpty += Reload;
    }
    
    private void OnDisable()
    {
        Revolver.OnMagazineEmpty -= Reload;
    }

    private void Reload()
    {
        isReloadInProcess = true;
        reloadLable.SetActive(true);
        Invoke("ReloadOff", 1 / reloadSpeed);
    }

    private void ReloadOff()
    {
        isReloadInProcess = false;
        reloadLable.SetActive(false);
        magazineCurrent = magazineMax;
    }

    private void DoShoot()
    {        
        magazineCurrent--;
        FireBullet();
        previousShootStopwatch.Restart();
    }

    private void FireBullet()
    {
        bullet = CreateProjectile(bulletPrefab);
        AccelerateProjectileInDirection(bullet, shootForce, GetActualShotDirection(GetRawShotDirection, MaxShotDeflectionAngle));
    }

    private GameObject CreateProjectile(GameObject projectile)
    {
        GameObject bullet = Instantiate(projectile, firePoint.position, firePoint.rotation);
        bullet.GetComponent<Bullet>().lifePoints = pierceNumber;
        return bullet;
    }

    private Vector2 GetActualShotDirection (Vector2 direction, float MaxShotDeflectionAngle)
    {
        float angleInRad = (float)Mathf.Deg2Rad * MaxShotDeflectionAngle;
        float shotDeflectionAngle = Range(-angleInRad, angleInRad);
        return rotate(direction, shotDeflectionAngle);
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

    private Vector2 rotate(Vector2 point, float angle)
    {
        Vector2 rotated_point;
        rotated_point.x = point.x * Mathf.Cos(angle) - point.y * Mathf.Sin(angle);
        rotated_point.y = point.x * Mathf.Sin(angle) + point.y * Mathf.Cos(angle);
        return rotated_point;
    }

    private void AccelerateProjectileInDirection(GameObject projectile, float force, Vector2 direction)
    {
        Rigidbody2D rb2D = projectile.GetComponent<Rigidbody2D>();
        rb2D.AddForce(direction.normalized * force, ForceMode2D.Impulse);
    }
    private bool CanShoot()
    {
        return previousShootStopwatch.Elapsed >= MinShootInterval && !IsMagazineEmpty;
    } 
}
