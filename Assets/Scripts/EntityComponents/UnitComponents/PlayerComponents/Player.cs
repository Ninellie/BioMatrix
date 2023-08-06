using System;
using Assets.Scripts.EntityComponents.Stats;
using Assets.Scripts.EntityComponents.UnitComponents.Knockback;
using Assets.Scripts.EntityComponents.UnitComponents.Movement;
using Assets.Scripts.EntityComponents.UnitComponents.Turret;
using Assets.Scripts.FirearmComponents;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents
{
    [RequireComponent(typeof(PlayerStatsSettings))]
    public class Player : Unit
    {
        [SerializeField] private Transform _firePoint;

        public PlayerStatsSettings Settings => GetComponent<PlayerStatsSettings>();

        public OldStat MagnetismRadius { get; private set; }
        public OldStat ExpMultiplier { get; private set; }
        public OldStat ExpToNextLvl { get; set; }

        public event Action GamePausedEvent;
        public event Action ExperienceTakenEvent;
        public event Action FireEvent;
        public event Action FireOffEvent;
    
        public Shield Shield { get; private set; }
        public TurretHub TurretHub { get; set; }
        public Firearm Firearm { get; private set; }
        public bool IsFireButtonPressed { get; private set; }
        public Vector2 CurrentAimDirection  { get; private set; }

        public OldResource Lvl { get; set; }
        public OldResource Exp { get; set; }

        private const int ExperienceToSecondLevel = 20;
        private const int ExperienceAmountIncreasingPerLevel = 15;
        private const int InitialLevel = 1;

        private KnockbackController _knockbackController;
        private Invulnerability _invulnerability;

        private CircleCollider2D _circleCollider;
        private Animator _animator;
        private string _currentState;
        private StatList _statList;

        private void Awake() => BaseAwake(Settings);

        private void OnEnable() => BaseOnEnable();
    
        private void OnDisable() => BaseOnDisable();

        private void OnCollisionEnter2D(Collision2D collision2D) => BaseOnCollisionEnter2D(collision2D);

        private void Update() => BaseUpdate();

        protected void BaseAwake(PlayerStatsSettings settings)
        {
            _statList = GetComponent<StatList>();
            Debug.Log($"{gameObject.name} {nameof(Player)} Awake");
            base.BaseAwake(settings);

            _animator = GetComponent<Animator>();
            _circleCollider = GetComponent<CircleCollider2D>();

            _invulnerability = GetComponent<Invulnerability>();
            Shield = GetComponentInChildren<Shield>();

            _knockbackController = GetComponent<KnockbackController>();

            MagnetismRadius = StatFactory.GetStat(settings.magnetismRadius);
            ExpMultiplier = StatFactory.GetStat(settings.expMultiplier);
            ExpToNextLvl = StatFactory.GetStat(ExperienceToSecondLevel);

            _circleCollider.radius = Math.Max(MagnetismRadius.Value, 0);

            Lvl = new OldResource(InitialLevel);
            Exp = new OldResource(ExpToNextLvl);

            //_oldMovementController = new OldMovementControllerPlayer(this);
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

                if (LifePoints.IsEmpty)
                    return;
                
                _invulnerability.ApplyInvulnerable();

                if (isEnemy)
                    KnockBackFromEntity(entity);
                
            }
            else Shield.LayersCount.Decrease(1);

            if (isEnclosure)
                KnockBackToEnclosureCenter(entity);
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

            _knockbackController.Knockback(force);
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

            _knockbackController.Knockback(force);
        }

        private void ChangeCurrentMagnetismRadius()
        {
            _circleCollider.radius = Math.Max(MagnetismRadius.Value, 0);
        }

        private void ChangeAnimationState(string newState)
        {
            if (_currentState == newState) return;
            _animator.Play(newState);
        }

        public void OnMove(InputValue input)
        {
            ChangeAnimationState("Run");
            var inputVector2 = input.Get<Vector2>();
            if (inputVector2 == Vector2.zero)
            {
                ChangeAnimationState("Idle");
            }
        }

        public void OnFirePosition(InputValue input)
        {
            var inputVector2 = input.Get<Vector2>();
            inputVector2 = (Camera.main.ScreenToWorldPoint(inputVector2) - gameObject.transform.position).normalized;
            CurrentAimDirection = inputVector2;
        }

        public void OnFire()
        {
            const float animationFireSpeed = 0.5f;
            _animator.speed = animationFireSpeed;
            IsFireButtonPressed = true;
            FireEvent?.Invoke();
        }

        public void OnFireOff()
        {
            const int animationFireSpeed = 1;
            _animator.speed = animationFireSpeed;
            IsFireButtonPressed = false;
            FireOffEvent?.Invoke();
        }

        public void OnPause()
        {
            GamePausedEvent?.Invoke();
            Debug.Log("Game on pause");
        }

        public void OnUnpause()
        {
            GamePausedEvent?.Invoke();
            Debug.Log("Game is active");
        }
    }
}