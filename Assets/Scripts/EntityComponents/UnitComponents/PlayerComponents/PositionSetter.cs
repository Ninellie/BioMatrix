using Assets.Scripts.Core.Variables;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents
{
    public class GameObjectSetter : MonoBehaviour
    {
        [SerializeField] private GameObjectVariable _variable;

        private void Awake()
        {
            _variable.SetValue(gameObject);
        }
    }

    public class PositionSetter : MonoBehaviour
    {
        [SerializeField] private Vector2Variable _variable;
        [SerializeField] private Transform _transform;
        private void Awake()
        {
            if (_transform == null) _transform = transform;
        }

        private void FixedUpdate()
        {
            if (_variable == null) return;
            _variable.SetValue(_transform.position);
        }
    }
}