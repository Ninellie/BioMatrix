using Core.Sets;
using Core.Variables.References;
using GameSession.Spawner;
using UnityEngine;

namespace EntityComponents.UnitComponents.Rotation
{
    public class OrbitRotator : MonoBehaviour
    {
        [SerializeField] private FloatReference orbitRadius;
        [SerializeField] [Tooltip("In degrees per second")] private FloatReference orbitalSpeed;
        [SerializeField] private TransformRuntimeSet pool;

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

        private Transform _attractionPoint;

        private void Awake()
        {
            _attractionPoint = transform;
            _currentAngle = Random.Range(0f, 360f);
        }

        private void FixedUpdate() => OrbitalStep(Time.fixedDeltaTime);

        private void OrbitalStep(float time)
        {
            var gap = 360f / pool.items.Count;
            CurrentAngle += orbitalSpeed * time;
            foreach (var o in pool.items)
            {
                var fi = CurrentAngle * Mathf.Deg2Rad;
                var nextPosition = _circle.GetPointOn(orbitRadius, _attractionPoint.position, fi);
                o.Transform.position = nextPosition;
                CurrentAngle += gap;
            }
        }
    }
}