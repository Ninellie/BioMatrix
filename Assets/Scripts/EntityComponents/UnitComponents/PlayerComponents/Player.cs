using System;
using Assets.Scripts.EntityComponents.Resources;
using Assets.Scripts.EntityComponents.Stats;
using Assets.Scripts.EntityComponents.UnitComponents.Knockback;
using Assets.Scripts.EntityComponents.UnitComponents.Turret;
using Assets.Scripts.FirearmComponents;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents
{
    public interface IWeaponBearer
    {
        public GameObject CreateWeapon(GameObject weapon);
    }

    public interface IPlayableCharacter
    {

    }

    public class Player : MonoBehaviour, IWeaponBearer, IPlayableCharacter
    {

        [SerializeField] private Transform _firePoint;
        [SerializeField] private Shield _shield;

        public event Action GamePausedEvent;
        public event Action FireEvent;
        public event Action FireOffEvent;

        public Shield Shield => _shield;
        public TurretHub TurretHub { get; set; }
        public Firearm Firearm { get; private set; }
        public bool IsFireButtonPressed { get; private set; }
        public Vector2 CurrentAimDirection  { get; private set; }

        private const int ExperienceAmountIncreasingPerLevel = 15;

        private CircleCollider2D _circleCollider;
        private Animator _animator;

        private KnockbackController _knockbackController;
        private Invulnerability _invulnerability;
        private StatList _stats;
        private ResourceList _resources;

        private string _currentState;

        private void Awake()
        {
            Debug.Log($"{gameObject.name} {nameof(Player)} Awake");

            _stats = GetComponent<StatList>();
            _resources = GetComponent<ResourceList>();
            _animator = GetComponent<Animator>();
            _circleCollider = GetComponent<CircleCollider2D>();
            _invulnerability = GetComponent<Invulnerability>();
            _knockbackController = GetComponent<KnockbackController>();

            _circleCollider.radius = Math.Max(_stats.GetStatByName(StatName.MagnetismRadius).Value, 0);
        }

        private void OnEnable()
        {
            _stats.GetStatByName(StatName.Size).valueChangedEvent.AddListener(ChangeCurrentSize);
            _stats.GetStatByName(StatName.MagnetismRadius).valueChangedEvent.AddListener(ChangeCurrentMagnetismRadius);
            _resources.GetResourceByName(ResourceName.Health).GetEvent(ResourceEventType.Empty).AddListener(Death);
            _resources.GetResourceByName(ResourceName.Experience).GetEvent(ResourceEventType.Fill).AddListener(LevelUp);
        }

        private void OnDisable()
        {
            _stats.GetStatByName(StatName.Size).valueChangedEvent.RemoveListener(ChangeCurrentSize);
            _stats.GetStatByName(StatName.MagnetismRadius).valueChangedEvent.RemoveListener(ChangeCurrentMagnetismRadius);
            _resources.GetResourceByName(ResourceName.Health).GetEvent(ResourceEventType.Empty).RemoveListener(Death);
            _resources.GetResourceByName(ResourceName.Experience).GetEvent(ResourceEventType.Fill).RemoveListener(LevelUp);
        }

        private void OnCollisionEnter2D(Collision2D collision2D)
        {
            var otherCollider2D = collision2D.collider;
            if (!otherCollider2D.gameObject.TryGetComponent<Entity>(out var entity))
            {
                Debug.LogWarning("OnTriggerEnter2D with game object without Entity component", this);
                return;
            }

            otherCollider2D.gameObject.TryGetComponent<StatList>(out var enemyStats);

            var isEnemy = otherCollider2D.gameObject.CompareTag("Enemy");
            var isEnclosure = otherCollider2D.gameObject.CompareTag("Enclosure");

            if (!isEnemy && !isEnclosure) return;

            if (!Shield.LayersCount.IsEmpty)
                Shield.LayersCount.Decrease();
            else
            {
                var damageValue = (int)enemyStats.GetStatByName(StatName.Damage).Value;
                TakeDamage(damageValue);

                if (_resources.GetResourceByName(ResourceName.Health).IsEmpty) return;

                _invulnerability.ApplyInvulnerable();

                if (isEnemy) KnockBackFromEntity(entity);
            }
        }

        public GameObject CreateWeapon(GameObject weapon)
        {
            var w = Instantiate(weapon);

            w.transform.SetParent(_firePoint);

            w.transform.position = _firePoint.transform.position;
            var firearm = w.GetComponent<Firearm>();
            firearm.SetHolder(this);
            Firearm = firearm;
            return w;
        }

        public void IncreaseExperience(int value)
        {
            var expMultiplierValue = (int)_stats.GetStatByName(StatName.ExperienceMultiplier).Value;
            var expTakenAmount = value * expMultiplierValue;
            _resources.GetResourceByName(ResourceName.Experience).Increase(expTakenAmount);
        }

        private void LevelUp()
        {
            var statMod = new StatMod
            {
                Value = ExperienceAmountIncreasingPerLevel, // TODO add this to stats
                Type = OperationType.Addition
            };
            _stats.GetStatByName(StatName.ExperienceToNewLevel).AddModifier(statMod);
            _resources.GetResourceByName(ResourceName.Level).Increase();
            _resources.GetResourceByName(ResourceName.Experience).Empty();
        }

        private void KnockBackFromEntity(Entity entity)
        {
            var magnitude = entity.KnockbackPower.Value;
            var direction = ((Vector2)transform.position - (Vector2)entity.transform.position).normalized;
            var force = direction * magnitude;

            _knockbackController.Knockback(force);
        }

        private void Death()
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }

        private void TakeDamage(int amount)
        {
            _resources.GetResourceByName(ResourceName.Health).Decrease(amount);
            Debug.Log("Damage is taken " + gameObject.name);
        }

        private void ChangeCurrentSize()
        {
            var sizeValue = _stats.GetStatByName(StatName.Size).Value;
            transform.localScale = new Vector3(sizeValue, sizeValue, 1);
        }

        private void ChangeCurrentMagnetismRadius()
        {
            var magnetismRadiusValue = _stats.GetStatByName(StatName.MagnetismRadius).Value;
            _circleCollider.radius = Math.Max(magnetismRadiusValue, 0);
        }

        private void ChangeAnimationState(string newState)
        {
            if (_currentState == newState) return;
            _animator.Play(newState);
        }

        public void OnMove(InputValue input)
        {
            ChangeAnimationState("Run");
            if (input.Get<Vector2>() == Vector2.zero)
                ChangeAnimationState("Idle");
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