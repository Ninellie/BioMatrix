using System;
using Assets.Scripts.EntityComponents.Resources;
using Assets.Scripts.EntityComponents.Stats;
using Assets.Scripts.EntityComponents.UnitComponents.BoonComponents;
using Assets.Scripts.EntityComponents.UnitComponents.EnemyComponents;
using Assets.Scripts.EntityComponents.UnitComponents.Knockback;
using Assets.Scripts.EntityComponents.UnitComponents.ProjectileComponents;
using Assets.Scripts.EntityComponents.UnitComponents.Turret;
using Assets.Scripts.FirearmComponents;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents
{
    public interface ISlayer
    {
        public void IncreaseKills();
    }
    public interface IWeaponBearer
    {
        public Transform GetFirePoint();
        public GameObject CreateWeapon(GameObject weapon);
        public IWeapon GetWeapon();

    }

    public interface IPlayableCharacter
    {

    }

    public class Player : MonoBehaviour, IWeaponBearer, IPlayableCharacter, ISlayer, ISource
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
        private bool _isSubscribed;

        private void Awake()
        {
            Debug.Log($"{gameObject.name} {nameof(Player)} Awake");

            _stats = GetComponent<StatList>();
            _resources = GetComponent<ResourceList>();

            _animator = GetComponent<Animator>();
            _circleCollider = GetComponent<CircleCollider2D>();
            _invulnerability = GetComponent<Invulnerability>();
            _knockbackController = GetComponent<KnockbackController>();
        }

        private void Start()
        {
            _circleCollider.radius = Math.Max(_stats.GetStat(StatName.MagnetismRadius).Value, 0);
            Subscribe();
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

            sizeStat.valueChangedEvent.AddListener(ChangeCurrentSize);
            magnetismRadiusStat.valueChangedEvent.AddListener(ChangeCurrentMagnetismRadius);

            _resources.GetResource(ResourceName.Health).AddListenerToEvent(ResourceEventType.Empty, Death);
            //_resources.GetResource(ResourceName.Experience).AddListenerToEvent(ResourceEventType.Fill, LevelUp);
            _resources.GetResource(ResourceName.Experience).onFill.AddListener(LevelUp);

            _isSubscribed = true;
        }

        private void Unsubscribe()
        {
            if (!_isSubscribed) return;

            _stats.GetStat(StatName.Size).valueChangedEvent.RemoveListener(ChangeCurrentSize);
            _stats.GetStat(StatName.MagnetismRadius).valueChangedEvent.RemoveListener(ChangeCurrentMagnetismRadius);
            _resources.GetResource(ResourceName.Health).RemoveListenerToEvent(ResourceEventType.Empty, Death);
            _resources.GetResource(ResourceName.Experience).RemoveListenerToEvent(ResourceEventType.Fill, LevelUp);

            _isSubscribed = false;
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

        //private void CollideWithEnclosure(Player player)
        //{
        // shield, damage, knockback from pos with power
        //}

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
                    
                    //CollideWithEnclosure(projectile);
                    break;
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

        //private void IncreaseExperience(int value)
        //{
        //    var expMultiplierValue = (int)_stats.GetStat(StatName.ExperienceMultiplier).Value;
        //    var expTakenAmount = value * expMultiplierValue;
        //    _resources.GetResource(ResourceName.Experience).Increase(expTakenAmount);
        //}

        private void LevelUp()
        {
            Debug.LogWarning("Level up");

            var statMod = new StatMod(OperationType.Addition, ExperienceAmountIncreasingPerLevel); // TODO add this value to stats as stat

            _stats.GetStat(StatName.ExperienceToNewLevel).AddModifier(statMod);
            _resources.GetResource(ResourceName.Level).Increase();
            _resources.GetResource(ResourceName.Experience).Empty();
            
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
            Debug.Log("Damage is taken " + gameObject.name); // ECS?
        }

        private void ChangeCurrentSize()
        {
            var sizeValue = _stats.GetStat(StatName.Size).Value;
            transform.localScale = new Vector3(sizeValue, sizeValue, 1);
        }

        private void ChangeCurrentMagnetismRadius()
        {
            var magnetismRadiusValue = _stats.GetStat(StatName.MagnetismRadius).Value;
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

        public void IncreaseKills()
        {
            _resources.GetResource(ResourceName.Kills).Increase();
        }
    }
}