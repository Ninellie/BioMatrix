using UnityEngine;

public class Repulse : MonoBehaviour
{
    [SerializeField] private float _baseRepulseForce;
    [SerializeField] private float _penetrationDistanceMultiplier;

    private CircleCollider2D _circleCollider;

    void Start()
    {
        _circleCollider = GetComponent<CircleCollider2D>();
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            var circleColliderOther = other.gameObject.GetComponent<CircleCollider2D>();
            
            Vector2 pushDirection = (transform.position - other.transform.position).normalized;

            float distance = Vector2.Distance(transform.position, other.transform.position);
            float radiiSum = _circleCollider.radius + circleColliderOther.radius;
            float multipliedRadiiSum = radiiSum * _penetrationDistanceMultiplier;
            float penetrationDepth = multipliedRadiiSum - distance;

            penetrationDepth = Mathf.Clamp(penetrationDepth, 0f, Mathf.Infinity);

            float repulseForce = _baseRepulseForce * penetrationDepth;

            transform.Translate(pushDirection * repulseForce * Time.deltaTime);
        }
    }
}
