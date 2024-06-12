using System;
using Core.Variables.References;
using UnityEngine;

namespace EntityComponents.UnitComponents.PlayerComponents
{
    public class ColliderRadiusUpdater : MonoBehaviour
    {
        [SerializeField] private CircleCollider2D _circleCollider;
        [SerializeField] private FloatReference _radius;

        private void Awake()
        {
            if (_circleCollider == null) _circleCollider = GetComponent<CircleCollider2D>();
        }

        public void UpdateRadius()
        {
            _circleCollider.radius = Math.Max(_radius, 0);
        }
    }
}