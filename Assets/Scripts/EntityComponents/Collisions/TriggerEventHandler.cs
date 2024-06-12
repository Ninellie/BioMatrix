using UnityEngine;
using UnityEngine.Events;

namespace EntityComponents.Collisions
{
    public class TriggerEventHandler : MonoBehaviour
    {
        [Header("Collision object")]
        [SerializeField] private string _tag;
        [Header("Response")]
        [SerializeField] private UnityEvent<Collider2D> _onTriggerEnter2D;

        private void OnTriggerEnter2D(Collider2D collider2D)
        {
            if (collider2D.tag != _tag) return;
            _onTriggerEnter2D.Invoke(collider2D);
        }
    }
}