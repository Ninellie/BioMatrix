using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Core.Events
{
    public class TransformGameEventListener : MonoBehaviour
    {
        [Tooltip("Event to register with.")]
        public TransformGameEvent gameEvent;

        [Tooltip("Response to invoke when Event is raised.")]
        public UnityEvent<Transform> response;

        private void OnEnable()
        {
            gameEvent.RegisterListener(this);
        }

        private void OnDisable()
        {
            gameEvent.UnregisterListener(this);
        }

        public void OnEventRaised(Transform transform)
        {
            response.Invoke(transform);
        }
    }
}