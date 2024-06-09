using UnityEngine;
using UnityEngine.Events;

namespace Core.Events
{
    public class GameObjectGameEventListener : MonoBehaviour
    {
        [Tooltip("Event to register with.")]
        public GameObjectGameEvent gameEvent;

        [Tooltip("Response to invoke when Event is raised.")]
        public UnityEvent<GameObject> response;

        private void OnEnable()
        {
            gameEvent.RegisterListener(this);
        }

        private void OnDisable()
        {
            gameEvent.UnregisterListener(this);
        }

        public void OnEventRaised(GameObject gameObject)
        {
            response.Invoke(gameObject);
        }
    }
}