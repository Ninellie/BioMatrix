using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Unit
{
    public Action onGamePaused;
    public Action onLevelUp;
    public Action onExperienceTaken;
    public PlayerStatsSettings Settings => GetComponent<PlayerStatsSettings>();
    protected Stat MagnetismRadius { get; private set; }
    protected Stat MaxApproachableShieldLayers { get; private set; }
    protected Stat ShieldLayerRegenerationRatePerMinute { get; private set; }

    public int CurrentActiveShieldLayers
    {
        get => _currentActiveShieldLayers;
        private set => _currentActiveShieldLayers = (int)(value > MaxApproachableShieldLayers.Value ? MaxApproachableShieldLayers.Value : value);
    }
    private int _currentActiveShieldLayers;
    private float ReservedShieldRegeneration
    {
        get => _reservedShieldRegeneration;
        set
        {
            if (CurrentActiveShieldLayers >= MaxApproachableShieldLayers.Value)
            {
                return;
            }
            if (value < 1)
            {
                _reservedShieldRegeneration = value;
                return;
            }
            AddLayer();
            _reservedShieldRegeneration = 0;
        }
    }

    private float _reservedShieldRegeneration = 0f;

    [SerializeField] private SpriteRenderer _shieldSprite;
    [SerializeField] private float _alphaPerLayer = 0.2f;
    [SerializeField] private Color _shieldColor = Color.cyan;
    [SerializeField] private float _shieldRepulseRadius = 250f;
    [SerializeField] private LayerMask _enemyLayer;

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

    public Firearm CurrentFirearm { get; private set; }

    [SerializeField] private Transform _firePoint;
    [SerializeField] private GameObject _shield;

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
        _capsuleCollider.enabled = CurrentActiveShieldLayers < 0;
        for (int i = 0; i < MaxApproachableShieldLayers.Value; i++)
        {
            AddLayer();
        }
    }

    private void AddLayer()
    {
        if (CurrentActiveShieldLayers == 0)
        {
            _capsuleCollider.enabled = true;
            _shield.SetActive(true);
        }
        CurrentActiveShieldLayers++;
        UpdateShieldAlpha();
    }

    private void UpdateShieldAlpha()
    {
        var a = _alphaPerLayer * CurrentActiveShieldLayers;
        _shieldColor.a = a;
        _shieldSprite.color = _shieldColor;
    }

    private void RemoveLayer()
    {
        if (CurrentActiveShieldLayers == 0) return;
        CurrentActiveShieldLayers--;
        UpdateShieldAlpha();
        if (CurrentActiveShieldLayers != 0) return;
        _capsuleCollider.enabled = false;
        _shield.SetActive(false);
    }
    private void Freeze()
    {
        Time.timeScale = 0f;
        this.GetComponent<PlayerInput>().SwitchCurrentActionMap("Menu");
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
        if (CurrentActiveShieldLayers > 0)
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
    protected void BaseFixedUpdate()
    {
        _movementController.FixedUpdateStep();
    }
    protected void BaseAwake(PlayerStatsSettings settings)
    {
        Debug.Log($"{gameObject.name} Player Awake");
        _level = InitialLevel;
        _experience = InitialExperience;
        _circleCollider = GetComponent<CircleCollider2D>();
        _capsuleCollider = GetComponent<CapsuleCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        MagnetismRadius = new Stat(settings.magnetismRadius);
        _circleCollider.radius = MagnetismRadius.Value;
        if (MagnetismRadius.Value < 0)
        {
            _circleCollider.radius = 0;
        }
        MaxApproachableShieldLayers = new Stat(settings.maxShieldLayers);
        ShieldLayerRegenerationRatePerMinute = new Stat(settings.shieldLayerRegenerationRate);

        _shieldSprite = _shield.GetComponent<SpriteRenderer>();
        _invulnerability = GetComponent<Invulnerability>();
        base.BaseAwake(settings);

        _movementController = new MovementControllerPlayer(this);
    }

    protected override void BaseUpdate()
    {
        base.BaseUpdate();
        _freezeTimer.Update();
        ReservedShieldRegeneration += ShieldLayerRegenerationRatePerMinute.Value / 60 * Time.deltaTime;
    }
    protected override void BaseOnEnable()
    {
        base.BaseOnEnable();
        if (MagnetismRadius != null) MagnetismRadius.onValueChanged += ChangeCurrentMagnetismRadius;
        if (MaxApproachableShieldLayers != null) MaxApproachableShieldLayers.onValueChanged += AddLayer;
    }
    protected override void BaseOnDisable()
    {
        base.BaseOnDisable();
        if (MagnetismRadius != null) MagnetismRadius.onValueChanged -= ChangeCurrentMagnetismRadius;
        if (MaxApproachableShieldLayers != null) MaxApproachableShieldLayers.onValueChanged -= AddLayer;
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
            case "maxApproachableShieldLayers":
                MaxApproachableShieldLayers.AddModifier(statModifier);
                break;
            case "shieldLayerRegenerationRate":
                ShieldLayerRegenerationRatePerMinute.AddModifier(statModifier);
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