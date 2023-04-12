using UnityEngine;

public class Knockback : MonoBehaviour
{
    [SerializeField] private float _knockbackForce;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("g");
            KnockbackFrom(other.gameObject, _knockbackForce);
        }
    }
    private void KnockbackFrom(GameObject other, float force)
    {
        Vector2 pushDirection = (transform.position - other.transform.position).normalized;
        transform.Translate(pushDirection * force);
    }
}
