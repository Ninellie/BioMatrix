using UnityEngine;

public class Knockback : MonoBehaviour
{
    [SerializeField] private float _knockbackForce;
    private Rigidbody2D _rigidbody2D;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }
    private void OnTriggerEnter2D(Collider2D otherCollider2D) 
    {
        if (otherCollider2D.gameObject.CompareTag("Player"))
        {
            Debug.Log("g");
            KnockbackFrom(otherCollider2D.gameObject, _knockbackForce);
        }
    }
    private void KnockbackFrom(GameObject other, float force)
    {
        Vector2 pushDirection = (transform.position - other.transform.position).normalized;
        transform.Translate(pushDirection * force);
    }
}
