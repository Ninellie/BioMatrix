using Assets.Scripts.Core.Sets;
using Assets.Scripts.Core.Variables.References;
using Assets.Scripts.GameSession.UIScripts;
using UnityEngine;

namespace Assets.Scripts.FirearmComponents
{
    public class Aim : MonoBehaviour
    {
        [SerializeField] private PlayerTargetRuntimeSet _visibleEnemies;
        [SerializeField] private AimMode _mode;
        [SerializeField] private Vector2Reference _selfAimDirection;
        [Space]
        [Header("Stats")]
        [SerializeField] private FloatReference _radius;

        private Vector2 _direction;

        private Transform _transform;
        private Transform Transform
        {
            get
            {
                if (_transform != null) return _transform;
                _transform = transform;
                return _transform;
            }
        }
        private void OnDrawGizmos()
        {
            Gizmos.DrawRay(Transform.position, _direction);
        }

        public Vector2 GetDirection()
        {
            if (_mode == AimMode.SelfAim) return _selfAimDirection.Value;
            var target = _visibleEnemies.GetNearestToCenterInCircle(Transform.position, _radius);
            if (target == null) _direction = Random.onUnitSphere;
            _direction = target!.Transform.position - Transform.position;
            return _direction.normalized;
        }

        public void ToggleMode()
        {
            if (_mode == AimMode.AutoAim)
            {
                _mode = AimMode.SelfAim;
            }
            else
            {
                _mode = AimMode.AutoAim;
            }
        }
    }
}