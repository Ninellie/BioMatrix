using Core.Variables.References;
using UnityEngine;

namespace Assets.Scripts.FirearmComponents
{
    public class SelfDistanceConditionComponent : ConditionComponent
    {
        public Vector2Reference otherPoint;
        public float distance;

        private Transform _transform;

        private void Awake()
        {
            _transform = transform;
        }

        public override bool IsMet()
        {
            return !(Vector2.Distance(otherPoint.Value, _transform.position) < distance);
        }
    }
}