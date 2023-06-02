using Assets.Scripts.GameSession.Spawner;
using UnityEngine;

namespace Assets.Scripts.GameSession.Tentative
{
    public class Rotation : MonoBehaviour
    {
        [SerializeField] private GameObject _attractor;
        [SerializeField] private float _orbitRadius;
        [SerializeField] private float _speedDegreesPerSecond;
        [SerializeField] private float _currentAngle;
        private Vector2 Center => _attractor.transform.position;
        private Rigidbody2D _rigidbody2D;
        private Circle _circle = new Circle();

        private float CurrentAngle
        {
            get => _currentAngle;
            set
            {
                if (value > 360f)
                {
                    _currentAngle = value - 360f;
                }
                else
                {
                    _currentAngle = value;
                }
            }
        }
        private void FixedUpdate()
        {
            CurrentAngle += _speedDegreesPerSecond * Time.fixedDeltaTime;
            float fi = _currentAngle * Mathf.Deg2Rad;
            Vector2 nextPosition = _circle.GetPointOn(_orbitRadius, Center, fi);
            _rigidbody2D.MovePosition(nextPosition);//set new position
        }
        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            CurrentAngle = 0f;//
        }
    }
}