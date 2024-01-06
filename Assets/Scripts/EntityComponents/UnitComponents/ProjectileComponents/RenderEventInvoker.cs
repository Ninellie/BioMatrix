using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.EntityComponents.UnitComponents.ProjectileComponents
{
    public class RenderEventInvoker : MonoBehaviour
    {
        [SerializeField] private bool _invokeOnBecameInvisible;
        [SerializeField] private UnityEvent _onCollisionEnter2D;

        [SerializeField] private bool _invokeOnBecameVisible;
        [SerializeField] private UnityEvent _onCollisionExit2D;

        private void OnBecameInvisible()
        {
            if (!_invokeOnBecameInvisible) return;
            _onCollisionEnter2D.Invoke();
        }

        private void OnBecameVisible()
        {
            if (!_invokeOnBecameVisible) return;
            _onCollisionExit2D.Invoke();
        }
    }
}