using Assets.Scripts.Core.Variables.References;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.EntityComponents.Collisions
{
    public class CollisionEventHandler : MonoBehaviour
    {
        [Header("Inner components")]
        [SerializeField] private Transform _transform;
        [Header("Collision object")]
        [SerializeField] private string _tag;
        [SerializeField] private FloatReference _distanceLength;
        [SerializeField] private bool _useSeparatePosition;
        [SerializeField] private Vector2Reference _position;
        [Header("Response")]
        [Tooltip("Direction to collision object")]
        [SerializeField] private bool _inverseDirection;
        [SerializeField] private UnityEvent<Vector2> _onCollisionEnter2D;
        [SerializeField] private UnityEvent<Vector3> _onCollisionStay2D;

        private void Awake()
        {
            if (_transform == null) _transform = transform;
        }

        private void OnCollisionEnter2D(Collision2D collision2D)
        {
            if (collision2D.collider.tag != _tag) return;
            var distance = GetDistance(collision2D);
            _onCollisionEnter2D.Invoke(distance);
        }

        private void OnCollisionStay2D(Collision2D collision2D)
        {
            if (collision2D.collider.tag != _tag) return;
            var distance = GetDistance(collision2D);
            _onCollisionStay2D.Invoke(distance);
        }

        private Vector3 GetDistance(Collision2D collision2D)
        {
            Vector3 collisionObjectPosition = _useSeparatePosition ?
                _position.Value : collision2D.gameObject.transform.position;
            var direction = (_transform.position - collisionObjectPosition).normalized;
            if (_inverseDirection) direction = -direction;
            var force = direction * _distanceLength;
            return force;
        }
    }
}