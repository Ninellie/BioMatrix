using Assets.Scripts.EntityComponents.Stats;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.Movement
{
    public abstract class MovementController : MonoBehaviour, IMovementController
    {
        [SerializeField]
        [Tooltip("In seconds")]
        private float _speedScaleRestoreSpeed;

        [SerializeField]
        [Range(0, 2)]
        private float _baseSpeedScale;

        [SerializeField]
        private bool _restoreSpeedScale;

        protected float SpeedScale { get; set; }
        private Vector2 AddedVelocity { get; set; } = Vector2.zero;
        protected Stat speedStat;
        protected virtual float Speed => speedStat.Value * SpeedScale;
        protected abstract Vector2 MovementDirection { get; set; }
        protected abstract Vector2 RawMovementDirection { get; set; }
        private Vector2 MovementVelocity => MovementDirection * Speed;
        private Vector2 Velocity => MovementVelocity + AddedVelocity;
        private Vector2 MyPosition => transform.position;
        private Rigidbody2D _rigidbody2D;

        protected void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            speedStat = GetComponent<Unit>().Speed;
            SpeedScale = _baseSpeedScale;
        }

        protected void Start()
        {
            speedStat = GetComponent<Unit>().Speed;
        }

        protected void FixedUpdate()
        {
            var t = Time.fixedDeltaTime;
            if (_restoreSpeedScale)
                RestoreSpeedScale(t);

            //UpdateKnockbackTime(t);

            var nextPosition = GetMoveStep(t);
            _rigidbody2D.MovePosition(nextPosition);
        }

        protected void OnDrawGizmos()
        {
            if (speedStat == null) return;
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, Velocity + (Vector2)transform.position);
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(transform.position, MovementVelocity + (Vector2)transform.position);
        }

        public Vector2 GetRawMovementDirection()
        {
            return RawMovementDirection;
        }

        public float GetSpeedScale()
        {
            return SpeedScale;
        }

        public void SetSpeedScale(float value)
        {
            switch (value)
            {
                case > 1:
                    SpeedScale = Mathf.Max(value, 2);
                    return;
                case < 1:
                    SpeedScale = Mathf.Min(value, 1);
                    return;
                default:
                    SpeedScale = 1;
                    break;
            }
        }

        public bool IsStopped()
        {
            return Speed <= 0f;
        }

        public void AddVelocity(Vector2 velocity)
        {
            AddedVelocity += velocity;
        }

        private Vector2 GetMoveStep(float time)
        {
            return MyPosition + Velocity * time;
        }

        private void RestoreSpeedScale(float time)
        {
            //if (!(SpeedScale > 0) && !(SpeedScale < 0))
            //    return;

            var speedScaleRestoreStep = _speedScaleRestoreSpeed * time;

            switch (SpeedScale)
            {
                case > 1:
                    DecreaseSpeedScale(speedScaleRestoreStep);
                    break;
                case < 1:
                    IncreaseSpeedScale(speedScaleRestoreStep);
                    break;
            }
        }

        private void IncreaseSpeedScale(float value)
        {
            var newValue = SpeedScale + value;
            SpeedScale = Mathf.Max(newValue, 1);
        }

        private void DecreaseSpeedScale(float value)
        {
            var newValue = SpeedScale + value;
            SpeedScale = Mathf.Min(newValue, 1);
        }
    }
}