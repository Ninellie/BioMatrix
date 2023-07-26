using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.Movement
{
    public abstract class OldEnemyMoveController
    {
        protected float knockbackSpeed = 700;
        protected float knockbackTime = 0;
        public Vector2 KnockbackDirection
        {
            get => _knockbackDirection;
            set => _knockbackDirection = value.normalized;
        }
        private Vector2 _knockbackDirection = Vector2.zero;
        protected GameObject Target { get;}
        protected EnemyComponents.Enemy MyUnit { get; }
        public Vector2 Velocity => MovementVelocity + KnockbackVelocity;
        protected Vector2 MovementVelocity => MovementDirection * Speed;
        protected Vector2 KnockbackVelocity => KnockbackDirection * knockbackSpeed;
        protected float Speed => MyUnit.Speed.Value * SpeedScale;
        protected float SpeedScale
        {
            get => _speedScale;
            set
            {
                _speedScale = value switch
                {
                    < 0 => 0,
                    > 1 => 1,
                    _ => value
                };
            }
        }
        private float _speedScale = 1.0f;
        private const float SpeedScaleRestoreSpeedPerSecond = 1f;
        protected float SpeedScaleStep => SpeedScaleRestoreSpeedPerSecond * Time.fixedDeltaTime;
        //DIRECTION
        protected Vector2 MovementDirection => (TargetPosition - MyPosition).normalized;
        protected Vector2 TargetPosition
        {
            get
            {
                if (Time.timeScale > 0)
                {
                    return Target.transform.position;
                }
                return MyPosition;
            }
        }

        protected Vector2 MyPosition => MyUnit.transform.position;
        protected OldEnemyMoveController(EnemyComponents.Enemy myUnit, GameObject target)
        {
            MyUnit = myUnit;
            Target = target;
        }
        public abstract void FixedUpdateMoveStep();
        public abstract Vector2 GetFixedUpdateMoveStep();
        public abstract void Stag();
        public abstract void KnockBackFromTarget(float thrustPower);
        public Vector2 GetMovementDirection()
        {
            return MovementDirection;
        }
        public Vector2 GetVelocity()
        {
            return Velocity;
        }
    }
}