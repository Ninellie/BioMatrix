using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class Player : Unit
{
    public PlayerStatsSettings Settings => GetComponent<PlayerStatsSettings>();
    private GameTimeScheduler _gameTimeScheduler;
    protected Stat MagnetismRadius { get; private set; }
    protected Stat TurretCount { get; private set; }
    protected Stat MaxShieldLayersCount { get; private set; }
    protected Stat MaxRechargeableShieldLayersCount { get; private set; }
    protected Stat ShieldLayerRechargeRatePerMinute { get; private set; }

    public Action onGamePaused;
    public Action onLevelUp;
    public Action onExperienceTaken;
    public Action onLayerLost;
    public Action onShieldLost;
    public Action onLayerRestore;
    public Action onShieldRestore;

    [SerializeField] private SpriteRenderer _shieldSprite;
    [SerializeField] private float _alphaPerLayer = 0.2f;
    [SerializeField] private Color _shieldColor = Color.cyan;
    [SerializeField] private float _shieldRepulseRadius = 250f;
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private GameObject _shield;
    [SerializeField] private GameObject _turretPrefab;
    [SerializeField] private GameObject _turretWeaponPrefab;

    private Stack<Turret> _currentTurrets = new Stack<Turret>();
    
    public Firearm CurrentFirearm { get; private set; }

    public bool isShieldOnRecharge => _currentActiveShieldLayersCount < MaxRechargeableShieldLayersCount.Value;
    public bool isShieldFullyCharged => _currentActiveShieldLayersCount >= MaxRechargeableShieldLayersCount.Value;
    public bool isShieldOverCharged => _currentActiveShieldLayersCount > MaxRechargeableShieldLayersCount.Value;
    public bool isShieldMaxCharged => _currentActiveShieldLayersCount == MaxShieldLayersCount.Value;
    private int _currentActiveShieldLayersCount;
    private float AccumulatedShieldCharge
    {
        get => _accumulatedShieldCharge;
        set
        {
            if (_currentActiveShieldLayersCount >= MaxRechargeableShieldLayersCount.Value)
            {
                return;
            }
            if (value < 1)
            {
                _accumulatedShieldCharge = value;
                return;
            }
            AddLayer();
            _accumulatedShieldCharge = 0;
        }
    }

    private float _accumulatedShieldCharge = 0f;

    public bool isFireButtonPressed = false;
    public int Level
    {
        get => _level;
        set
        {
            if (value < InitialLevel) throw new ArgumentOutOfRangeException(nameof(value));
            _level = value;
            onLevelUp?.Invoke();
        }
    }
    public int ExpToLvlup => ExperienceToSecondLevel + (Level * ExperienceAmountIncreasingPerLevel) - Experience;
    public int Experience
    {
        get => _experience;
        set
        {
            _experience = value;
            onExperienceTaken?.Invoke();
            if (ExpToLvlup != 0) return;
            _experience = 0;
            Level++;
        }
    }

    private const int ExperienceToSecondLevel = 2;
    private const int ExperienceAmountIncreasingPerLevel = 1;
    private const int InitialLevel = 1;
    private const int InitialExperience = 0;
    private int _level;
    private int _experience;

    private MovementControllerPlayer _movementController;
    private CircleCollider2D _circleCollider;
    private CapsuleCollider2D _capsuleCollider;
    private GameTimer _freezeTimer;
    private SpriteRenderer _spriteRenderer;
    private Invulnerability _invulnerability;
    private void Awake() => BaseAwake(Settings);

    private void Start()
    {
        _freezeTimer = new GameTimer(Freeze, 0.2f);
        UpdateShieldAlpha();
        _capsuleCollider.enabled = _currentActiveShieldLayersCount < 0;
        for (int i = 0; i < MaxRechargeableShieldLayersCount.Value; i++)
        {
            AddLayer();
        }

        UpdateTurrets();
    }

    private void OnEnable() => BaseOnEnable();
    private void OnDisable() => BaseOnDisable();
    private void Update() => BaseUpdate();
    private void FixedUpdate() => BaseFixedUpdate();
    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        Collider2D otherCollider2D = collision2D.collider;
        if (!otherCollider2D.gameObject.TryGetComponent<Entity>(out Entity entity))
        {
            Debug.LogWarning("OnTriggerEnter2D with game object without Entity component");
            return;
        }
        if (!otherCollider2D.gameObject.CompareTag("Enemy") && !otherCollider2D.gameObject.CompareTag("Enclosure")) return;
        if (_currentActiveShieldLayersCount > 0)
        {
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, _shieldRepulseRadius, _enemyLayer);

            foreach (Collider2D collider2d in hitColliders)
            {
                collider2d.gameObject.GetComponent<Enemy>()._enemyMoveController.KnockBackFromTarget(KnockbackPower.Value);
            }
            RemoveLayer();

            if (!otherCollider2D.gameObject.CompareTag("Enclosure")) return;
            var collisionEnclosure = otherCollider2D.gameObject.GetComponent<Entity>();
            KnockBackToEnclosureCenter(collisionEnclosure);
        }
        else
        {
            if (otherCollider2D.gameObject.CompareTag("Enemy"))
            {
                var collisionEnemy = otherCollider2D.gameObject.GetComponent<Entity>();
                var enemyDamage = collisionEnemy.Damage.Value;
                TakeDamage(enemyDamage);
                KnockBackFrom(collisionEnemy);
            }
            if (otherCollider2D.gameObject.CompareTag("Enclosure"))
            {
                var collisionEnclosure = otherCollider2D.gameObject.GetComponent<Entity>();
                var enclosureDamage = collisionEnclosure.Damage.Value;
                TakeDamage(enclosureDamage);
                KnockBackToEnclosureCenter(collisionEnclosure);
            }
            _invulnerability.ApplyInvulnerable(collision2D);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _shieldRepulseRadius);
    }
    private void OnDrawGizmos()
    {
        if (_movementController is null)
        {
            return;
        }
        Gizmos.color = Color.red;
        Gizmos.DrawLine(Rb2D.transform.position, _movementController.GetVelocity() + (Vector2)Rb2D.transform.position);
    }

    protected void BaseFixedUpdate()
    {
        _movementController.FixedUpdateStep();
    }
    protected void BaseAwake(PlayerStatsSettings settings)
    {
        Debug.Log($"{gameObject.name} Player Awake");

        _gameTimeScheduler = FindObjectOfType<GameTimeScheduler>();

        _level = InitialLevel;
        _experience = InitialExperience;

        _circleCollider = GetComponent<CircleCollider2D>();
        _capsuleCollider = GetComponent<CapsuleCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _invulnerability = GetComponent<Invulnerability>();
        _shieldSprite = _shield.GetComponent<SpriteRenderer>();


        MaxRechargeableShieldLayersCount = new Stat(settings.maxRechargeableShieldLayersCount);
        MaxShieldLayersCount = new Stat(settings.maxShieldLayersCount);
        ShieldLayerRechargeRatePerMinute = new Stat(settings.shieldLayerRechargeRate);
        MagnetismRadius = new Stat(settings.magnetismRadius);
        TurretCount = new Stat(settings.turretCount);
        
        _circleCollider.radius = MagnetismRadius.Value;

        if (MagnetismRadius.Value < 0)
        {
            _circleCollider.radius = 0;
        }

        base.BaseAwake(settings);

        _movementController = new MovementControllerPlayer(this);
    }
    
    protected override void BaseUpdate()
    {
        base.BaseUpdate();
        _freezeTimer.Update();
        AccumulatedShieldCharge += ShieldLayerRechargeRatePerMinute.Value / 60 * Time.deltaTime;
    }
    protected override void BaseOnEnable()
    {
        base.BaseOnEnable();
        if (MagnetismRadius != null) MagnetismRadius.onValueChanged += ChangeCurrentMagnetismRadius;
        if (TurretCount != null) TurretCount.onValueChanged += UpdateTurrets;
    }
    protected override void BaseOnDisable()
    {
        base.BaseOnDisable();
        if (MagnetismRadius != null) MagnetismRadius.onValueChanged -= ChangeCurrentMagnetismRadius;
        if (TurretCount != null) TurretCount.onValueChanged -= UpdateTurrets;
    }

    private void UpdateTurrets()
    {
        int dif = (int)TurretCount.Value - _currentTurrets.Count;
        bool isAboveZero = dif > 0;
        float delay = 1;

        while (dif != 0)
        {
            if (isAboveZero)
            {
                Invoke(nameof(CreateTurret), delay);
                dif--;
            }
            else
            {
                Invoke(nameof(DestroyTurret), delay);
                dif++;
            }

            delay++;
        }
    }
    protected void KnockBackFrom(Entity collisionEntity)
    {
        _movementController.KnockBackFromEntity(collisionEntity);
    }
    protected void KnockBackFrom(Entity collisionEntity, Vector2 position)
    {
        _movementController.KnockBackFromPosition(collisionEntity, position);
    }
    protected void KnockBackToEnclosureCenter(Entity collisionEntity)
    {
        Vector2 entityPosition = collisionEntity.transform.position;

        Camera mainCamera = Camera.main;
        Vector3 cameraPos = mainCamera.transform.position;
        Vector3 cameraTopRight = new Vector3(mainCamera.aspect * mainCamera.orthographicSize, mainCamera.orthographicSize, 0f) + cameraPos;
        Vector3 cameraBottomLeft = new Vector3(-mainCamera.aspect * mainCamera.orthographicSize, -mainCamera.orthographicSize, 0f) + cameraPos;

        float width = cameraTopRight.x - cameraBottomLeft.x;
        float height = cameraTopRight.y - cameraBottomLeft.y;

        Vector2 addedPos = new Vector2(width / 2, height / 2);

        entityPosition += addedPos;

        _movementController.KnockBackTo(collisionEntity, entityPosition);
    }
    public void AddLayer()
    {
        if (_currentActiveShieldLayersCount >= MaxShieldLayersCount.Value) return;

        if (_currentActiveShieldLayersCount == 0)
        {
            _capsuleCollider.enabled = true;
            _shield.SetActive(true);
        }

        _currentActiveShieldLayersCount++;

        UpdateShieldAlpha();

        if (isShieldFullyCharged)
        {
            onShieldRestore?.Invoke();
        }
    }

    public void RemoveLayer()
    {
        if (_currentActiveShieldLayersCount == 0) return;
        _currentActiveShieldLayersCount--;
        UpdateShieldAlpha();
        onLayerLost?.Invoke();

        if (_currentActiveShieldLayersCount != 0) return;
        _capsuleCollider.enabled = false;
        _shield.SetActive(false);
        onShieldLost?.Invoke();
    }

    private void UpdateShieldAlpha()
    {
        var a = _alphaPerLayer * _currentActiveShieldLayersCount;
        _shieldColor.a = a;
        _shieldSprite.color = _shieldColor;
    }

    private void Freeze()
    {
        Time.timeScale = 0f;
        this.GetComponent<PlayerInput>().SwitchCurrentActionMap("Menu");
    }

    private void ChangeCurrentMagnetismRadius()
    {
        if (MagnetismRadius.Value < 0)
        {
            _circleCollider.radius = 0;
        }
        _circleCollider.radius = MagnetismRadius.Value;
    }
    public void CreateWeapon(GameObject weapon)
    {
        var w = Instantiate(weapon);

        w.transform.SetParent(_firePoint);

        w.transform.position = _firePoint.transform.position;
        var firearm = w.GetComponent<Firearm>();
        CurrentFirearm = firearm;
    }

    public void CreateTurret()
    {
        Debug.LogWarning("Turret creating");
        var turretGameObject = Instantiate(_turretPrefab);

        turretGameObject.transform.SetParent(this.gameObject.transform);

        var createdTurret = turretGameObject.GetComponent<Turret>();

        createdTurret.SetAttractor(this.gameObject);

        createdTurret.CreateWeapon(_turretWeaponPrefab);

        _currentTurrets.Push(createdTurret);
    }

    public void DestroyTurret()
    {
        var turret = _currentTurrets.Pop();
        turret.Destroy();
    }

    public void AddStatModifier(string statName, StatModifier statModifier)
    {
        switch (statName)
        {
            case "speed":
                Speed.AddModifier(statModifier);
                break;
            case "maximumLifePoints":
                MaximumLifePoints.AddModifier(statModifier);
                break;
            case "magnetismRadius":
                MagnetismRadius.AddModifier(statModifier);
                break;
            case "lifeRegenerationPerSecond":
                LifeRegenerationPerSecond.AddModifier(statModifier);
                break;
            case "maxShieldLayersCount":
                MaxShieldLayersCount.AddModifier(statModifier);
                break;
            case "maxRechargeableShieldLayersCount":
                MaxRechargeableShieldLayersCount.AddModifier(statModifier);
                break;
            case "shieldLayerRechargeRate":
                ShieldLayerRechargeRatePerMinute.AddModifier(statModifier);
                break;
            case "turretCount":
                TurretCount.AddModifier(statModifier);
                break;
        }
    }
    public void OnMove(InputValue input)
    {
        var inputVector2 = input.Get<Vector2>();
        _movementController.SetDirection(inputVector2);
        _spriteRenderer.flipX = inputVector2.x switch
        {
            < 0 => true,
            > 0 => false,
            _ => _spriteRenderer.flipX
        };
    }
    public void OnFire()
    {
        isFireButtonPressed = true;
    }
    public void OnFireOff()
    {
        isFireButtonPressed = false;
    }
    public void OnPause(InputValue input)
    {
        onGamePaused?.Invoke();
        Debug.Log("Game on pause");
    }
    public void OnUnpause(InputValue input)
    {
        onGamePaused?.Invoke();
        Debug.Log("Game is active");
    }
}