using Assets.Scripts.FirearmComponents;
using UnityEngine;

namespace Assets.Scripts.Core.Sets
{
    public class ShooterThing : MonoBehaviour
    {
        public Shooter Shooter { get; private set; }
        public ShooterRuntimeSet runtimeSet;

        private void OnEnable()
        {
            if (Shooter == null) Shooter = GetComponent<Shooter>();
            runtimeSet.Add(Shooter);
        }

        private void OnDisable()
        {
            if (Shooter == null) Shooter = GetComponent<Shooter>();
            runtimeSet.Remove(Shooter);
        }
    }
}