using System.Linq;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.Repulse
{
    public class Repulse : MonoBehaviour
    {
        [SerializeField] private float _repulseForce;
        [SerializeField] private float _penetrationDistanceMultiplier;
        [SerializeField] private bool _considerPenetrationDepth;
        [SerializeField] private string _layerName;

        private CircleCollider2D _circleCollider;
        private Collider2D _collider;
        private Transform _transform;

        private void Start()
        {
            _collider = GetComponent<Collider2D>();
            _transform = transform;

            if (TryGetComponent<CircleCollider2D>(out _circleCollider))
            {
                _circleCollider = GetComponent<CircleCollider2D>();
            }
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (!enabled) return;
            var otherCollider2D = collision.collider;
            if (otherCollider2D.gameObject.layer != LayerMask.NameToLayer(_layerName)) return;
            if (_considerPenetrationDepth)
            {
                RepulseFrom(otherCollider2D, collision);
                return;
            }
            SimpleRepulse(collision);
        }

        private void RepulseFrom(Collider2D otherCollider2D, Collision2D collision2D)
        {
            switch (otherCollider2D)
            {
                case CircleCollider2D circleCollider2D:
                    CircleCollider2DRepulseFrom(circleCollider2D);
                    break;
                default:
                    Collider2DRepulseFrom(otherCollider2D, collision2D);
                    break;
            }
        }

        private void CircleCollider2DRepulseFrom(CircleCollider2D otherCollider2D)
        {
            var otherTransform = otherCollider2D.transform;

            var distance = Vector2.Distance(_transform.position, otherTransform.position);
            var radiiSum = _circleCollider.radius + otherCollider2D.radius;
            var multipliedRadiiSum = radiiSum * _penetrationDistanceMultiplier;
            var penetrationDepth = multipliedRadiiSum - distance;
            penetrationDepth = Mathf.Clamp(penetrationDepth, 0f, Mathf.Infinity);
            var repulseForce = _repulseForce * penetrationDepth;

            var repulseVector = (_transform.position - otherTransform.position).normalized;
            repulseVector *= repulseForce;

            _transform.Translate(repulseVector * Time.fixedDeltaTime, Space.World);
        }

        private void Collider2DRepulseFrom(Collider2D otherCollider2D, Collision2D collision)
        {
            var penetrationDepth = collision.contacts.Aggregate(0f, (current, contact) => GetPenetrationDepth(otherCollider2D, contact, current));
            var repulseForce = _repulseForce * penetrationDepth;
            var repulseVector = collision.contacts[0].normal * repulseForce;
            transform.Translate(repulseVector * Time.fixedDeltaTime, Space.World);
        }

        private float GetPenetrationDepth(Collider2D otherCollider2D, ContactPoint2D contact, float penetrationDepth)
        {
            var distance = Vector2.Distance(transform.position, contact.point);
            var radiiSum = _collider.bounds.extents.magnitude + otherCollider2D.bounds.extents.magnitude;
            var multipliedRadiiSum = radiiSum * _penetrationDistanceMultiplier;
            var f = multipliedRadiiSum - distance;
            penetrationDepth = Mathf.Max(penetrationDepth, f);
            return penetrationDepth;
        }

        private void SimpleRepulse(Collision2D collision)
        {
            var repulseVector = collision.contacts[0].normal * _repulseForce;
            transform.Translate(repulseVector * Time.fixedDeltaTime, Space.World);
        }
    }
}
