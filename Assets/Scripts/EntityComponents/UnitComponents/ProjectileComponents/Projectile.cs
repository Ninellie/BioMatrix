using Assets.Scripts.EntityComponents.Resources;
using Assets.Scripts.EntityComponents.Stats;
using Assets.Scripts.EntityComponents.UnitComponents.EnemyComponents;
using Assets.Scripts.EntityComponents.UnitComponents.Movement;
using Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.ProjectileComponents
{
    public interface ISource
    {

    }

    public interface IDerivative
    {
        public void SetSource(ISource source);
    }

    public interface IKnockbackDealer
    {
        float GetKnockbackPower();
    }

    [RequireComponent(typeof(UnitStatsSettings))]
    public class Projectile : MonoBehaviour, ISlayer, IDerivative, IDamageDealer, IDamageable, IKnockbackDealer
    {
        private TrailRenderer _trail;
        private CircleCollider2D _circleCollider;
        private IProjectileMovementController _movementController;

        private ISlayer _source;
        private StatList _stats;
        private ResourceList _resources;

        private bool _isSubscribed;

        private void Awake()
        {
            Debug.Log($"{gameObject.name} Projectile Awake");

            _stats = GetComponent<StatList>();
            _resources = GetComponent<ResourceList>();
            _trail = GetComponent<TrailRenderer>();
            _circleCollider = GetComponent<CircleCollider2D>();
            _movementController = GetComponent<IProjectileMovementController>();

            ChangeCurrentSize();
        }

        private void Start() => Subscribe();

        private void OnEnable() => Subscribe();

        private void OnDisable() => Unsubscribe();

        private void FixedUpdate()
        {
            if (_movementController.IsStopped())
                Death();
        }

        private void OnCollisionEnter2D(Collision2D collision2D)
        {
            var otherCollider2D = collision2D.collider;
            if (!otherCollider2D.gameObject.CompareTag("Enemy")) return;
            if (!otherCollider2D.gameObject.GetComponent<Enemy>().IsAlive) return;
            _resources.GetResource(ResourceName.Health).Decrease();
        }

        private void OnBecameInvisible() => Death();

        public int GetDamage()
        {
            return (int)_stats.GetStat(StatName.Damage).Value;
        }

        public void TakeDamage(int damageAmount)
        {
            _resources.GetResource(ResourceName.Health).Decrease(damageAmount);
        }

        public void Launch(Vector2 direction, float force)
        {
            _movementController.SetDirection(direction);
            var speedMod = new StatMod(OperationType.Addition, force);
            _stats.GetStat(StatName.MovementSpeed).AddModifier(speedMod);
        }

        public void IncreaseKills()
        {
            _resources.GetResource(ResourceName.Kills).Increase();
            _source.IncreaseKills();
        }

        public void SetSource(ISource source)
        {
            if (source is ISlayer slayer)
                _source = slayer;
        }

        public float GetKnockbackPower() => _stats.GetStat(StatName.KnockbackPower).Value;

        private void ChangeCurrentSize()
        {
            var sizeValue = _stats.GetStat(StatName.Size).Value;
            transform.localScale = new Vector3(sizeValue, sizeValue, 1);
            _trail.startWidth = _circleCollider.radius * 2 * sizeValue;
        }

        private void Subscribe()
        {
            if (_isSubscribed) return;

            var sizeStat = _stats.GetStat(StatName.Size);
            var healthResource = _resources.GetResource(ResourceName.Health);

            if (sizeStat is null) return;
            if (healthResource is null) return;

            sizeStat.valueChangedEvent.AddListener(ChangeCurrentSize);
            healthResource.AddListenerToEvent(ResourceEventType.Empty, Death);

            _isSubscribed = true;
        }

        private void Unsubscribe()
        {
            if (!_isSubscribed) return;
            var sizeStat = _stats.GetStat(StatName.Size);
            var healthResource = _resources.GetResource(ResourceName.Health);

            if (sizeStat is null) return;
            if (healthResource is null) return;

            sizeStat.valueChangedEvent.RemoveListener(ChangeCurrentSize);
            healthResource.RemoveListenerToEvent(ResourceEventType.Empty, Death);

            _isSubscribed = false;
        }

        private void Death()
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}