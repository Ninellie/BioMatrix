using Assets.Scripts.EntityComponents.Stats;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.Movement
{

    public class KnockbackController : MonoBehaviour, IKnockbackController
    {
        [SerializeField]
        [Tooltip("In points per seconds")]
        private float _knockbackSpeed;
        public void Knockback(Vector2 force)
        {
            throw new System.NotImplementedException();
        }

        public void Knockback(GameObject target)
        {
            throw new System.NotImplementedException();
        }
    }

    public abstract class MovementController : MonoBehaviour, IKnockbackController, IMovementController
    {
        //[SerializeField]
        //[Tooltip("In points per seconds")]
        //private float _knockbackSpeed;

        [SerializeField]
        [Tooltip("In seconds")]
        private float _speedScaleRestoreSpeed;

        [SerializeField]
        [Range(0, 2)]
        private float _baseSpeedScale;

        [SerializeField]
        private bool _restoreSpeedScale;

        protected float SpeedScale { get; set; }

        private float _knockbackTime;
        private Vector2 KnockbackDirection { get; set; }

        private Vector2 AddedVelocity { get; set; } = Vector2.zero;
        private Vector2 KnockbackVelocity => KnockbackDirection * _knockbackSpeed;
        protected Stat speedStat;
        protected virtual float Speed => speedStat.Value * SpeedScale;
        protected abstract Vector2 MovementDirection { get; set; }
        protected abstract Vector2 RawMovementDirection { get; set; }
        private Vector2 MovementVelocity => MovementDirection * Speed;
        private Vector2 Velocity => MovementVelocity + KnockbackVelocity + AddedVelocity;
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

            UpdateKnockbackTime(t);

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

        public Vector2 GetMovementDirection()
        {
            return MovementDirection;
        }

        public Vector2 GetRawMovementDirection()
        {
            return RawMovementDirection;
        }

        public float GetSpeedScale()
        {
            return SpeedScale;
        }

        public void Knockback(Vector2 force)
        {
            KnockbackDirection = force.normalized;
            _knockbackTime = force.magnitude / _knockbackSpeed;
            SpeedScale = 0;
        }

        public void Knockback(GameObject target)
        {
            var force = MyPosition - (Vector2)target.transform.position;
            force.Normalize();
            force *= target.GetComponent<Entity>().KnockbackPower.Value;
            Knockback(force);
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

        private void UpdateKnockbackTime(float time)
        {
            if (!(_knockbackTime > 0)) return;
            _knockbackTime -= time;
            if (_knockbackTime > 0) return;
            KnockbackDirection = Vector2.zero;
        }
    }
}