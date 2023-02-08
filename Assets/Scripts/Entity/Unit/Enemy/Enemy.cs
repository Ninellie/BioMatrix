using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Enemy : Unit
{
    [SerializeField] private GameObject _onDeathDrop;
    [SerializeField] private GameObject _damagePopup;

    public Stat spawnWeight = new(GlobalStatsSettingsRepository.EnemyStats.SpawnWeight);

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
    private void Start() => BaseStart();
    private void Update() => BaseUpdate();
    private void FixedUpdate()
    {
        DeathTimerFixedUpdate();
        BaseFixedUpdate();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _collisionGameObject = collision.gameObject;
        switch (_collisionGameObject.tag)
        {
            case "Player":
                if (collision.collider is BoxCollider2D)
                {
                    //TakeDamage(CurrentLifePoints);
                    var collisionPlayerEntity = _collisionGameObject.GetComponent<Entity>();
                    KnockBack(collisionPlayerEntity);
                }
                break;
            case "Projectile":
                TakeDamage(MinimalDamageTaken);
                DropDamagePopup(MinimalDamageTaken, _collisionGameObject.transform.position);
                var collisionEntity = _collisionGameObject.GetComponent<Entity>();
                KnockBackFromPlayer(collisionEntity);
                _spriteRenderer.color = Color.cyan;
                break;
            case "Enemy":
                break;
        }
    }
    protected void BaseAwake(EnemyStatsSettings settings)
    {
        Debug.Log($"{gameObject.name} Enemy Awake");

        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteOutline = GetComponent<SpriteOutline>();
        _spriteColor = _spriteRenderer.color;

        var movement = new Movement(this, MovementState.Seek, settings.Speed);
        movement.SetPursuingTarget(FindObjectOfType<Player>().gameObject);

        base.BaseAwake(settings, movement);

        _rarity.Value = RarityEnum.Normal;

        Level = MinInitialLevel;
        RestoreLifePoints();
    }
    protected override void BaseUpdate()
    {
        base.BaseUpdate();
        BackToNormalColor();
    }
    private void BackToNormalColor()
    {
        if (_spriteRenderer.color == Color.white) return;
        _spriteColor = _spriteRenderer.color;
        _spriteColor.r += ReturnToDefaultColorSpeed * Time.deltaTime;
        _spriteColor.g += ReturnToDefaultColorSpeed * Time.deltaTime;
        _spriteColor.b += ReturnToDefaultColorSpeed * Time.deltaTime;
        _spriteRenderer.color = _spriteColor;
    }
    private void DeathTimerFixedUpdate()
    {
        if (IsOnScreen)
        {
            _deathTimer = 0; 
            return;
        }
        //if (Time.timeScale == 0) return;
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
        if (Level <= 1) return;
        var addingValue = (_level - 1) * MaxLifeIncreasePerLevel;
        var mod = new StatModifier(OperationType.Addition, addingValue);
        MaximumLifePoints.AddModifier(mod);
    }
    public void LookAt2D(Vector2 target)
    {
        var direction = (Vector2)_rigidbody2D.transform.position - target;
        var angle = (Mathf.Atan2(direction.y, direction.x) + Mathf.PI / 2) * Mathf.Rad2Deg;
        _rigidbody2D.SetRotation(angle);
    }
}