using UnityEngine;

namespace Assets.Scripts.Entity.Unit.Movement
{
    public abstract class EnemyMoveController
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
        protected Enemy.Enemy MyUnit { get; }
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
                if (Target is null)
                {
                    return MyPosition;
                }
                return Target.transform.position;
            }
        }

        protected Vector2 MyPosition => MyUnit.transform.position;
        protected EnemyMoveController(Enemy.Enemy myUnit, GameObject target)
        {
            this.MyUnit = myUnit;
            this.Target = target;
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