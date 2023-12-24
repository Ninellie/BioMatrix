using Assets.Scripts.Core.Variables.References;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.ProjectileComponents
{
    public class CircleColliderRadiusUpdater : MonoBehaviour
    {
        [SerializeField] private FloatReference _radius;
        [SerializeField] private CircleCollider2D _circleCollider;

        public void UpdateRadius()
        {
            _circleCollider.radius = Mathf.Max(_radius.Value, 0);
        }
    }
}