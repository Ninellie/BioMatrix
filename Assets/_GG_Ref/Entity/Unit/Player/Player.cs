using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Unit
{
    //public Action onPlayerAwake;
    public Action onGamePaused;
    public Action onPlayerDeath;
    public Action onLevelUp;
    public Action onExperienceTaken;
    
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
        private get => _experience;
        set
        {
            _experience = value;
            onExperienceTaken?.Invoke();
            if (ExpToLvlup == 0)
            {
                _experience = 0;
                Level++;
            }
        }
    }
    private const int ExperienceToSecondLevel = 10;
    private const int ExperienceAmountIncreasingPerLevel = 2;
    private const int InitialLevel = 1;
    private const int InitialExperience = 0;

    private int _level;
    private int _experience;
    private SpriteRenderer Sprite => GetComponent<SpriteRenderer>();
    private void Awake() => BaseAwake(GlobalStatsSettingsRepository.PlayerStats);
    private void Start() => Time.timeScale = 1f;
    private void OnEnable() => BaseOnEnable();
    private void OnDisable() => BaseOnDisable();
    private void Update() => BaseUpdate();
    private void FixedUpdate() => Movement.FixedUpdateMove();
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var collisionGameObject = collision.gameObject;
        switch (collisionGameObject.tag)
        {
            case "Enemy":
                TakeDamage(MinimalDamageTaken);
                break;
            case "Boon":
                Experience++;
                break;
        }
    }
    protected void BaseAwake(UnitStatsSettings settings)
    {
        Debug.Log($"{gameObject.name} Player Awake");
        _level = InitialLevel;
        _experience = InitialExperience;
        var movement = new Movement(gameObject, MovementMode.Rectilinear, settings.Speed);
        base.BaseAwake(settings, movement);
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
    protected override void Death()
    {
        onPlayerDeath?.Invoke();
        base.Death();
    }
}