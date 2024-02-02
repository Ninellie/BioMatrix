using UnityEngine;

namespace Assets.Scripts.FirearmComponents
{
    public abstract class ConditionComponent : MonoBehaviour
    {
        public abstract bool IsMet();
    }
}