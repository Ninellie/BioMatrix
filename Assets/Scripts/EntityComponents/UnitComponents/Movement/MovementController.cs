using Assets.Scripts.Core.Variables.References;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.Movement
{
    public abstract class MovementController : MonoBehaviour, IMovementController
    {
        [Header("Speed scale settings")]
        [SerializeField] [Min(0)] private float _baseSpeedScale;
        [SerializeField] [Min(0)] private float _minSpeedScale;
        [SerializeField] [Min(0)] private float _maxSpeedScale;
        [SerializeField] [Min(0)] private float _tendSpeedScale;
        [Tooltip("Time in seconds for which the SpeedScale value unit will be restored")]
        [SerializeField] private float _speedScaleRestoreSpeed;
        [SerializeField] private bool _restoreSpeedScale;
        [SerializeField] private bool _staticSpeedScale;

        protected float SpeedScale
        {
            get => _staticSpeedScale ? _baseSpeedScale : _speedScale;
            set => _speedScale = value;
        }

        private Vector2 AddedVelocity { get; set; } = Vector2.zero;
        [SerializeField] protected FloatReference speedStat;
        protected abstract float Speed { get; }
        protected abstract Vector2 MovementDirection { get; set; }
        protected abstract Vector2 RawMovementDirection { get; set; }
        private Vector2 MovementVelocity => MovementDirection * Speed;
        private Vector2 Velocity => MovementVelocity + AddedVelocity;
        private Vector2 MyPosition => transform.position;
        private Rigidbody2D _rigidbody2D;
        private float _speedScale;

        protected void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            SpeedScale = _baseSpeedScale;
        }

        protected void FixedUpdate()
        {
            var t = Time.fixedDeltaTime;
            if (_restoreSpeedScale) RestoreSpeedScale(t);
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

        public Vector2 GetRawMovementDirection() => RawMovementDirection;

        public float GetSpeedScale() => SpeedScale;

        public void SetSpeedScale(float value)
        {
            if (value > _maxSpeedScale)
            {
                SpeedScale = _maxSpeedScale;
                return;
            }

            if (value < _minSpeedScale)
            {
                SpeedScale = _minSpeedScale;
                return;
            }

            SpeedScale = value;
        }

        public bool IsStopped() => Speed <= 0f;

        public void AddVelocity(Vector2 velocity) => AddedVelocity += velocity;

        private Vector2 GetMoveStep(float time) => MyPosition + Velocity * time;

        private void RestoreSpeedScale(float time)
        {
            if (!(SpeedScale > _tendSpeedScale) && !(SpeedScale < _tendSpeedScale))
                return;

            var speedScaleRestoreStep = time / _speedScaleRestoreSpeed;

            if (SpeedScale > _tendSpeedScale)
                DecreaseSpeedScale(speedScaleRestoreStep);
            else if (SpeedScale < _tendSpeedScale) IncreaseSpeedScale(speedScaleRestoreStep);
        }

        private void IncreaseSpeedScale(float value)
        {
            var newValue = SpeedScale + value;
            SpeedScale = Mathf.Min(newValue, _tendSpeedScale);
        }

        private void DecreaseSpeedScale(float value)
        {
            var newValue = SpeedScale - value;
            SpeedScale = Mathf.Max(newValue, _tendSpeedScale);
        }
    }
}