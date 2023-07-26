using Assets.Scripts.EntityComponents.Stats;
using Assets.Scripts.EntityComponents.UnitComponents.Movement;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.ProjectileComponents
{
    [RequireComponent(typeof(UnitStatsSettings))]
    public class Projectile : Unit
    {
        //[SerializeField] public float timeToStop = 15f;
        public Entity Source { get; private set; }
        public UnitStatsSettings Settings => GetComponent<UnitStatsSettings>();

        private TrailRenderer _trail;
        private CircleCollider2D _circleCollider;
        //private OldMovementControllerBullet _oldMovementController;
        private IProjectileMovementController _movementController;

        private void Awake() => BaseAwake(Settings);

        private void OnEnable() => BaseOnEnable();

        private void OnDisable() => BaseOnDisable();

        private void Update() => BaseUpdate();

        private void FixedUpdate()
        {
            //_oldMovementController.FixedUpdateStep();
            //if (!_oldMovementController.IsStopped()) return;
            if (!_movementController.IsStopped()) return;
            Death();
        }

        private void OnCollisionEnter2D(Collision2D collision2D)
        {
            var otherCollider2D = collision2D.collider;
            if (!otherCollider2D.gameObject.CompareTag("Enemy")) return;
            if (!otherCollider2D.gameObject.GetComponent<Enemy.Enemy>().Alive) return;
            TakeDamage(MinimalDamageTaken);
        }

        protected override void BaseAwake(UnitStatsSettings settings)
        {
            base.BaseAwake(settings);
            _trail = GetComponent<TrailRenderer>();
            _circleCollider = GetComponent<CircleCollider2D>();
            _movementController = GetComponent<IProjectileMovementController>();
            //_oldMovementController = new OldMovementControllerBullet(this);
            //_oldMovementController.FixedUpdateStep();
        }

        protected override void BaseOnEnable()
        {
            base.BaseOnEnable();
            KillsCount.IncrementEvent += () => Source.KillsCount.Increase();
        }

        protected override void BaseOnDisable()
        {
            base.BaseOnDisable();
            KillsCount.IncrementEvent -= () => Source.KillsCount.Increase();
        }

        protected override void BaseUpdate()
        {
            base.BaseUpdate();
            if (IsOnScreen == false)
            {
                TakeDamage(LifePoints.GetValue());
            }
        }

        protected override void ChangeCurrentSize()
        {
            base.ChangeCurrentSize();
            _trail.startWidth = _circleCollider.radius * 2 * Size.Value;
        }

        public void Launch(Vector2 direction, float force)
        {
            _movementController.SetDirection(direction);
            //_oldMovementController.SetDirection(direction);
            var speedMod = new StatModifier(OperationType.Addition, force);
            Speed.AddModifier(speedMod);
        }

        public void SetSource(Entity source)
        {
            Source = source;
        }
    }
}