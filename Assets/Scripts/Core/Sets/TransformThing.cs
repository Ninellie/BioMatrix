using UnityEngine;

namespace Assets.Scripts.Core.Sets
{
    public class TransformThing : MonoBehaviour
    {
        public Transform Transform { get; private set; }

        public TransformRuntimeSet runtimeSet;

        private void OnEnable()
        {
            if (Transform == null) Transform = transform;
            runtimeSet.Add(this);
        }

        private void OnDisable()
        {
            if (Transform == null) Transform = transform;
            runtimeSet.Remove(this);
        }
    }
}