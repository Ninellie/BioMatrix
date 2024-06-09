using Assets.Scripts.EntityComponents.UnitComponents.EnemyComponents;
using Assets.Scripts.GameSession.UIScripts;
using Core.Sets;
using Core.Variables.References;
using UnityEngine;

namespace Assets.Scripts.FirearmComponents
{
    public class Aim : MonoBehaviour
    {
        [SerializeField] private PlayerTargetRuntimeSet _targets; // TODO заменить на что-то нейтральное
        [SerializeField] private bool _displayTarget;
        [SerializeField] private AimMode _mode;
        [SerializeField] private Vector2Reference _selfAimDirection;
        [Space]
        [Header("Stats")]
        [SerializeField] private FloatReference _radius;
        [SerializeField] private bool _inCamBounds;

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

        private PlayerTarget _target;

        private void FixedUpdate()
        {
            UpdateTarget();
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawRay(Transform.position, _direction);
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(_transform.position, Vector2.up * _radius);
            Gizmos.DrawRay(_transform.position, Vector2.left * _radius);
            Gizmos.DrawRay(_transform.position, Vector2.right * _radius);
            Gizmos.DrawRay(_transform.position, Vector2.down * _radius);
        }

        public Vector2 GetDirection()
        {
            if (_mode == AimMode.SelfAim) return _selfAimDirection.Value;
            if (_target == null)
            {
                _direction = Random.onUnitSphere;
                return _direction.normalized;
            }
            _direction = _target!.Transform.position - Transform.position;
            return _direction.normalized;
        }

        private void UpdateTarget()
        {
            if (_mode == AimMode.SelfAim)
            {
                if (_target == null) return;
                
                if (_displayTarget) _target.RemoveFromTarget();
                _target = null;
                return;
            }

            var nearestTarget = _inCamBounds ?
                _targets.GetNearestToPosition(Transform.position) :
                _targets.GetNearestToCenterInCircle(Transform.position, _radius);

            if (nearestTarget == null)
            {
                _target = null;
                return;
            }
            if (_target != null)
            {
                if (_target == nearestTarget) return;
                if (_displayTarget) _target.RemoveFromTarget();
            }
            _target = nearestTarget;
            if (_displayTarget) _target.TakeAsTarget();
        }


        public void ToggleMode()
        {
            _mode = _mode == AimMode.AutoAim ? AimMode.SelfAim : AimMode.AutoAim;
        }

        public void SetAutoMode(bool autoAim)
        {
            _mode = autoAim ? AimMode.AutoAim : AimMode.SelfAim;
        }
    }
}