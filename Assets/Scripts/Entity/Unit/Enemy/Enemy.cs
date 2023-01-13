using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Enemy : Unit
{
    [SerializeField] private GameObject _onDeathDrop;
    [SerializeField] private GameObject _damagePopup;

    public Stat spawnWeight = new(GlobalStatsSettingsRepository.EnemyStats.SpawnWeight);

    private readonly Rarity _rarity = new Rarity();
    private GameObject _collisionGameObject;
    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _spriteRenderer;
    private readonly Stopwatch _deathTimer = new Stopwatch();
    private bool _isPaused = false;
    private const int MinInitialLevel = 1;
    private const float MaxLifeIncreasePerLevel = 1;
    private const long OffscreenDieMilliseconds = 10000;
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
                if (collision.collider is BoxCollider2D)
                {
                    TakeDamage(CurrentLifePoints);
                }
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
        Subscription();
    }
    protected override void BaseUpdate()
    {
        base.BaseUpdate();
        DeathTimerCheck();
    }
    public void Subscription()
    {
        FindObjectOfType<Player>().onGamePaused += OnPause;
        FindObjectOfType<Player>().onDeath += Unsubscription;
    }
    private void Unsubscription()
    {
        FindObjectOfType<Player>().onGamePaused -= OnPause;
        FindObjectOfType<Player>().onDeath -= Unsubscription;
    }
    private void DeathTimerCheck()
    {
        if (_isPaused) { return; }

        if (!IsOnScreen)
        {
            if (!_deathTimer.IsRunning)
            {
                if (_deathTimer.IsRunning) return;
                Debug.Log($"Death timer of {gameObject.name} started");
                _deathTimer.Start();
            }
            else
            {
                if (_deathTimer.ElapsedMilliseconds < OffscreenDieMilliseconds) return;
                TakeDamage(CurrentLifePoints);
                Debug.Log($"Unit named {gameObject.name} died because it was off screen for too long");
            }
        }
        else
        {
            if (_deathTimer.IsRunning)
            {
                _deathTimer.Reset();
            }
        }
    }
    private void OnPause()
    {
        if (_deathTimer.IsRunning)
        {
            _isPaused = true;
            _deathTimer.Stop();
        }
        else
        {
            _isPaused = false;
            _deathTimer.Start();
        }

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
        if(value < 0) return;
        Level += value;
    }
    protected override void Death()
    {
        base.Death();
        if (_collisionGameObject != null && _collisionGameObject.tag == "Projectile")
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