using System;
using Assets.Scripts.EntityComponents.Resources;
using Assets.Scripts.EntityComponents.Stats;
using Assets.Scripts.EntityComponents.UnitComponents.EnemyComponents;
using Assets.Scripts.EntityComponents.UnitComponents.Knockback;
using Assets.Scripts.EntityComponents.UnitComponents.ProjectileComponents;
using Assets.Scripts.FirearmComponents;
using Assets.Scripts.GameSession.UIScripts;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents
{
    public interface IWeaponBearer
    {
        Transform GetFirePoint();
        GameObject CreateWeapon(GameObject weapon);
        IWeapon GetWeapon();
        void Shoot();
    }

    public interface IAiming
    {
        void SetAimMode(AimMode mode);
    }

    public interface IPlayableCharacter
    {

    }

    public class Player : MonoBehaviour, IWeaponBearer, IPlayableCharacter, ISlayer, ISource, IAiming
    {
        [SerializeField] private Transform _firePoint;
        [SerializeField] private Shield _shield;

        public event Action GamePausedEvent;
        public event Action FireEvent;
        public event Action FireOffEvent;

        public AimMode aimMode = AimMode.AutoAim;

        public Shield Shield => _shield;
        public Firearm Firearm { get; private set; }
        public bool IsFireButtonPressed { get; private set; }
        public Vector2 CurrentAimDirection  { get; private set; }
        private const int ExperienceAmountIncreasingPerLevel = 16;

        private CircleCollider2D _circleCollider;
        private Animator _animator;

        private KnockbackController _knockbackController;
        private Invulnerability _invulnerability;
        private StatList _stats;
        private ResourceList _resources;
        private string _currentState;
        private bool _isSubscribed;

        private bool _isCaged;
        private float _cageTime = 15f;

        private void Awake()
        {
            Debug.Log($"{gameObject.name} {nameof(Player)} Awake");

            _stats = GetComponent<StatList>();
            _resources = GetComponent<ResourceList>();
            _invulnerability = GetComponent<Invulnerability>();
            _knockbackController = GetComponent<KnockbackController>();
            _circleCollider = GetComponent<CircleCollider2D>();
            _animator = GetComponent<Animator>();

            aimMode = AimMode.AutoAim;
            CurrentAimDirection = Vector2.zero;
        }

        private void Start()
        {
            Subscribe();
            UpdateMagnetismRadius();
            UpdateSize();
        }

        private void OnEnable() => Subscribe();

        private void OnDisable() => Unsubscribe();

        private void Subscribe()
        {
            if (_isSubscribed) return;

            var sizeStat = _stats.GetStat(StatName.Size);
            var magnetismRadiusStat = _stats.GetStat(StatName.MagnetismRadius);
            var healthResource = _resources.GetResource(ResourceName.Health);
            var experienceResource = _resources.GetResource(ResourceName.Experience);

            if (sizeStat is null) return;
            if (magnetismRadiusStat is null) return;
            if (healthResource is null) return;
            if (experienceResource is null) return;

            sizeStat.valueChangedEvent.AddListener(UpdateSize);
            magnetismRadiusStat.valueChangedEvent.AddListener(UpdateMagnetismRadius);

            _resources.GetResource(ResourceName.Health).AddListenerToEvent(ResourceEventType.Empty, Death);
            _resources.GetResource(ResourceName.Experience).onFill.AddListener(LevelUp);

            _isSubscribed = true;
        }

        private void Unsubscribe()
        {
            if (!_isSubscribed) return;

            _stats.GetStat(StatName.Size).valueChangedEvent.RemoveListener(UpdateSize);
            _stats.GetStat(StatName.MagnetismRadius).valueChangedEvent.RemoveListener(UpdateMagnetismRadius);
            _resources.GetResource(ResourceName.Health).RemoveListenerToEvent(ResourceEventType.Empty, Death);
            _resources.GetResource(ResourceName.Experience).RemoveListenerToEvent(ResourceEventType.Fill, LevelUp);

            _isSubscribed = false;
        }

        private void OnCollisionEnter2D(Collision2D collision2D)
        {
            var otherTag = collision2D.collider.tag;

            switch (otherTag)
            {
                case "Enemy":
                    var enemy = collision2D.gameObject.GetComponent<Enemy>();
                    CollideWithEnemy(enemy);
                    break;
                case "Enclosure":
                    Debug.LogWarning("ENCLOSURE");
                    CollideWithEnclosure(collision2D);
                    break;
            }
        }

        //private void CollideWithEnclosure(Player player)
        //{
        // shield, damage, knockback from pos with power
        //}

        private void CollideWithEnclosure(Collision2D collision2D)
        {
            if (!_invulnerability.IsInvulnerable)
            {
                if (!Shield.Layers.IsEmpty)
                    Shield.Layers.Decrease();
                else
                {
                    TakeDamage(1);
                }

                if (_resources.GetResource(ResourceName.Health).IsEmpty) return;
                _invulnerability.ApplyInvulnerable();
            }

            KnockBackFromEntity(50f, collision2D.transform.position);
        }

        private void CollideWithEnemy(Enemy enemy)
        {
            if (!Shield.Layers.IsEmpty)
                Shield.Layers.Decrease();
            else
            {
                var damageAmount = enemy.GetDamage();
                TakeDamage(damageAmount);

                if (_resources.GetResource(ResourceName.Health).IsEmpty) return;

                _invulnerability.ApplyInvulnerable();

                var knockbackPower = enemy.GetKnockbackPower();
                Vector2 enemyPosition = enemy.transform.position;

                KnockBackFromEntity(knockbackPower, enemyPosition);
            }
        }

        public Transform GetFirePoint() => _firePoint;

        public GameObject CreateWeapon(GameObject weapon)
        {
            var firePoint = GetFirePoint();
            var w = Instantiate(weapon);
            w.transform.SetParent(firePoint);
            w.transform.position = firePoint.transform.position;
            var firearm = w.GetComponent<Firearm>();
            firearm.SetSource(this);
            Firearm = firearm;
            return w;
        }

        public IWeapon GetWeapon() => Firearm;

        [ContextMenu(nameof(LevelUp))]
        private void LevelUp()
        {
            Debug.LogWarning("Level up");

            var statMod = new StatMod(OperationType.Addition, ExperienceAmountIncreasingPerLevel); // TODO add this value to stats as stat

            _stats.GetStat(StatName.ExperienceToNewLevel).AddModifier(statMod);
            _resources.GetResource(ResourceName.Level).Increase();
            _resources.GetResource(ResourceName.Experience).Empty();
            IsFireButtonPressed = false;
        }

        private void KnockBackFromEntity(float knockbackPower, Vector2 position)
        {
            var direction = ((Vector2)transform.position - position).normalized;
            var force = direction * knockbackPower;
            _knockbackController.Knockback(force);
        }

        private void Death()
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }

        private void TakeDamage(int amount)
        {
            _resources.GetResource(ResourceName.Health).Decrease(amount);
            Debug.Log("Damage is taken " + gameObject.name);
        }

        private void UpdateSize()
        {
            var sizeValue = _stats.GetStat(StatName.Size).Value;
            transform.localScale = new Vector3(sizeValue, sizeValue, 1);
        }

        private void UpdateMagnetismRadius()
        {
            _circleCollider.radius = Math.Max(_stats.GetStat(StatName.MagnetismRadius).Value, 0);
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

        private void FixedUpdate()
        {
            if (IsFireButtonPressed) Shoot();
        }

        public void Shoot()
        {
            Firearm.DoAction();
            const float animationFireSpeed = 0.5f;
            _animator.speed = animationFireSpeed;
            CancelInvoke(nameof(RestoreAnimationSpeed));
            Invoke(nameof(RestoreAnimationSpeed), 0.2f);
        }

        private void RestoreAnimationSpeed()
        {
            _animator.speed = 1f;
        }

        public void OnFireStart()
        {
            IsFireButtonPressed = true;
        }

        public void OnFireEnd()
        {
            IsFireButtonPressed = false;
        }

        private void TakeAim(Vector2 direction)
        {
            direction.Normalize();

            if (aimMode == AimMode.AutoAim)
            {
                if (CurrentAimDirection.Equals(Vector2.zero)) return;
                CurrentAimDirection = Vector2.zero;
                return;
            }

            CurrentAimDirection = direction;
        }

        public void OnMouseAiming(InputValue input)
        {
            Debug.LogWarning("OnMouseAiming");
            var mousePosition = Camera.main.ScreenToWorldPoint(input.Get<Vector2>());
            var directionToMousePos = mousePosition - gameObject.transform.position;
            TakeAim(directionToMousePos);
        }

        public void OnJoystickAimingFire(InputValue input)
        {
            Debug.LogWarning("OnJoystickAimingFire");
            TakeAim(input.Get<Vector2>());
            IsFireButtonPressed = !CurrentAimDirection.Equals(Vector2.zero);
        }

        public void OnChangeAimMode()
        {
            switch (aimMode)
            {
                case AimMode.AutoAim:
                    aimMode = AimMode.SelfAim;
                    break;
                case AimMode.SelfAim:
                {
                    aimMode = AimMode.AutoAim;
                    var t = Firearm.GetCurrentTarget();
                    if (t != null) t.RemoveFromTarget();
                    break;
                }
            }

            Debug.Log($"Aim mode changes to {aimMode}");
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

        public void IncreaseKills()
        {
            _resources.GetResource(ResourceName.Kills).Increase();
        }

        public void SetAimMode(AimMode mode)
        {
            aimMode = mode;
        }
    }
}