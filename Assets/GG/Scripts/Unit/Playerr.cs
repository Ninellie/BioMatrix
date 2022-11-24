using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Playerr : Unit
{
    public static Action OnGamePaused;
    public static Action OnCharacterDeath;
    public static Action OnLevelUp;
    public bool isFireButtonPressed = false;

    protected override EntityStatsSettings Settings => GlobalStatsSettingsRepository.PlayerStats;

    private int _level;
    private int _experience;
    private int _expToLvlup;
    private Firearmr _weapon;
    private SpriteRenderer Sprite => GetComponent<SpriteRenderer>();
    protected new void Awake()
    {
        base.Awake();
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
    private void Update()
    {
        if (_level == 0)
        {
            _expToLvlup = 10 - _experience;
        }
        else
        {
            _expToLvlup = 10 + (_level * 2) - _experience;
        }

        if (_experience >= 10 + (_level * 2))
        {
            _experience = 0;
            _level++;
            OnLevelUp?.Invoke();
        }
    }
    protected new void FixedUpdate()
    {
        base.FixedUpdate();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var newLifePointsValue = currentLifePoints - 1;

        UnityEngine.Debug.Log("Dont touch the Hero!");
        var collisionGameObject = collision.gameObject;
        switch (collisionGameObject.tag)
        {
            case "Enemy":
                currentLifePoints = newLifePointsValue;
                break;
            case "Projectile":
                currentLifePoints--;
                break;
            case "Boon":
                _experience++;
                break;
        }
        if (currentLifePoints <= 0)
        {
            OnCharacterDeath?.Invoke();
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
        OnGamePaused?.Invoke();
        UnityEngine.Debug.Log("Game on pause");
    }
    public void OnUnPause(InputValue input)
    {
        OnGamePaused?.Invoke();
        UnityEngine.Debug.Log("Game is active");
    }
}