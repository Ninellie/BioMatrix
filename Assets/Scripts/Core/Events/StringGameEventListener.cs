using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Core.Events
{
    public class StringGameEventListener : MonoBehaviour
    {
        [Tooltip("Event to register with.")]
        public StringGameEvent gameEvent;

        [Tooltip("Response to invoke when Event is raised.")]
        public UnityEvent<string> response;

        private void OnEnable()
        {
            gameEvent.RegisterListener(this);
        }

        private void OnDisable()
        {
            gameEvent.UnregisterListener(this);
        }

        public void OnEventRaised(string str)
        {
            response.Invoke(str);
        }
    }
}