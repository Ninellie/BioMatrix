using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.EntityComponents.UnitComponents.ProjectileComponents
{
    public class ProjectileCollisionHandler : MonoBehaviour
    {
        [SerializeField] private string _enemyTag;
        [SerializeField] private UnityEvent _onBecomeInvisible;
        [SerializeField] private UnityEvent<int> _onCollision;

        private void OnCollisionEnter2D(Collision2D collision2D)
        {
            var otherCollider2D = collision2D.collider;
            if (!otherCollider2D.gameObject.CompareTag(_enemyTag)) return;
            _onCollision.Invoke(1);
        }

        private void OnBecameInvisible()
        {
            _onBecomeInvisible.Invoke();
        }
    }
}