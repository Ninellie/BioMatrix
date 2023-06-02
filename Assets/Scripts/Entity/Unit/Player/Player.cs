using System;
using Assets.Scripts.Entity.Unit.Movement;
using Assets.Scripts.Entity.Unit.Turret;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Entity.Unit.Player
{
    [RequireComponent(typeof(PlayerStatsSettings))]
    public class Player : Unit
    {
        [SerializeField] private LayerMask _enemyLayer;
        [SerializeField] private GameObject _shield;
        [SerializeField] private SpriteRenderer _shieldSprite;
        [SerializeField] private float _alphaPerLayer = 0.2f;
        [SerializeField] private Color _shieldColor = Color.cyan;
        [SerializeField] private float _shieldRepulseRadius = 250f;
        [SerializeField] private Transform _firePoint;
    
        public PlayerStatsSettings Settings => GetComponent<PlayerStatsSettings>();

        public Stat.Stat MagnetismRadius { get; private set; }
        public Stat.Stat MaxShieldLayersCount { get; private set; }
        public Stat.Stat MaxRechargeableShieldLayersCount { get; private set; }
        public Stat.Stat ShieldLayerRechargeRatePerSecond { get; private set; }

        public Resource ShieldLayers { get; private set; }

        public event Action GamePausedEvent;
        public event Action LevelUpEvent;
        public event Action ExperienceTakenEvent;
        public event Action FireEvent;
        public event Action FireOffEvent;
    
        public TurretHub TurretHub { get; set; }
        public Firearm.Firearm Firearm { get; private set; }
        public bool IsFireButtonPressed { get; private set; } = false;

        public int Level
        {
            get => _level;
            set
            {
                if (value < InitialLevel) throw new ArgumentOutOfRangeException(nameof(value));
                _level = value;
                LevelUpEvent?.Invoke();
            }
        }
        private int _level;
        public int Experience
        {
            get => _experience;
            set
            {
                _experience = value;
                ExperienceTakenEvent?.Invoke();
                if (ExpToLvlup != 0) return;
                _experience = 0;
                Level++;
            }
        }
        private int _experience;
        public int ExpToLvlup => ExperienceToSecondLevel + (Level * ExperienceAmountIncreasingPerLevel) - Experience;
        private const int ExperienceToSecondLevel = 2;
        private const int ExperienceAmountIncreasingPerLevel = 1;
        private const int InitialLevel = 1;
        private const int InitialExperience = 0;

        private MovementControllerPlayer _movementController;
        private CircleCollider2D _circleCollider;
        private CapsuleCollider2D _capsuleCollider;
        private SpriteRenderer _spriteRenderer;
        private Invulnerability _invulnerability;

        private void Awake() => BaseAwake(Settings);

        private void Start() => BaseStart();

        private void OnEnable() => BaseOnEnable();
    
        private void OnDisable() => BaseOnDisable();

        private void FixedUpdate() => BaseFixedUpdate();

        private void OnCollisionEnter2D(Collision2D collision2D) => BaseOnCollisionEnter2D(collision2D);
    
        private void Update() => BaseUpdate();

        private void OnDrawGizmosSelected() => BaseOnDrawGizmosSelected();

        private void OnDrawGizmos() => BaseOnDrawGizmos();

        protected void BaseAwake(PlayerStatsSettings settings)
        {
            Debug.Log($"{gameObject.name} {nameof(Player)} Awake");
            base.BaseAwake(settings);

            _level = InitialLevel;
            _experience = InitialExperience;

            _circleCollider = GetComponent<CircleCollider2D>();
            _capsuleCollider = GetComponent<CapsuleCollider2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _invulnerability = GetComponent<Invulnerability>();
            _shieldSprite = _shield.GetComponent<SpriteRenderer>();

            MaxRechargeableShieldLayersCount = StatFactory.GetStat(settings.maxRechargeableShieldLayersCount);
            MaxShieldLayersCount = StatFactory.GetStat(settings.maxShieldLayersCount);
            ShieldLayerRechargeRatePerSecond = StatFactory.GetStat(settings.shieldLayerRechargeRate / 60f);
            MagnetismRadius = StatFactory.GetStat(settings.magnetismRadius);
        

            _circleCollider.radius = MagnetismRadius.Value;

            if (MagnetismRadius.Value < 0)
            {
                _circleCollider.radius = 0;
            }

            ShieldLayers = new Resource(0, MaxShieldLayersCount, ShieldLayerRechargeRatePerSecond,
                MaxRechargeableShieldLayersCount);

            _movementController = new MovementControllerPlayer(this);
        }

        protected override void BaseOnEnable()
        {
            base.BaseOnEnable();

            foreach (var effect in effects)
            {
                effect.Subscribe(this);
            }

            ShieldLayers.EmptyEvent += UpdateShield;
            ShieldLayers.NotEmptyEvent += UpdateShield;
            ShieldLayers.ValueChangedEvent += UpdateShieldAlpha;

            if (MagnetismRadius != null) MagnetismRadius.ValueChangedEvent += ChangeCurrentMagnetismRadius;

        }

        protected override void BaseOnDisable()
        {
            foreach (var effect in effects)
            {
                effect.Unsubscribe(this);
            }

            ShieldLayers.EmptyEvent -= UpdateShield;
            ShieldLayers.NotEmptyEvent -= UpdateShield;
            ShieldLayers.ValueChangedEvent -= UpdateShieldAlpha;

            if (MagnetismRadius != null) MagnetismRadius.ValueChangedEvent -= ChangeCurrentMagnetismRadius;


            base.BaseOnDisable();
        }

        protected void BaseStart()
        {
            GameTimeScheduler.Schedule(Freeze, 0.2f);

            ShieldLayers.Increase((int)MaxRechargeableShieldLayersCount.Value);
            UpdateShieldAlpha();
            UpdateShield();
        }

        protected void BaseFixedUpdate()
        {
            _movementController.FixedUpdateStep();
        }

        private void BaseOnCollisionEnter2D(Collision2D collision2D)
        {
            Collider2D otherCollider2D = collision2D.collider;
            if (!otherCollider2D.gameObject.TryGetComponent<Entity>(out Entity entity))
            {
                Debug.LogWarning("OnTriggerEnter2D with game object without Entity component");
                return;
            }
            if (!otherCollider2D.gameObject.CompareTag("Enemy") && !otherCollider2D.gameObject.CompareTag("Enclosure")) return;
            if (!ShieldLayers.IsEmpty)
            {
                Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, _shieldRepulseRadius, _enemyLayer);

                foreach (Collider2D collider2d in hitColliders)
                {
                    collider2d.gameObject.GetComponent<Enemy.Enemy>().enemyMoveController.KnockBackFromTarget(KnockbackPower.Value);
                }
                ShieldLayers.Decrease();

                if (!otherCollider2D.gameObject.CompareTag("Enclosure")) return;
                var collisionEnclosure = otherCollider2D.gameObject.GetComponent<Entity>();
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
                    KnockBackToEnclosureCenter(collisionEnclosure);
                }
                _invulnerability.ApplyInvulnerable(collision2D);
            }
        }

        protected override void BaseUpdate()
        {
            base.BaseUpdate();
            ShieldLayers.TimeToRecover += Time.deltaTime;
        }

        private void BaseOnDrawGizmos()
        {
            if (_movementController is null)
            {
                return;
            }
            Gizmos.color = Color.red;
            Gizmos.DrawLine(Rb2D.transform.position, _movementController.GetVelocity() + (Vector2)Rb2D.transform.position);
        }

        private void BaseOnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _shieldRepulseRadius);
        }
    
        protected void KnockBackFrom(Entity collisionEntity)
        {
            _movementController.KnockBackFromEntity(collisionEntity);
        }

        protected void KnockBackToEnclosureCenter(Entity collisionEntity)
        {
            Vector2 entityPosition = collisionEntity.transform.position;

            Camera mainCamera = Camera.main;
            Vector3 cameraPos = mainCamera.transform.position;
            Vector3 cameraTopRight =
                new Vector3(mainCamera.aspect * mainCamera.orthographicSize, mainCamera.orthographicSize, 0f) + cameraPos;
            Vector3 cameraBottomLeft =
                new Vector3(-mainCamera.aspect * mainCamera.orthographicSize, -mainCamera.orthographicSize, 0f) + cameraPos;

            float width = cameraTopRight.x - cameraBottomLeft.x;
            float height = cameraTopRight.y - cameraBottomLeft.y;

            Vector2 addedPos = new Vector2(width / 2, height / 2);

            entityPosition += addedPos;

            _movementController.KnockBackTo(collisionEntity, entityPosition);
        }

        private void UpdateShield()
        {
            var isShieldExists = !ShieldLayers.IsEmpty;
            _capsuleCollider.enabled = isShieldExists;
            _shield.SetActive(isShieldExists);
        }

        private void UpdateShieldAlpha()
        {
            var a = _alphaPerLayer * ShieldLayers.GetValue();
            _shieldColor.a = a;
            _shieldSprite.color = _shieldColor;
        }

        private void Freeze()
        {
            Time.timeScale = 0f;
            this.GetComponent<PlayerInput>().SwitchCurrentActionMap("Menu");
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
            var firearm = w.GetComponent<Firearm.Firearm>();
            Firearm = firearm;
        }

        public void OnMove(InputValue input)
        {
            var inputVector2 = input.Get<Vector2>();
            _movementController.SetDirection(inputVector2);
            _spriteRenderer.flipX = inputVector2.x switch
            {
                < 0 => true,
                > 0 => false,
                _ => _spriteRenderer.flipX
            };
        }

        public void OnFire()
        {
            IsFireButtonPressed = true;
            FireEvent?.Invoke();
        }

        public void OnFireOff()
        {
            IsFireButtonPressed = false;
            FireOffEvent?.Invoke();
        }

        public void OnPause(InputValue input)
        {
            GamePausedEvent?.Invoke();
            Debug.Log("Game on pause");
        }

        public void OnUnpause(InputValue input)
        {
            GamePausedEvent?.Invoke();
            Debug.Log("Game is active");
        }
    }
}