using UnityEngine;

namespace FirearmComponents
{
    public abstract class ConditionComponent : MonoBehaviour
    {
        public abstract bool IsMet();
    }
}