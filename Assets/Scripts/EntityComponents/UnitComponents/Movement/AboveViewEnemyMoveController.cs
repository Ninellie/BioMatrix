using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.Movement
{
    public class AboveViewEnemyMoveController : EnemyMoveController
    {
        private Vector2 ViewDirection => MyUnit.transform.up;
        //ROTATION
        private float RotationSpeed => MyUnit.RotationSpeed.Value;
        private float RotationStep => RotationSpeed * Time.fixedDeltaTime;
        public AboveViewEnemyMoveController(Enemy.Enemy myUnit, GameObject target) : base(myUnit, target)
        {
        }
        public override void FixedUpdateMoveStep()
        {
            TurnToTargetStep();
            var nextPosition = GetFixedUpdateMoveStep();
            MyUnit.Rb2D.MovePosition(nextPosition);
        }
        public override Vector2 GetFixedUpdateMoveStep()
        {
            Vector2 nextPosition = MyPosition;
            if (knockbackTime > 0)
            {
                nextPosition += KnockbackVelocity * Time.fixedDeltaTime;
                knockbackTime -= Time.fixedDeltaTime;
                if (knockbackTime <= 0)
                {
                    KnockbackDirection = Vector2.zero;
                }
            }
            Vector2 moveStep = ViewDirection * Speed * Time.fixedDeltaTime;
            nextPosition += moveStep;
            if (SpeedScale < 1)
            {
                SpeedScale += SpeedScaleStep;
            }
            return nextPosition;
        }
        public override void Stag()
        {
            SpeedScale = 0;
        }
        public override void KnockBackFromTarget(float thrustPower)
        {
            knockbackTime = thrustPower / knockbackSpeed;
            KnockbackDirection = (MyPosition - TargetPosition).normalized;
            Stag();
            Debug.Log("Knockback");
        }
        private void TurnToTargetStep()
        {
            var angle = GetDirectionAngle(MovementDirection);
            var speed = RotationStep;
            var lerpAngle = Mathf.LerpAngle(MyUnit.Rb2D.rotation, angle, speed);
            MyUnit.Rb2D.rotation = lerpAngle;
        }
        private float GetDirectionAngle(Vector2 direction)
        {
            var angleInDegrees = (Mathf.Atan2(direction.y, direction.x) - Mathf.PI / 2) * Mathf.Rad2Deg;
            return angleInDegrees;
        }

    }
}