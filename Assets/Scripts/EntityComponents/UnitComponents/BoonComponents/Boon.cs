using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.BoonComponents
{
    public class Boon : MonoBehaviour
    {
        [SerializeField] private int _experienceAmount = 4;
        [SerializeField] private float speed = 1500;
        private Rigidbody2D _rigidbody2D;

        private void Awake()
        {
            Debug.Log($"{gameObject.name} Boon Awake");
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void OnTriggerStay2D(Collider2D collider2D)
        {
            if (!collider2D.gameObject.CompareTag("Player")) return;
            Vector2 nextPosition = transform.position;
            Vector2 direction = collider2D.transform.position - transform.position;
            direction.Normalize();
            nextPosition += direction * speed * Time.fixedDeltaTime;
            _rigidbody2D.MovePosition(nextPosition);
        }

        private void OnTriggerEnter2D(Collider2D collider2D)
        {
            var isBoxCollider = collider2D is BoxCollider2D;
            if (!isBoxCollider) return;
            if (!collider2D.gameObject.CompareTag("Player")) return;
            Debug.Log("The exp boon was taken");
            gameObject.SetActive(false);
            Destroy(gameObject);
        }

        public int GetExperience()
        {
            return _experienceAmount;
        }
    }
}
