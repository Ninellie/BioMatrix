using Assets.Scripts.Core.Variables;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents
{
    public class PositionSetter : MonoBehaviour
    {
        [SerializeField] private Vector2Variable _myPosition;
        [SerializeField] private Transform _transform;
        private void Awake()
        {
            if (_transform == null) _transform = transform;
        }

        private void FixedUpdate()
        {
            if (_myPosition == null) return;
            _myPosition.SetValue(_transform.position);
        }
    }
}