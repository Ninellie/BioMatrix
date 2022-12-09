using System;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class Enemy : Unit
{
    [SerializeField] private GameObject _onDeathDrop;
    [SerializeField] private GameObject _damagePopup;

    public Stat SpawnWeight = new(GlobalStatsSettingsRepository.EnemyStats.SpawnWeight);

    private GameObject _collisionGameObject;
    private Rigidbody2D Rigidbody2D => GetComponent<Rigidbody2D>();

    private const int MinInitialLevel = 1;
    private const float MaxLifeIncreasePerLevel = 1;
    private int Level
    {
        get => _level;
        set
        {
            _level = value < MinInitialLevel ? MinInitialLevel : value;
            UpdateMaxLifeStat();
        }
    }
    private int _level;
    private void OnEnable() => BaseOnEnable();
    private void OnDisable() => BaseOnDisable();
    private void Awake() => BaseAwake(GlobalStatsSettingsRepository.EnemyStats);
    private void Update() => BaseUpdate();
    private void FixedUpdate() => Movement.FixedUpdateMove();
    private void OnCollisionEnter2D(Collision2D collision)
    {
        _collisionGameObject = collision.gameObject;
        switch (_collisionGameObject.tag)
        {
            case "Player":
                TakeDamage(CurrentLifePoints);
                break;
            case "Projectile":
                TakeDamage(MinimalDamageTaken);
                DropDamagePopup(MinimalDamageTaken, _collisionGameObject.transform.position);
                break;
            case "Enemy":
                break;
        }
    }
    protected void BaseAwake(EnemyStatsSettings settings)
    {
        Debug.Log($"{gameObject.name} Enemy Awake");
        
        
        var movement = new Movement(gameObject, MovementMode.Seek, settings.Speed);
        movement.SetPursuingTarget(FindObjectOfType<Player>().gameObject);
        base.BaseAwake(settings, movement);

        Level = MinInitialLevel;
        RestoreLifePoints();
    }
    public void LevelUp(int value)
    {
        if(value < 1) return;
        Level += value;
    }
    protected override void Death()
    {
        base.Death();
        if (_collisionGameObject.tag == "Projectile")
        {
            DropBonus();
        }
    }
    private void DropBonus()
    {
        Instantiate(_onDeathDrop, Rigidbody2D.position, Rigidbody2D.transform.rotation);
    }
    private void DropDamagePopup(int damage, Vector2 positionVector2)
    {
        var droppedDamagePopup = Instantiate(_damagePopup);
        droppedDamagePopup.transform.position = positionVector2;
        droppedDamagePopup.GetComponent<DamagePopup>().Setup(damage);
    }
    private void UpdateMaxLifeStat()
    {
        if (!MaximumLifePoints.IsModifierListEmpty())
        {
            MaximumLifePoints.ClearModifiersList();
        }
        var mod = new StatModifier(OperationType.Addition, _level * MaxLifeIncreasePerLevel);
        MaximumLifePoints.AddModifier(mod);
    }
}