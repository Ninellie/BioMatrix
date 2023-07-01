using System;
using Assets.Scripts.Entity.Stats;
using Assets.Scripts.Entity.Unit.Movement;
using Assets.Scripts.Entity.Unit.Turret;
using Assets.Scripts.Weapons;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Assets.Scripts.Entity.Unit.Player
{
    [RequireComponent(typeof(PlayerStatsSettings))]
    [RequireComponent(typeof(Shield))]
    public class Player : Unit
    {
        [SerializeField] private Transform _firePoint;
    
        public PlayerStatsSettings Settings => GetComponent<PlayerStatsSettings>();

        public Stat MagnetismRadius { get; private set; }
        public Stat ExpMultiplier { get; private set; }
        public Stat ExpToNextLvl { get; set; }

        public event Action GamePausedEvent;
        public event Action ExperienceTakenEvent;
        public event Action FireEvent;
        public event Action FireOffEvent;
    
        public Shield Shield { get; private set; }
        public TurretHub TurretHub { get; set; }
        public Firearm Firearm { get; private set; }
        public bool IsFireButtonPressed { get; private set; }

        public Resource Lvl { get; set; }
        public Resource Exp { get; set; }

        private const int ExperienceToSecondLevel = 20;
        private const int ExperienceAmountIncreasingPerLevel = 5;
        private const int InitialLevel = 1;

        private MovementControllerPlayer _movementController;
        private Invulnerability _invulnerability;

        private CircleCollider2D _circleCollider;
        private SpriteRenderer _spriteRenderer;

        private void Awake() => BaseAwake(Settings);

        private void Start() => BaseStart();

        private void OnEnable() => BaseOnEnable();
    
        private void OnDisable() => BaseOnDisable();

        private void FixedUpdate() => BaseFixedUpdate();

        private void OnCollisionEnter2D(Collision2D collision2D) => BaseOnCollisionEnter2D(collision2D);

        private void Update() => BaseUpdate();

        private void OnDrawGizmos() => BaseOnDrawGizmos();

        protected void BaseAwake(PlayerStatsSettings settings)
        {
            Debug.Log($"{gameObject.name} {nameof(Player)} Awake");
            base.BaseAwake(settings);

            _circleCollider = GetComponent<CircleCollider2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _invulnerability = GetComponent<Invulnerability>();
            Shield = GetComponent<Shield>();

            MagnetismRadius = StatFactory.GetStat(settings.magnetismRadius);
            ExpMultiplier = StatFactory.GetStat(settings.expMultiplier);
            ExpToNextLvl = StatFactory.GetStat(ExperienceToSecondLevel);

            _circleCollider.radius = Math.Max(MagnetismRadius.Value, 0);

            Lvl = new Resource(InitialLevel);
            Exp = new Resource(ExpToNextLvl);

            _movementController = new MovementControllerPlayer(this);
        }

        protected override void BaseOnEnable()
        {
            base.BaseOnEnable();

            foreach (var effect in effects)
            {
                effect.Subscribe(this);
            }

            MagnetismRadius.ValueChangedEvent += ChangeCurrentMagnetismRadius;

            Exp.FillEvent += LevelUp;
        }

        protected override void BaseOnDisable()
        {
            foreach (var effect in effects)
            {
                effect.Unsubscribe(this);
            }

            MagnetismRadius.ValueChangedEvent -= ChangeCurrentMagnetismRadius;

            Exp.FillEvent -= LevelUp;

            base.BaseOnDisable();
        }

        protected void BaseStart()
        {
            
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

            if (Shield.LayersCount.IsEmpty)
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
                Shield.LayersCount.Decrease(1);
            }

            if (isEnclosure)
            {
                KnockBackToEnclosureCenter(entity);
            }
        }

        protected override void BaseUpdate()
        {
            base.BaseUpdate();
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

        public void CreateWeapon(GameObject weapon)
        {
            var w = Instantiate(weapon);

            w.transform.SetParent(_firePoint);

            w.transform.position = _firePoint.transform.position;
            var firearm = w.GetComponent<Firearm>();
            firearm.SetHolder(this);
            Firearm = firearm;
        }

        public void IncreaseExperience(int value)
        {
            var expTakenAmount = (int)(value * ExpMultiplier.Value);
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