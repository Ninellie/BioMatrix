using System;
using Core.Sets;
using UnityEngine;

namespace EntityComponents.UnitComponents.PlayerComponents
{
    public class RigidbodySimulationSwitcher : MonoBehaviour
    {
        [SerializeField] private ShooterRuntimeSet runtimeSet;
        [SerializeField] private bool simulationEnabled;
        
        private void OnEnable()
        {
            simulationEnabled = true;
            UpdateSimulationState();
        }

        private void OnDisable()
        {
            simulationEnabled = false;
            UpdateSimulationState();
        }

        private void OnValidate()
        {
            UpdateSimulationState();
        }

        public void UpdateSimulationState()
        {
            foreach (var item in runtimeSet.items)
            {
                if (item.TryGetComponent<Rigidbody2D>(out var rb2D))
                {
                    rb2D.simulated = simulationEnabled;
                }
            }
        }
    }
}