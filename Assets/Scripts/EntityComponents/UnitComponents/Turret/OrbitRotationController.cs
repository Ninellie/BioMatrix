using System.Collections.Generic;
using Assets.Scripts.GameSession.Spawner;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.TurretComponents
{
    public class OrbitRotationController : MonoBehaviour, IOrbitRotationController
    {
        [SerializeField]
        private float _orbitRadius = 80;

        [SerializeField] 
        [Tooltip("In degrees per second")]
        private float _orbitalSpeed;

        private Stack<Turret> _objects;

        private float OrbitRadius => _orbitRadius;
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

        private GameObject _attractor;
        private Vector2 Center => _attractor.transform.position;

        private void Awake() => _currentAngle = Random.Range(0f, 360f);

        private void FixedUpdate() => OrbitalStep(Time.fixedDeltaTime);

        public void SetObjects(Stack<Turret> objects)
        {
            _objects = objects;
        }

        public void SetAttractor(GameObject attractor)
        {
            _attractor = attractor;
        }

        private void OrbitalStep(float time)
        {
            var gap = 360f / _objects.Count;

            CurrentAngle += _orbitalSpeed * time;

            foreach (var o in _objects)
            {
                var fi = CurrentAngle * Mathf.Deg2Rad;
                var nextPosition = _circle.GetPointOn(OrbitRadius, Center, fi);
                o.transform.position = nextPosition;

                CurrentAngle += gap;
            }
        }
    }
}