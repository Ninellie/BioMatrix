using UnityEngine;

namespace Assets.Scripts.Core.Sets
{
    public class TransformThing : MonoBehaviour
    {
        public TransformRuntimeSet runtimeSet;
        public Transform myTransform;

        private void OnEnable()
        {
            if (myTransform == null) myTransform = transform;
            runtimeSet.Add(this);
        }

        private void OnDisable()
        {
            if (myTransform == null) myTransform = transform;
            runtimeSet.Remove(this);
        }
    }
}