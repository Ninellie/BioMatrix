using UnityEngine;

namespace Assets.Scripts.Core.Variables.Components
{
    public class PositionSetter : MonoBehaviour
    {
        [SerializeField] private Vector2Variable _variable;
        [SerializeField] private Transform _transform;

        private void Awake()
        {
            if (_transform == null) _transform = transform;
            if (_variable == null) return;
            _variable.SetValue(_transform.position);
        }

        private void FixedUpdate()
        {
            if (_variable == null) return;
            _variable.SetValue(_transform.position);
        }
    }
}