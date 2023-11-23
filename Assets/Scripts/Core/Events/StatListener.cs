using Assets.Scripts.Core.Variables;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Core.Events
{
    public class StatListener : MonoBehaviour
    {
        [Tooltip("Event to register with")]
        public StatVariable statChangedEvent;

        [Tooltip("Response to invoke when stat value changes")]
        public UnityEvent<float> statValueResponse;
        public UnityEvent response;

        /*
     * Size
     * MagnetismRadius
     *
     * Player reserves
     * Life onEdge
     * Life decrease
     * Life on zero
     *
     * Last ammo
     */

        private void OnEnable()
        {
            statChangedEvent.RegisterListener(this);
        }

        private void OnDisable()
        {
            statChangedEvent.UnregisterListener(this);
        }

        public void OnEventRaised()
        {
            response.Invoke();
            statValueResponse.Invoke(statChangedEvent.value);
        }
    }
}