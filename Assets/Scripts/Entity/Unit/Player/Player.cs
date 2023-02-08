using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Unit
{
    public Action onGamePaused;
    public Action onLevelUp;
    public Action onExperienceTaken;
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

    private const int ExperienceToSecondLevel = 2;
    private const int ExperienceAmountIncreasingPerLevel = 1;
    private const int InitialLevel = 1;
    private const int InitialExperience = 0;

    private int _level;
    private int _experience;
    [SerializeField] private Transform _firePoint;
    private CircleCollider2D _circleCollider;
    private PointEffector2D _pointEffector;
    private SpriteRenderer Sprite => GetComponent<SpriteRenderer>();
    private void Awake() => BaseAwake(GlobalStatsSettingsRepository.PlayerStats);
    private void Start() => Time.timeScale = 1f;
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
                var collisionEnemyEntity = collisionGameObject.GetComponent<Entity>();
                TakeDamage(MinimalDamageTaken);
                KnockBack(collisionEnemyEntity);
                break;
            case "Enclosure":
            {
                var collisionEnclosureEntity = collisionGameObject.GetComponent<Entity>();
                TakeDamage(MinimalDamageTaken);
                KnockBack(collisionEnclosureEntity);
                break;
            }
        }
    }
    protected void BaseAwake(HeroStatsSettings settings)
    {
        Debug.Log($"{gameObject.name} Player Awake");
        _level = InitialLevel;
        _experience = InitialExperience;
        _circleCollider = GetComponent<CircleCollider2D>();
        _pointEffector = GetComponent<PointEffector2D>();
        MagnetismRadius = new Stat(settings.MagnetismRadius);
        if (MagnetismRadius.Value < 0)
        {
            _circleCollider.radius = 0;
        }
        _circleCollider.radius = MagnetismRadius.Value;
        MagnetismPower = new Stat(settings.MagnetismPower);
        _pointEffector.forceMagnitude = MagnetismPower.Value * -1;
        var movement = new Movement(this, MovementState.Rectilinear, settings.Speed);
        base.BaseAwake(settings, movement);
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
    public void CreateWeapon(GameObject weapons)
    {
        var weapon = Instantiate(weapons);

        weapon.transform.SetParent(_firePoint);

        weapon.transform.position = _firePoint.transform.position;
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
        Movement.SetMovementDirection(inputVector2);
        switch (inputVector2.x)
        {
            case < 0:
                Sprite.flipX = true;
                break;
            case > 0:
                Sprite.flipX = false;
                break;
        }
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