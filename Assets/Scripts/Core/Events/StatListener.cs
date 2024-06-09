using Core.Variables;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Events
{
    public class StatListener : MonoBehaviour
    {
        [Tooltip("Event to register with")]
        public StatVariable statChangedEvent;

        [Tooltip("Response to invoke when stat value changes")]
        public UnityEvent<float> onChange;


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
            onChange.Invoke(statChangedEvent.value);
        }
    }
}