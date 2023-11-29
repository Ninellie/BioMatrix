using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.EntityComponents.UnitComponents.ProjectileComponents
{
    public class ProjectileCollisionHandler : MonoBehaviour
    {
        [SerializeField] private string _otherTag;
        [SerializeField] private UnityEvent _onBecomeInvisible;
        [SerializeField] private UnityEvent _onCollisionEnter2D;

        private void OnCollisionEnter2D(Collision2D collision2D)
        {
            if (!collision2D.gameObject.CompareTag(_otherTag)) return;
            _onCollisionEnter2D.Invoke();
        }

        private void OnBecameInvisible()
        {
            _onBecomeInvisible.Invoke();
        }
    }
}