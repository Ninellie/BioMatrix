using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Unit
{
    public static Action onGamePaused;
    public static Action onCharacterDeath;
    public static Action onLevelUp;
    public bool isFireButtonPressed = false;
    public int Level
    {
        get => level;
        set
        {
            //if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
            level = value;
            onLevelUp?.Invoke();
        }
    }
    public int ExpToLvlup => ExperienceToSecondLevel + (Level * ExperienceAmountIncreasingPerLevel) - Experience;
    public int Experience
    {
        private get => Experience;
        set
        {
            Experience = value;
            if (ExpToLvlup == 0)
            {
                Experience = 0;
                Level++;
                
            }
        }
    }
    protected const int ExperienceToSecondLevel = 10;
    protected const int ExperienceAmountIncreasingPerLevel = 2;
    protected const int InitialLevel = 1;
    protected override EntityStatsSettings Settings => GlobalStatsSettingsRepository.PlayerStats;
    private int level;
    private SpriteRenderer Sprite => GetComponent<SpriteRenderer>();
    protected new void Awake()
    {
        base.Awake();
        Level = InitialLevel;
    }
    private void Start()
    {
        Time.timeScale = 1f;
    }
    protected new void OnEnable()
    {
        base.OnEnable();
    }
    protected new void OnDisable()
    {
        base.OnDisable();
    }
    private new void Update()
    {
        base.Update();
        if (ExpToLvlup == 0)
        {
            Experience = 0;
            Level++;
            onLevelUp?.Invoke();
        }
    }
    protected new void FixedUpdate()
    {
        base.FixedUpdate();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var collisionGameObject = collision.gameObject;
        switch (collisionGameObject.tag)
        {
            case "Enemy":
                TakeDamage(1);
                break;
            case "Boon":
                Experience++;
                break;
        }
    }
    public void OnMove(InputValue input)
    {
        var inputVector2 = input.Get<Vector2>();
        movement.SetMovementDirection(inputVector2);
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
        UnityEngine.Debug.Log("Game on pause");
    }
    public void OnUnPause(InputValue input)
    {
        onGamePaused?.Invoke();
        UnityEngine.Debug.Log("Game is active");
    }
    protected override void Death()
    {
        onCharacterDeath?.Invoke();
        base.Death();
    }
}