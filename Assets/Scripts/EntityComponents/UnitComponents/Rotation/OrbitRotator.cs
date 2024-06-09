using Assets.Scripts.GameSession.Spawner;
using Core.Sets;
using GameSession.Spawner;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.Turret
{
    public class OrbitRotator : MonoBehaviour
    {
        [SerializeField] private float _orbitRadius = 80;
        [SerializeField] [Tooltip("In degrees per second")] private float _orbitalSpeed;
        [SerializeField] private TransformRuntimeSet _pool;

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
            var gap = 360f / _pool.items.Count;
            CurrentAngle += _orbitalSpeed * time;
            foreach (var o in _pool.items)
            {
                var fi = CurrentAngle * Mathf.Deg2Rad;
                var nextPosition = _circle.GetPointOn(_orbitRadius, _attractionPoint.position, fi);
                o.Transform.position = nextPosition;
                CurrentAngle += gap;
            }
        }
    }
}