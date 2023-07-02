using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.Movement
{
    public class MovementControllerBullet
    {
        private readonly Projectile.Projectile _myUnit;
        //private Vector2 Velocity => Direction * Speed * SpeedScale;
        private float Speed => _myUnit.Speed.Value;
        private Vector2 MyPosition => _myUnit.transform.position;
        private float SpeedScale
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
        private float _speedScale = 1f;
        private float SpeedDecreaseStep => SpeedDecreasePerSecond * Time.fixedDeltaTime;
        private float SpeedDecreasePerSecond => 1 / TimeToStop;
        private float TimeToStop => _myUnit.timeToStop;
        private Vector2 Direction
        {
            get => _direction;
            set => _direction = value.normalized;
        }
        private Vector2 _direction;
        public MovementControllerBullet(Projectile.Projectile myUnit)
        {
            _myUnit = myUnit;
        }
        public void FixedUpdateStep()
        {
            Vector2 nextPosition = MyPosition;

            Vector2 movementStep = Direction * Speed * SpeedScale * Time.fixedDeltaTime;

            nextPosition += movementStep;

            _myUnit.Rb2D.MovePosition(nextPosition);

            SpeedScale -= SpeedDecreaseStep;
        }
        public void SetDirection(Vector2 direction)
        {
            Direction = direction;
        }

        public bool IsStopped()
        {
            return SpeedScale <= 0f;
        }
    }
}