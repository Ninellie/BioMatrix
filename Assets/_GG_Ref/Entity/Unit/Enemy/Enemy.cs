using System;
using UnityEngine;

public class Enemy : Unit
{
    [SerializeField] private GameObject _onDeathDrop;
    [SerializeField] private GameObject _damagePopup;

    public Stat spawnWeight = new(GlobalStatsSettingsRepository.EnemyStats.SpawnWeight);

    private Rarity _rarity = new Rarity();
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
        _rarity.Value = RarityEnum.Normal;
        Level = MinInitialLevel;
        RestoreLifePoints();
    }
    public void SetRarity(RarityEnum rarityEnum)
    {
        _rarity.Value = rarityEnum;
        if (rarityEnum == RarityEnum.Normal) return;

        var width = _rarity.Width;
        var color = _rarity.Color;
        var multiplier = _rarity.Multiplier;

        _spriteRenderer.material.SetFloat("_OutlineWidth", width);
        _spriteRenderer.material.SetColor("_OutlineColor", color);
        var statMod = new StatModifier(OperationType.Multiplication, multiplier);
        MaximumLifePoints.AddModifier(statMod);
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
        var rotation = new Quaternion(0, 0, 0, 0);
        Instantiate(_onDeathDrop, _rigidbody2D.position, rotation);
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