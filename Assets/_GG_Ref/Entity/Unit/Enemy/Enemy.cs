using System;
using UnityEngine;

public class Enemy : Unit
{
    [SerializeField] private GameObject _onDeathDrop;
    [SerializeField] private GameObject _damagePopup;

    public Stat SpawnWeight = new(GlobalStatsSettingsRepository.EnemyStats.SpawnWeight);
    private Rarity _rarity;
    private GameObject _collisionGameObject;
    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _spriteRenderer;

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
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        
        var movement = new Movement(gameObject, MovementMode.Seek, settings.Speed);
        movement.SetPursuingTarget(FindObjectOfType<Player>().gameObject);
        base.BaseAwake(settings, movement);
        _rarity = Rarity.Normal;
        Level = MinInitialLevel;
        RestoreLifePoints();
    }
    public void SetRarity(Rarity rarity)
    {
        _rarity = rarity;
        switch (rarity)
        {
            case Rarity.Normal:
                _spriteRenderer.material.SetFloat("_OutlineWidth", 0);
                _spriteRenderer.material.SetColor("_OutlineColor", Color.white);
                break;
            case Rarity.Magic:
                _spriteRenderer.material.SetFloat("_OutlineWidth", 0.02f);
                _spriteRenderer.material.SetColor("_OutlineColor", Color.cyan);
                MaximumLifePoints.AddModifier(new StatModifier(OperationType.Multiplication, 500));
                break;
            case Rarity.Rare:
                _spriteRenderer.material.SetFloat("_OutlineWidth", 0.02f);
                _spriteRenderer.material.SetColor("_OutlineColor", Color.yellow);
                MaximumLifePoints.AddModifier(new StatModifier(OperationType.Multiplication, 1000));
                break;
            case Rarity.Unique:
                _spriteRenderer.material.SetFloat("_OutlineWidth", 0.02f);
                _spriteRenderer.material.SetColor("_OutlineColor", Color.magenta);
                MaximumLifePoints.AddModifier(new StatModifier(OperationType.Multiplication, 1500));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(rarity), rarity, null);
        }
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
        Instantiate(_onDeathDrop, _rigidbody2D.position, _rigidbody2D.transform.rotation);
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