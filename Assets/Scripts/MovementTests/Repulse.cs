using UnityEngine;

public class Repulse : MonoBehaviour
{
    [SerializeField] private float _baseRepulseForce;
    [SerializeField] private float _penetrationDistanceMultiplier;
    [SerializeField] private string _layerName;

    private CircleCollider2D _circleCollider;
    private Collider2D _collider;

    void Start()
    {
        _collider = GetComponent<Collider2D>();
        if (TryGetComponent<CircleCollider2D>(out _circleCollider))
        {
            _circleCollider = GetComponent<CircleCollider2D>();
        }
    }
    //void OnCollisionStay2D(Collision2D collision)
    //{
    //    Collider2D otherCollider2D = collision.collider;
    //    if (otherCollider2D.gameObject.layer != LayerMask.NameToLayer(_layerName)) return;

    //    var circleColliderOther = otherCollider2D.gameObject.GetComponent<CircleCollider2D>();
    //    Vector2 pushDirection = (transform.position - otherCollider2D.transform.position).normalized;
    //    float distance = Vector2.Distance(transform.position, otherCollider2D.transform.position);
    //    float radiiSum = _circleCollider.radius + circleColliderOther.radius;
    //    float multipliedRadiiSum = radiiSum * _penetrationDistanceMultiplier;
    //    float penetrationDepth = multipliedRadiiSum - distance;

    //    penetrationDepth = Mathf.Clamp(penetrationDepth, 0f, Mathf.Infinity);

    //    float repulseForce = _baseRepulseForce * penetrationDepth;

    //    transform.Translate(pushDirection * repulseForce * Time.deltaTime);
    //}
    void OnCollisionStay2D(Collision2D collision)
    {
        Collider2D otherCollider2D = collision.collider;

        if (otherCollider2D.gameObject.layer != LayerMask.NameToLayer(_layerName)) return;

        if (otherCollider2D is CircleCollider2D)
        {
            var circleColliderOther = otherCollider2D.gameObject.GetComponent<CircleCollider2D>();
            float distance = Vector2.Distance(transform.position, otherCollider2D.transform.position);
            float radiiSum = _circleCollider.radius + circleColliderOther.radius;
            float multipliedRadiiSum = radiiSum * _penetrationDistanceMultiplier;
            float penetrationDepth = multipliedRadiiSum - distance;

            penetrationDepth = Mathf.Clamp(penetrationDepth, 0f, Mathf.Infinity);

            float repulseForce = _baseRepulseForce * penetrationDepth;

            Vector2 repulseVector = (transform.position - otherCollider2D.transform.position).normalized;

            repulseVector *= repulseForce;

            transform.Translate(repulseVector * Time.deltaTime);
        }
        else
        {
            Debug.LogWarning("else");
            float penetrationDepth = 0f;

            foreach (ContactPoint2D contact in collision.contacts)
            {
                float distance = Vector2.Distance(transform.position, contact.point);
                float radiiSum = _collider.bounds.extents.magnitude + otherCollider2D.bounds.extents.magnitude;
                float multipliedRadiiSum = radiiSum * _penetrationDistanceMultiplier;
                float depth = multipliedRadiiSum - distance;

                penetrationDepth = Mathf.Max(penetrationDepth, depth);
            }

            float repulseForce = _baseRepulseForce * penetrationDepth;

            Vector2 repulseVector = collision.contacts[0].normal * repulseForce;

            transform.Translate(repulseVector * Time.deltaTime);
        }
    }
}
