using System;
using Assets.Scripts.Entity.Stat;
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
        public Stat.Stat ExpMultiplier { get; private set; }

        public Resource ShieldLayers { get; private set; }

        public event Action GamePausedEvent;

        public event Action ExperienceTakenEvent;

        public event Action FireEvent;
        public event Action FireOffEvent;
    
        public TurretHub TurretHub { get; set; }
        public Firearm.Firearm Firearm { get; private set; }
        public bool IsFireButtonPressed { get; private set; } = false;

        public Resource Lvl { get; set; }
        public Resource Exp { get; set; }
        public Stat.Stat ExpToNextLvl { get; set; }

        private const int ExperienceToSecondLevel = 20;
        private const int ExperienceAmountIncreasingPerLevel = 5;
        private const int InitialLevel = 1;

        private MovementControllerPlayer _movementController;
        private Invulnerability _invulnerability;

        private CircleCollider2D _circleCollider;
        private CapsuleCollider2D _capsuleCollider;
        private SpriteRenderer _spriteRenderer;

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

            //_level = InitialLevel;
            //_experience = InitialExperience;

            _circleCollider = GetComponent<CircleCollider2D>();
            _capsuleCollider = GetComponent<CapsuleCollider2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _invulnerability = GetComponent<Invulnerability>();
            _shieldSprite = _shield.GetComponent<SpriteRenderer>();

            MaxRechargeableShieldLayersCount = StatFactory.GetStat(settings.maxRechargeableShieldLayersCount);
            MaxShieldLayersCount = StatFactory.GetStat(settings.maxShieldLayersCount);
            ShieldLayerRechargeRatePerSecond = StatFactory.GetStat(settings.shieldLayerRechargeRate / 60f);
            MagnetismRadius = StatFactory.GetStat(settings.magnetismRadius);
            ExpMultiplier = StatFactory.GetStat(settings.expMultiplier);
            ExpToNextLvl = StatFactory.GetStat(ExperienceToSecondLevel);

            _circleCollider.radius = Math.Max(MagnetismRadius.Value, 0);

            Lvl = new Resource(InitialLevel);
            Exp = new Resource(0, ExpToNextLvl);

            ShieldLayers = new Resource(0, 
                MaxShieldLayersCount,
                ShieldLayerRechargeRatePerSecond,
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

            MagnetismRadius.ValueChangedEvent += ChangeCurrentMagnetismRadius;

            Exp.FillEvent += LevelUp;
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

            MagnetismRadius.ValueChangedEvent -= ChangeCurrentMagnetismRadius;

            Exp.FillEvent -= LevelUp;

            base.BaseOnDisable();
        }

        protected void BaseStart()
        {
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
            var otherCollider2D = collision2D.collider;
            if (!otherCollider2D.gameObject.TryGetComponent<Entity>(out var entity))
            {
                Debug.LogWarning("OnTriggerEnter2D with game object without Entity component", this);
                return;
            }

            var isEnemy = otherCollider2D.gameObject.CompareTag("Enemy");
            var isEnclosure = otherCollider2D.gameObject.CompareTag("Enclosure");

            if (!isEnemy && !isEnclosure) return;

            if (ShieldLayers.IsEmpty)
            {
                TakeDamage(entity.Damage.Value);
                _invulnerability.ApplyInvulnerable();

                if (isEnemy)
                {
                    KnockBackFromEntity(entity);
                }
            }
            else
            {
                ShieldRepulse();
            }

            if (isEnclosure)
            {
                KnockBackToEnclosureCenter(entity);
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

        public void CreateWeapon(GameObject weapon)
        {
            var w = Instantiate(weapon);

            w.transform.SetParent(_firePoint);

            w.transform.position = _firePoint.transform.position;
            var firearm = w.GetComponent<Firearm.Firearm>();
            firearm.SetHolder(this);
            Firearm = firearm;
        }

        public void GetExperience(int value)
        {
            var expTakenAmount = value * (int)ExpMultiplier.Value;
            Exp.Increase(expTakenAmount);
            ExperienceTakenEvent?.Invoke();
        }

        private void LevelUp()
        {
            Lvl.Increase();
            var mod = new StatModifier(OperationType.Addition, ExperienceAmountIncreasingPerLevel);
            ExpToNextLvl.AddModifier(mod);
            Exp.Empty();
        }

        private void KnockBackFromEntity(Entity entity)
        {
            var magnitude = entity.KnockbackPower.Value;
            var direction = ((Vector2)transform.position - (Vector2)entity.transform.position).normalized;
            var force = direction * magnitude;

            _movementController.KnockBack(force);
        }

        private void KnockBackToEnclosureCenter(Entity entity)
        {
            Vector2 entityPosition = entity.transform.position;

            var mainCamera = Camera.main;
            var cameraPos = mainCamera.transform.position;
            var cameraTopRight =
                new Vector3(mainCamera.aspect * mainCamera.orthographicSize, mainCamera.orthographicSize, 0f) + cameraPos;
            var cameraBottomLeft =
                new Vector3(-mainCamera.aspect * mainCamera.orthographicSize, -mainCamera.orthographicSize, 0f) + cameraPos;

            var width = cameraTopRight.x - cameraBottomLeft.x;
            var height = cameraTopRight.y - cameraBottomLeft.y;

            var addedPos = new Vector2(width / 2, height / 2);
            entityPosition += addedPos;

            var magnitude = entity.KnockbackPower.Value;
            var direction = (entityPosition - (Vector2)transform.position).normalized;
            var force = direction * magnitude;

            _movementController.KnockBack(force);
        }

        private void ChangeCurrentMagnetismRadius()
        {
            _circleCollider.radius = Math.Max(MagnetismRadius.Value, 0);
        }

        private void ShieldRepulse()
        {
            var colliders2D = Physics2D.OverlapCircleAll(transform.position, _shieldRepulseRadius, _enemyLayer);

            foreach (var collider2d in colliders2D)
            {
                collider2d.gameObject.GetComponent<Enemy.Enemy>().enemyMoveController
                    .KnockBackFromTarget(KnockbackPower.Value);
            }

            ShieldLayers.Decrease();
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