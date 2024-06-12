using UnityEngine;
using UnityEngine.Events;

namespace EntityComponents.UnitComponents.ProjectileComponents
{
    public class Collision2DEventInvoker : MonoBehaviour
    {
        [SerializeField] private string _otherTag;

        [SerializeField] private bool _invokeOnCollisionEnter2D;
        [SerializeField] private UnityEvent<Collision2D> _onCollisionEnter2D;

        [SerializeField] private bool _invokeOnCollisionExit2D;
        [SerializeField] private UnityEvent<Collision2D> _onCollisionExit2D;

        [SerializeField] private bool _invokeOnCollisionStay2D;
        [SerializeField] private UnityEvent<Collision2D> _onCollisionStay2D;

        private void OnCollisionEnter2D(Collision2D collision2D)
        {
            if (!_invokeOnCollisionEnter2D) return;
            if (!collision2D.gameObject.CompareTag(_otherTag)) return;
            _onCollisionEnter2D.Invoke(collision2D);
        }

        private void OnCollisionExit2D(Collision2D collision2D)
        {
            if (!_invokeOnCollisionExit2D) return;
            if (!collision2D.gameObject.CompareTag(_otherTag)) return;
            _onCollisionExit2D.Invoke(collision2D);
        }

        private void OnCollisionStay2D(Collision2D collision2D)
        {
            if (!_invokeOnCollisionStay2D) return;
            if (!collision2D.gameObject.CompareTag(_otherTag)) return;
            _onCollisionStay2D.Invoke(collision2D);
        }
    }
}