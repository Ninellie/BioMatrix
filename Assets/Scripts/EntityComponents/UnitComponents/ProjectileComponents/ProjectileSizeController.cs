using Core.Variables.References;
using UnityEngine;

namespace EntityComponents.UnitComponents.ProjectileComponents
{
    public class ProjectileSizeController : MonoBehaviour
    {
        [SerializeField] private FloatReference _size;
        [SerializeField] private TrailRenderer _trail;
        [SerializeField] private CircleCollider2D _circleCollider;
        [SerializeField] private Transform _transform;

        private void Awake()
        {
            if (_trail == null) _trail = GetComponent<TrailRenderer>();
            if(_circleCollider == null) _circleCollider = GetComponent<CircleCollider2D>();
            if (_transform == null) _transform = transform;
        }

        private void OnEnable()
        {
            _transform.localScale = new Vector3(_size, _size, 1);
            _trail.startWidth = _circleCollider.radius * 2 * _size;
        }
    }
}