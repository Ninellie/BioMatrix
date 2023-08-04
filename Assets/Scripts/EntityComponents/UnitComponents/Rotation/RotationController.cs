using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.Movement
{
    public class RotationController : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Radians per second")]
        private float _rotationSpeed;

        private float RotationSpeed => _rotationSpeed * _movementController.GetSpeedScale();
        private IMovementController _movementController;
        private Rigidbody2D _rigidbody2D;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _movementController = GetComponent<IMovementController>();
        }

        private void Start()
        {
            TurnToMovementDirection();
        }

        private void FixedUpdate()
        {
            var time = Time.fixedDeltaTime;
            var nextRotationAngle = GetNextRotationAngle(time);
            _rigidbody2D.SetRotation(nextRotationAngle);
        }

        private void TurnToMovementDirection()
        {
            var movementDirection = _movementController.GetRawMovementDirection();
            var rotation = GetDirectionAngle(movementDirection);
            _rigidbody2D.SetRotation(rotation);
        }

        private float GetNextRotationAngle(float time)
        {
            var movementDirection = _movementController.GetRawMovementDirection();
            var angleToDirection = GetDirectionAngle(movementDirection);
            var degreesPerTime = RotationSpeed * time;
            var lerpAngle = Mathf.LerpAngle(_rigidbody2D.rotation, angleToDirection, degreesPerTime);
            return lerpAngle;
        }

        private float GetDirectionAngle(Vector2 direction)
        {
            var angleInDegrees = (Mathf.Atan2(direction.y, direction.x) - Mathf.PI / 2) * Mathf.Rad2Deg;
            return angleInDegrees;
        }
    }
}