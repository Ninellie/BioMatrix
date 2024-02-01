using Assets.Scripts.Core.Variables.References;
using UnityEngine;

namespace Assets.Scripts.FirearmComponents
{
    public abstract class ConditionComponent : MonoBehaviour
    {
        public abstract bool IsMet();
    }

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