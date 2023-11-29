using Assets.Scripts.Core.Variables.References;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents
{
    public class CollisionEventHandler : MonoBehaviour
    {
        [Header("Inner components")]
        [SerializeField] private Transform _transform;
        [Header("Settings")]
        [SerializeField] private string _otherTag;
        [SerializeField] private FloatReference _otherKnockbackPower;
        [Header("Response")]
        [SerializeField] private UnityEvent<Vector2> _onCollisionEnter2D;

        private void Awake()
        {
            if (_transform == null) _transform = transform;
        }

        private void OnCollisionEnter2D(Collision2D collision2D)
        {
            if (collision2D.collider.tag != _otherTag) return;
            var direction = (_transform.position - collision2D.gameObject.transform.position).normalized;
            var force = direction * _otherKnockbackPower;
            _onCollisionEnter2D.Invoke(force);
        }
    }
}