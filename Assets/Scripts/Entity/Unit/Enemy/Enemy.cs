using System;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Enemy : Unit
{
    [SerializeField] private GameObject _onDeathDrop;
    [SerializeField] private GameObject _damagePopup;
    [SerializeField] private EnemyType _enemyType = EnemyType.SideView;
    public Stat spawnWeight = new(GlobalStatsSettingsRepository.EnemyStats.SpawnWeight);

    private EnemyMoveController _enemyMoveController;
    private readonly Rarity _rarity = new Rarity();
    private SpriteOutline _spriteOutline;
    private GameObject _collisionGameObject;
    private Color _spriteColor;
    private const float ReturnToDefaultColorSpeed = 5f;
    private float _deathTimer;
    private const int MinInitialLevel = 1;
    private const float MaxLifeIncreasePerLevel = 1;
    private const long OffscreenDieSeconds = 60;

    private int _level;
    private int Level
    {
        get => _level;
        set
        {
            if (_level != value)
            {
                if (value >= MinInitialLevel)
                {
                    _level = value;
                }
            }
            UpdateMaxLifeStat();
        }
    }
    private void OnEnable() => BaseOnEnable();
    private void OnDisable() => BaseOnDisable();
    private void Awake() => BaseAwake(GlobalStatsSettingsRepository.EnemyStats);
    private void Update() => BaseUpdate();
    private void FixedUpdate() => BaseFixedUpdate();
    private void OnCollisionEnter2D(Collision2D collision)
    {
        _collisionGameObject = collision.gameObject;
        switch (_collisionGameObject.tag)
        {
            case "Projectile":
                TakeDamage(MinimalDamageTaken);
                DropDamagePopup(MinimalDamageTaken, _collisionGameObject.transform.position);
                var collisionEntity = _collisionGameObject.GetComponent<Entity>();
                _enemyMoveController.KnockBackFromTarget(collisionEntity);
                spriteRenderer.color = _enemyType switch
                {
                    EnemyType.AboveView => Color.cyan,
                    EnemyType.SideView => Color.red,
                    _ => throw new ArgumentOutOfRangeException()
                };
                break;
        }
    }
    protected void BaseFixedUpdate()
    {
        DeathTimerFixedUpdate();
        _enemyMoveController.FixedUpdateAccelerationStep();
    }
    protected void BaseAwake(EnemyStatsSettings settings)
    {
        Debug.Log($"{gameObject.name} Enemy Awake");
        base.BaseAwake(settings);

        spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteOutline = GetComponent<SpriteOutline>();
        _spriteColor = spriteRenderer.color;
        _rarity.Value = RarityEnum.Normal;
        Level = MinInitialLevel;
        RestoreLifePoints();
        var player = FindObjectOfType<Player>().gameObject;
        if (_enemyType == EnemyType.SideView)
        {
            _enemyMoveController = new SideViewEnemyMoveController(this, player);
        }   
        else
        {
            _enemyMoveController = new AboveViewEnemyMoveController(this, player);
        }
    }
    protected override void BaseUpdate()
    {
        if (_enemyMoveController is SideViewEnemyMoveController)
        {
            spriteRenderer.flipX = _enemyMoveController.GetMovementDirection().x switch
            {
                < 0 => false,
                > 0 => true,
                _ => spriteRenderer.flipX
            };
        }
        base.BaseUpdate();
        BackToNormalColor();
    }
    private void BackToNormalColor()
    {
        if (spriteRenderer.color == Color.white) return;
        _spriteColor = spriteRenderer.color;
        _spriteColor.r += ReturnToDefaultColorSpeed * Time.deltaTime;
        _spriteColor.g += ReturnToDefaultColorSpeed * Time.deltaTime;
        _spriteColor.b += ReturnToDefaultColorSpeed * Time.deltaTime;
        spriteRenderer.color = _spriteColor;
    }
    private void DeathTimerFixedUpdate()
    {
        if (IsOnScreen)
        {
            _deathTimer = 0; 
            return;
        }
        if (Time.timeScale == 0) return;
        _deathTimer += Time.fixedDeltaTime;
        if (!(_deathTimer >= OffscreenDieSeconds)) return;
        TakeDamage(CurrentLifePoints);
    }
    public void SetRarity(RarityEnum rarityEnum)
    {
        _rarity.Value = rarityEnum;
        if (rarityEnum == RarityEnum.Normal) return;

        var color = _rarity.Color;
        var multiplier = _rarity.Multiplier;

        _spriteOutline.enabled = true;
        _spriteOutline.color = color;
        
        var statMod = new StatModifier(OperationType.Multiplication, multiplier);
        MaximumLifePoints.AddModifier(statMod);
    }
    public void LevelUp(int value)
    {
        if(value < 0) return;
        Level += value;
    }
    public EnemyType GetEnemyType()
    {
        return _enemyType;
    }
    protected override void Death()
    {
        base.Death();
        if (_collisionGameObject == null) return;
        if (_collisionGameObject.tag != "Projectile") return;
        DropBonus();
    }
    private void DropBonus()
    {
        var rotation = new Quaternion(0, 0, 0, 0);
        Instantiate(_onDeathDrop, Rb2D.position, rotation);
    }
    private void DropDamagePopup(int damage, Vector2 positionVector2)
    {
        var droppedDamagePopup = Instantiate(_damagePopup);
        droppedDamagePopup.transform.position = positionVector2;
        droppedDamagePopup.GetComponent<DamagePopup>().Setup(damage);
    }
    private void UpdateMaxLifeStat()
    {
        if (Level <= 1) return;
        var addingValue = (_level - 1) * MaxLifeIncreasePerLevel;
        var mod = new StatModifier(OperationType.Addition, addingValue);
        MaximumLifePoints.AddModifier(mod);
    }
    public void LookAt2D(Vector2 target)
    {
        var direction = (Vector2)Rb2D.transform.position - target;
        var angle = (Mathf.Atan2(direction.y, direction.x) + Mathf.PI / 2) * Mathf.Rad2Deg;
        Rb2D.rotation = angle;
        Rb2D.SetRotation(angle);
    }
}