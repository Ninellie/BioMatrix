using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Repulse : MonoBehaviour
{
    [SerializeField] private float _baseRepulseForce;
    [SerializeField] private float _penetrationDistanceMultiplier;
    [SerializeField] private LayerMask _layerMask;

    private CircleCollider2D _circleCollider;

    void Start()
    {
        _circleCollider = GetComponent<CircleCollider2D>();
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        Collider2D otherCollider2D = collision.collider;
        if (otherCollider2D.gameObject.layer != _layerMask) return;
        //if (otherCollider2D.gameObject.layer != LayerMask.NameToLayer("Enemy")) return;
        var circleColliderOther = otherCollider2D.gameObject.GetComponent<CircleCollider2D>();

        Vector2 pushDirection = (transform.position - otherCollider2D.transform.position).normalized;

        float distance = Vector2.Distance(transform.position, otherCollider2D.transform.position);
        float radiiSum = _circleCollider.radius + circleColliderOther.radius;
        float multipliedRadiiSum = radiiSum * _penetrationDistanceMultiplier;
        float penetrationDepth = multipliedRadiiSum - distance;

        penetrationDepth = Mathf.Clamp(penetrationDepth, 0f, Mathf.Infinity);

        float repulseForce = _baseRepulseForce * penetrationDepth;

        transform.Translate(pushDirection * repulseForce * Time.deltaTime);
    }

    //void OnTriggerStay2D(Collider2D otherCollider2D)
    //{
    //    if (otherCollider2D.gameObject.layer == LayerMask.NameToLayer("Enemy"))
    //    {
    //        var circleColliderOther = otherCollider2D.gameObject.GetComponent<CircleCollider2D>();
            
    //        Vector2 pushDirection = (transform.position - otherCollider2D.transform.position).normalized;

    //        float distance = Vector2.Distance(transform.position, otherCollider2D.transform.position);
    //        float radiiSum = _circleCollider.radius + circleColliderOther.radius;
    //        float multipliedRadiiSum = radiiSum * _penetrationDistanceMultiplier;
    //        float penetrationDepth = multipliedRadiiSum - distance;

    //        penetrationDepth = Mathf.Clamp(penetrationDepth, 0f, Mathf.Infinity);

    //        float repulseForce = _baseRepulseForce * penetrationDepth;

    //        transform.Translate(pushDirection * repulseForce * Time.deltaTime);
    //    }
    //}
}
