using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Reload))]
[RequireComponent(typeof(FirearmStatsSettings))]
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
    [SerializeField] private bool _isForPlayer;
    [SerializeField] private LayerMask _enemyLayer;
    protected StatFactory statFactory;
    public Resource Magazine { get; set; }
    public FirearmStatsSettings Settings => GetComponent<FirearmStatsSettings>();
    private Reload _reload;
    private ProjectileCreator _projectileCreator;
    public bool CanShoot => _previousShootTimer <= 0
                            && !Magazine.IsEmpty
                            && !_reload.IsInProcess;
    private bool IsFireButtonPressed => !_isForPlayer || _player.IsFireButtonPressed;

    private float _previousShootTimer = 0;
    private float MinShootInterval => 1f / ShootsPerSecond.Value;
    private Player _player;
    private void Awake() => BaseAwake(Settings);
    private void BaseAwake(FirearmStatsSettings settings)
    {
        //magazine = GetComponent<Magazine>();
        _reload = GetComponent<Reload>();
        _projectileCreator = GetComponent<ProjectileCreator>();

        statFactory = Camera.main.GetComponent<StatFactory>();

        Damage = statFactory.GetStat(settings.damage);
        ShootForce = statFactory.GetStat(settings.shootForce);
        ShootsPerSecond = statFactory.GetStat(settings.shootsPerSecond);
        MaxShootDeflectionAngle = statFactory.GetStat(settings.maxShootDeflectionAngle);
        MagazineSize = statFactory.GetStat(settings.magazineSize);
        ReloadSpeed = statFactory.GetStat(settings.reloadSpeed);
        SingleShootProjectile = statFactory.GetStat(settings.singleShootProjectile);
        
        Magazine = new Resource(0, MagazineSize);
        Magazine.Fill();
        _player = FindObjectOfType<Player>();
    }
    private void Update()
    {
        _previousShootTimer -= Time.deltaTime;
        if (!IsFireButtonPressed) return;
        if (CanShoot) Shoot();
    }

    public void OnReload()
    {
        if (_isForPlayer)
        {
            _player.OnReload();
            //_player.ReloadEvent?.Invoke();
        }
    }

    public void OnReloadEnd()
    {
        if (_isForPlayer)
        {
            _player.OnReloadEnd();
            //_player.reloadEndEvent?.Invoke();
        }
    }
    public bool GetIsForPlayer()
    {
        return _isForPlayer;
    }
    //public void AddStatModifier(string statName, StatModifier statModifier)
    //{
    //    switch (statName)
    //    {
    //        case "firearmDamage":
    //            Damage.AddModifier(statModifier);
    //            break;
    //        case "projectileSpeed":
    //            ShootForce.AddModifier(statModifier);
    //            break;
    //        case "fireRate":
    //            ShootsPerSecond.AddModifier(statModifier);
    //            break;
    //        case "bulletsSpread":
    //            MaxShootDeflectionAngle.AddModifier(statModifier);
    //            break;
    //        case "magazineSize":
    //            MagazineSize.AddModifier(statModifier);
    //            break;
    //        case "reloadSpeed":
    //            ReloadSpeed.AddModifier(statModifier);
    //            break;
    //        case "projectileNumber":
    //            SingleShootProjectile.AddModifier(statModifier);
    //            break;
    //    }
    //}
    private void Shoot()
    {
        Magazine.Decrease();
        var projectiles = _projectileCreator.CreateProjectiles((int)SingleShootProjectile.Value, _ammo, gameObject.transform);
        var direction = GetShotDirection();
        foreach (var projectile in projectiles)
        {
            var actualShotDirection = GetActualShotDirection(direction, MaxShootDeflectionAngle.Value);
            projectile.GetComponent<Projectile>().Launch(actualShotDirection, ShootForce.Value);
        }
        _previousShootTimer = MinShootInterval;
    }

    private Vector2 GetShotDirection()
    {
        if (_isForPlayer)
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