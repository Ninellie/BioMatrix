using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.EntityComponents.UnitComponents.BoonComponents
{
    public class Boon : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private Transform _transform;
        [SerializeField] private float _speed = 1500;
        [SerializeField] private string _playerTag;
        [SerializeField] private string _magnetTag;

        private void Awake()
        {
            if (_transform == null) _transform = transform;
            if (_rigidbody2D == null) _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void OnTriggerStay2D(Collider2D collider2D)
        {
            if (!collider2D.gameObject.CompareTag(_magnetTag)) return;
            Vector2 nextPosition = _transform.position;
            Vector2 direction = collider2D.transform.position - _transform.position;
            direction.Normalize();
            nextPosition += direction * _speed * Time.fixedDeltaTime;
            _rigidbody2D.MovePosition(nextPosition);
        }

        private void OnTriggerEnter2D(Collider2D collider2D)
        {
            if (!collider2D.gameObject.CompareTag(_playerTag)) return;
            Invoke(nameof(Death), 0.1f);
        }

        private void Death()
        {
            gameObject.SetActive(false);
        }
    }
}
