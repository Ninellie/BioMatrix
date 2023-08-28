using Assets.Scripts.EntityComponents.Resources;
using Assets.Scripts.EntityComponents.Stats;
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

    [RequireComponent(typeof(UnitStatsSettings))]
    public class Projectile : MonoBehaviour, ISlayer, IDerivative
    {
        private TrailRenderer _trail;
        private CircleCollider2D _circleCollider;
        private IProjectileMovementController _movementController;

        private ISlayer _source;
        private StatList _stats;
        private ResourceList _resources;

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

        private void OnEnable()
        {
            _stats.GetStat(StatName.Size).valueChangedEvent.AddListener(ChangeCurrentSize);
            _resources.GetResource(ResourceName.Health).GetEvent(ResourceEventType.Empty).AddListener(Death);
        }

        private void OnDisable()
        {
            _stats.GetStat(StatName.Size).valueChangedEvent.RemoveListener(ChangeCurrentSize);
            _resources.GetResource(ResourceName.Health).GetEvent(ResourceEventType.Empty).RemoveListener(Death);
        }

        private void FixedUpdate()
        {
            if (_movementController.IsStopped())
                Death();
        }

        private void OnCollisionEnter2D(Collision2D collision2D)
        {
            var otherCollider2D = collision2D.collider;
            if (!otherCollider2D.gameObject.CompareTag("Enemy")) return;
            if (!otherCollider2D.gameObject.GetComponent<EnemyComponents.Enemy>().Alive) return;
            _resources.GetResource(ResourceName.Health).Decrease();
        }

        private void OnBecameInvisible() => Death();

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

        private void ChangeCurrentSize()
        {
            var sizeValue = _stats.GetStat(StatName.Size).Value;
            transform.localScale = new Vector3(sizeValue, sizeValue, 1);
            _trail.startWidth = _circleCollider.radius * 2 * sizeValue;
        }

        private void Death()
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}