using UnityEngine;
using UnityEngine.Events;

namespace EntityComponents
{
    public class LifeTimer : MonoBehaviour
    {
        public float lifeTime;

        public UnityEvent onEnable;
        public UnityEvent onDisable;

        private void OnEnable()
        {
            lifeTime = 0;
            onEnable.Invoke();
        }

        private void OnDisable()
        {
            Debug.Log($"Life time: {lifeTime}");
            onDisable.Invoke();
        }

        private void Update()
        {
            lifeTime += Time.deltaTime;
        }
    }
}