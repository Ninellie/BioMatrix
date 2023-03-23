using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Unit
{
    public Action onGamePaused;
    public Action onLevelUp;
    public Action onExperienceTaken;
    public PlayerStatsSettings Settings => GetComponent<PlayerStatsSettings>();
    protected Stat MagnetismRadius { get; private set; }
    protected Stat MagnetismPower { get; private set; }

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
    private float KnockbackTime
    {
        get => _knockbackTime;
        set
        {
            if (value < 0)
            {
                _knockbackTime = 0;
                return;
            }
            _knockbackTime = value;
        }
    }
    private float _knockbackTime;

    //public bool IsInvulnerable { get; private set; } = false;
    //private GameTimer _invulnerableTimer;
    private const float ReturnToDefaultColorSpeed = 5f;
    private const int ExperienceToSecondLevel = 2;
    private const int ExperienceAmountIncreasingPerLevel = 1;
    private const int InitialLevel = 1;
    private const int InitialExperience = 0;

    private MovementControllerPlayer _movementController;
    private int _level;
    private int _experience;
    private CircleCollider2D _circleCollider;
    private PointEffector2D _pointEffector;
    private GameTimer _freezeTimer;
    private SpriteRenderer SpriteRenderer => GetComponent<SpriteRenderer>();
    private void Awake() => BaseAwake(Settings);

    private void Start()
    {
        _freezeTimer = new GameTimer(Freeze, 0.2f);
    }

    private void Freeze()
    {
        Time.timeScale = 0f;
        this.GetComponent<PlayerInput>().SwitchCurrentActionMap("Menu");
        FindObjectOfType<GameSessionTimer>().Stop();
    }

    private void OnEnable() => BaseOnEnable();
    private void OnDisable() => BaseOnDisable();
    private void Update() => BaseUpdate();
    private void FixedUpdate() => BaseFixedUpdate();
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var collisionGameObject = collision.gameObject;
        switch (collisionGameObject.tag)
        {
            case "Enemy":
                if (KnockbackTime == 0)
                {
                    var collisionEnemyEntity = collisionGameObject.GetComponent<Entity>();
                    var enemyDamage = collisionEnemyEntity.Damage.Value;
                    TakeDamage(enemyDamage);
                    KnockBackFrom(collisionEnemyEntity);
                    spriteRenderer.color = Color.red;
                }
                break;

            case "Enclosure":
            {
                if (KnockbackTime == 0)
                {
                    var collisionEnclosureEntity = collisionGameObject.GetComponent<Entity>();
                    var enclosureDamage = collisionEnclosureEntity.Damage.Value;
                    TakeDamage(enclosureDamage);
                    KnockBackFrom(collisionEnclosureEntity);
                    spriteRenderer.color = Color.red;
                }
                break;
            }
        }
    }
    protected void KnockBackFrom(Entity collisionEntity)
    {
        KnockbackTime += 0.1f;
        _movementController.KnockBack(collisionEntity);
    }
    protected void BaseFixedUpdate()
    {
        if (KnockbackTime == 0)
        {
            _movementController.FixedUpdateStep();
        }
        KnockbackTime -= Time.fixedDeltaTime;
    }
    protected void BaseAwake(PlayerStatsSettings settings)
    {
        Debug.Log($"{gameObject.name} Player Awake");
        _level = InitialLevel;
        _experience = InitialExperience;
        _circleCollider = GetComponent<CircleCollider2D>();
        _pointEffector = GetComponent<PointEffector2D>();
        MagnetismRadius = new Stat(settings.magnetismRadius);
        _circleCollider.radius = MagnetismRadius.Value;
        if (MagnetismRadius.Value < 0)
        {
            _circleCollider.radius = 0;
        }
        MagnetismPower = new Stat(settings.magnetismPower);
        _pointEffector.forceMagnitude = MagnetismPower.Value * -1;

        base.BaseAwake(settings);

        _movementController = new MovementControllerPlayer(this);
    }

    protected override void BaseUpdate()
    {
        base.BaseUpdate();
        _freezeTimer.Update();
        BackToNormalColor();
    }
    private void BackToNormalColor()
    {
        if (spriteRenderer.color == Color.white) return;
        spriteColor = spriteRenderer.color;
        spriteColor.r += ReturnToDefaultColorSpeed * Time.deltaTime;
        spriteColor.g += ReturnToDefaultColorSpeed * Time.deltaTime;
        spriteColor.b += ReturnToDefaultColorSpeed * Time.deltaTime;
        spriteRenderer.color = spriteColor;
    }
    protected override void BaseOnEnable()
    {
        base.BaseOnEnable();
        if (MagnetismPower != null) MagnetismPower.onValueChanged += ChangeCurrentMagnetismPower;
        if (MagnetismRadius != null) MagnetismRadius.onValueChanged += ChangeCurrentMagnetismRadius;
    }
    protected override void BaseOnDisable()
    {
        base.BaseOnDisable();
        if (MagnetismPower != null) MagnetismPower.onValueChanged -= ChangeCurrentMagnetismPower;
        if (MagnetismRadius != null) MagnetismRadius.onValueChanged -= ChangeCurrentMagnetismRadius;
    }
    private void ChangeCurrentMagnetismPower()
    {
        _pointEffector.forceMagnitude = MagnetismPower.Value * -1;
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
            case "magnetismPower":
                MagnetismPower.AddModifier(statModifier);
                break;
            case "magnetismRadius":
                MagnetismRadius.AddModifier(statModifier);
                break;
            case "lifeRegenerationPerSecond":
                LifeRegenerationPerSecond.AddModifier(statModifier);
                break;
        }
    }
    public void OnMove(InputValue input)
    {
        var inputVector2 = input.Get<Vector2>();
        _movementController.SetDirection(inputVector2);
        SpriteRenderer.flipX = inputVector2.x switch
        {
            < 0 => true,
            > 0 => false,
            _ => SpriteRenderer.flipX
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