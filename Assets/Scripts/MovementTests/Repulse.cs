using System.Collections;
using System.Collections.Generic;
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
        //if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        //{
        //    Debug.Log("stay");
        //    Vector2 pushDirection = (transform.position - other.transform.position).normalized;
        //    transform.Translate(pushDirection * _repulseForce * Time.deltaTime);
        //}

        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            var circleColliderOther = other.gameObject.GetComponent<CircleCollider2D>();
            Debug.Log("stay");
            Vector2 pushDirection = (transform.position - other.transform.position).normalized;

            // Calculate penetration depth as the sum of half the collider sizes minus the distance between them
            //float penetrationDepth = (transform.localScale.x + other.transform.localScale.x) * 0.5f - Vector2.Distance(transform.position, other.transform.position);
            float distance = Vector2.Distance(transform.position, other.transform.position);
            float radiiSum = _circleCollider.radius + circleColliderOther.radius;
            float multipliedRadiiSum = radiiSum * _penetrationDistanceMultiplier;
            float penetrationDepth = multipliedRadiiSum - distance;

            // Clamp penetration depth to zero or positive values
            penetrationDepth = Mathf.Clamp(penetrationDepth, 0f, Mathf.Infinity);

            // Apply repulsion force based on penetration depth
            float repulseForce = _baseRepulseForce * penetrationDepth;

            transform.Translate(pushDirection * repulseForce * Time.deltaTime);
        }
    }
}
