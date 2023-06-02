using Assets.Scripts.GameSession.Spawner;
using UnityEngine;

namespace Assets.Scripts.Entity.Unit.Movement
{
    public class MovementControllerTurret
    {
        private readonly Turret.Turret _myTurret;
        private float OrbitRadius => _myTurret.AccelerationSpeed.Value;
        private readonly Circle _circle = new();
        private float _currentAngle;
        private float CurrentAngle
        {
            get => _currentAngle;
            set
            {
                _currentAngle = value switch
                {
                    > 360f => value - 360f,
                    _ => value
                };
            }
        }
        private float OrbitalSpeed => _myTurret.Speed.Value; //in degrees per second
        private Vector2 Center => _myTurret.GetAttractor().transform.position;
        public MovementControllerTurret(Turret.Turret myTurret)
        {
            _myTurret = myTurret;
            var r = Random.Range(0f, 360f);
            _currentAngle = r;
        }
        public void OrbitalFixedUpdateStep()
        {
            CurrentAngle += OrbitalSpeed * Time.fixedDeltaTime;
            var fi = CurrentAngle * Mathf.Deg2Rad;
            var nextPosition = _circle.GetPointOn(OrbitRadius, Center, fi);
            _myTurret.Rb2D.MovePosition(nextPosition); //set new position
        }
    }
}