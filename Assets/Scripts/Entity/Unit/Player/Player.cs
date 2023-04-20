using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Unit
{
    public Action onGamePaused;
    public Action onLevelUp;
    public Action onExperienceTaken;
    public PlayerStatsSettings Settings => GetComponent<PlayerStatsSettings>();
    protected Stat MagnetismRadius { get; private set; }
    protected Stat MagnetismPower { get; private set; }
    [SerializeField] private int _currentShieldLayer = 3;
    //[SerializeField] private int _maxShieldLayers = 3;
    [SerializeField] private SpriteRenderer _shieldSprite;
    [SerializeField] private float _alphaPerLayer = 0.2f;
    [SerializeField] private Color _shieldColor = Color.cyan;
    [SerializeField] private float _shieldRepulseRadius = 250f;
    [SerializeField] private LayerMask _enemyLayer;

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

    public Firearm CurrentFirearm { get; private set; }

    [SerializeField] private Transform _firePoint;
    [SerializeField] private GameObject _shield;


    private const int ExperienceToSecondLevel = 2;
    private const int ExperienceAmountIncreasingPerLevel = 1;
    private const int InitialLevel = 1;
    private const int InitialExperience = 0;

    private MovementControllerPlayer _movementController;
    private int _level;
    private int _experience;
    private CircleCollider2D _circleCollider;
    private CapsuleCollider2D _capsuleCollider;
    //private PointEffector2D _pointEffector;
    private GameTimer _freezeTimer;
    private SpriteRenderer SpriteRenderer => GetComponent<SpriteRenderer>();
    private void Awake() => BaseAwake(Settings);

    private void Start()
    {
        _freezeTimer = new GameTimer(Freeze, 0.2f);
        UpdateShieldAlpha();
        _capsuleCollider.enabled = _currentShieldLayer < 0;
    }

    private void AddLayer()
    {
        if (_currentShieldLayer == 0)
        {
            _capsuleCollider.enabled = true;
            _shield.SetActive(true);
        }
        _currentShieldLayer++;
        UpdateShieldAlpha();
    }

    private void UpdateShieldAlpha()
    {
        var a = _alphaPerLayer * _currentShieldLayer;
        _shieldColor.a = a;
        _shieldSprite.color = _shieldColor;
    }

    private void RemoveLayer()
    {
        if (_currentShieldLayer == 0) return;
        _currentShieldLayer--;
        UpdateShieldAlpha();
        if (_currentShieldLayer != 0) return;
        _capsuleCollider.enabled = false;
        _shield.SetActive(false);
    }
    private void Freeze()
    {
        Time.timeScale = 0f;
        this.GetComponent<PlayerInput>().SwitchCurrentActionMap("Menu");
        FindObjectOfType<GameSessionTimer>().Stop();
    }

    private void OnEnable() => BaseOnEnable();
    private void OnDisable() => BaseOnDisable();
    private void Update() => BaseUpdate();
    private void FixedUpdate() => BaseFixedUpdate();

    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        Collider2D otherCollider2D = collision2D.collider;
        if (!otherCollider2D.gameObject.TryGetComponent<Entity>(out Entity entity))
        {
            Debug.LogWarning("OnTriggerEnter2D with game object without Entity component");
            return;
        }
        if (!otherCollider2D.gameObject.CompareTag("Enemy") && !otherCollider2D.gameObject.CompareTag("Enclosure")) return;
        if (_currentShieldLayer > 0)
        {
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, _shieldRepulseRadius, _enemyLayer);

            foreach (Collider2D collider in hitColliders)
            {
                collider.gameObject.GetComponent<Enemy>()._enemyMoveController.KnockBackFromTarget(KnockbackPower.Value);
            }
            RemoveLayer();

            if (!otherCollider2D.gameObject.CompareTag("Enclosure")) return;
            var collisionEnclosure = otherCollider2D.gameObject.GetComponent<Entity>();
            //Vector2 collisionPoint = GetCollisionPoint(collision2D);
            //KnockBackFrom(collisionEnemy, collisionPoint);
            KnockBackToEnclosureCenter(collisionEnclosure);
        }
        else
        {
            if (otherCollider2D.gameObject.CompareTag("Enemy"))
            {
                var collisionEnemy = otherCollider2D.gameObject.GetComponent<Entity>();
                var enemyDamage = collisionEnemy.Damage.Value;
                TakeDamage(enemyDamage);
                KnockBackFrom(collisionEnemy);
            }
            if (otherCollider2D.gameObject.CompareTag("Enclosure"))
            {
                var collisionEnclosure = otherCollider2D.gameObject.GetComponent<Entity>();
                var enclosureDamage = collisionEnclosure.Damage.Value;
                TakeDamage(enclosureDamage);
                //Vector2 collisionPoint = GetCollisionPoint(collision2D);
                KnockBackToEnclosureCenter(collisionEnclosure);
            }

        }
    }
    public Vector2 GetCollisionPoint(Collision2D collision)
    {
        if (collision.contacts.Length > 0)
        {
            return collision.contacts[0].point;
        }

        Debug.LogError("No contact points found in collision!");
        return Vector2.zero;
    }

    //private void OnTriggerEnter2D(Collider2D otherCollider2D)
    //{
    //    if (!otherCollider2D.gameObject.TryGetComponent<Entity>(out Entity entity))
    //    {
    //        Debug.LogWarning("OnTriggerEnter2D with game object without Entity component");
    //        return;
    //    }
    //    if (!otherCollider2D.gameObject.CompareTag("Enemy") && !otherCollider2D.gameObject.CompareTag("Enclosure")) return;
    //    if (_currentShieldLayer > 0)
    //    {
    //        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, _shieldRepulseRadius, _enemyLayer);

    //        foreach (Collider2D collider in hitColliders)
    //        {
    //            collider.gameObject.GetComponent<Enemy>()._enemyMoveController.KnockBackFromTarget(KnockbackPower.Value);
    //        }
    //        RemoveLayer();
    //        if (!otherCollider2D.gameObject.CompareTag("Enclosure")) return;
    //        var collisionEnemy = otherCollider2D.gameObject.GetComponent<Entity>();
    //        var enemyDamage = collisionEnemy.Damage.Value;
    //        TakeDamage(enemyDamage);
    //        KnockBackFrom(collisionEnemy);
    //    }
    //    else
    //    {
    //        var collisionEnemy = otherCollider2D.gameObject.GetComponent<Entity>();
    //        var enemyDamage = collisionEnemy.Damage.Value;
    //        TakeDamage(enemyDamage);
    //        KnockBackFrom(collisionEnemy);
    //    }
    //}
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _shieldRepulseRadius);
    }
    private void OnDrawGizmos()
    {
        if (_movementController is null)
        {
            return;
        }
        Gizmos.color = Color.red;
        Gizmos.DrawLine(Rb2D.transform.position, _movementController.GetVelocity() + (Vector2)Rb2D.transform.position);
    }
    protected void KnockBackFrom(Entity collisionEntity)
    {
        _movementController.KnockBackFromEntity(collisionEntity);
    }
    protected void KnockBackFrom(Entity collisionEntity, Vector2 position)
    {
        _movementController.KnockBackFromPosition(collisionEntity, position);
    }
    protected void KnockBackToEnclosureCenter(Entity collisionEntity)
    {
        Vector2 entityPosition = collisionEntity.transform.position;

        Camera mainCamera = Camera.main;
        Vector3 cameraPos = mainCamera.transform.position;
        Vector3 cameraTopRight = new Vector3(mainCamera.aspect * mainCamera.orthographicSize, mainCamera.orthographicSize, 0f) + cameraPos;
        Vector3 cameraBottomLeft = new Vector3(-mainCamera.aspect * mainCamera.orthographicSize, -mainCamera.orthographicSize, 0f) + cameraPos;

        float width = cameraTopRight.x - cameraBottomLeft.x;
        float height = cameraTopRight.y - cameraBottomLeft.y;

        Vector2 addedPos = new Vector2(width / 2, height / 2);

        entityPosition += addedPos;

        _movementController.KnockBackTo(collisionEntity, entityPosition);
    }
    protected void BaseFixedUpdate()
    {
        _movementController.FixedUpdateStep();
    }
    protected void BaseAwake(PlayerStatsSettings settings)
    {
        Debug.Log($"{gameObject.name} Player Awake");
        _level = InitialLevel;
        _experience = InitialExperience;
        _circleCollider = GetComponent<CircleCollider2D>();
        _capsuleCollider = GetComponent<CapsuleCollider2D>();
        //_capsuleCollider.enabled = false;
        //_pointEffector = GetComponent<PointEffector2D>();
        MagnetismRadius = new Stat(settings.magnetismRadius);
        _circleCollider.radius = MagnetismRadius.Value;
        if (MagnetismRadius.Value < 0)
        {
            _circleCollider.radius = 0;
        }
        MagnetismPower = new Stat(settings.magnetismPower);
        //_pointEffector.forceMagnitude = MagnetismPower.Value * -1;

        _shieldSprite = _shield.GetComponent<SpriteRenderer>();
        base.BaseAwake(settings);

        _movementController = new MovementControllerPlayer(this);
    }

    protected override void BaseUpdate()
    {
        base.BaseUpdate();
        _freezeTimer.Update();
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
        //_pointEffector.forceMagnitude = MagnetismPower.Value * -1;
    }
    private void ChangeCurrentMagnetismRadius()
    {
        if (MagnetismRadius.Value < 0)
        {
            _circleCollider.radius = 0;
        }
        _circleCollider.radius = MagnetismRadius.Value;
    }
    public void CreateWeapon(GameObject weapon)
    {
        var w = Instantiate(weapon);

        w.transform.SetParent(_firePoint);

        w.transform.position = _firePoint.transform.position;
        var firearm = w.GetComponent<Firearm>();
        CurrentFirearm = firearm;
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
        _movementController.SetDirection(inputVector2);
        SpriteRenderer.flipX = inputVector2.x switch
        {
            < 0 => true,
            > 0 => false,
            _ => SpriteRenderer.flipX
        };
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