using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Assets.Scripts.EntityComponents.UnitComponents.EnemyComponents
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private EnemyType _enemyType;
        public bool IsAlive { get; private set; }
        public EnemyType EnemyType => _enemyType;
        private Rigidbody2D _rigidbody2D;

        private void Awake()
        {
            Debug.Log($"Enemy {gameObject.name} Awake");
            IsAlive = true;
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        //todo to rotationController?
        public void LookAt2D(Vector2 targetPosition)
        {
            var direction = (Vector2)_rigidbody2D.transform.position - targetPosition;
            var angle = (Mathf.Atan2(direction.y, direction.x) + Mathf.PI / 2) * Mathf.Rad2Deg;
            _rigidbody2D.rotation = angle;
            _rigidbody2D.SetRotation(angle);
        }
    }
}