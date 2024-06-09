using Core.Variables.References;
using UnityEngine;

namespace FirearmComponents
{
    public class DistanceConditionComponent : ConditionComponent
    {
        public Vector2Reference vectorA;
        public Vector2Reference vectorB;
        public float distance;

        public override bool IsMet()
        {
            return !(Vector2.Distance(vectorA.Value, vectorB.Value) < distance);
        }
    }
}