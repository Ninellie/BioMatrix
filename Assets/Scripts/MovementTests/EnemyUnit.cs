using System.Collections;
using UnityEngine;

public class EnemyUnit : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float maxSpeed = 10f;
    //[SerializeField] private float acceleration = 1f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerTransform = FindObjectOfType<PlayerController>().transform;
    }
    void FixedUpdate()
    {
        Vector2 playerDirection = (playerTransform.position - transform.position).normalized;

        Vector2 movement = playerDirection * maxSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);
        //Vector2 direction = (playerTransform.position - transform.position).normalized;
        //Vector2 targetPosition = (Vector2)transform.position + (direction * maxSpeed * Time.deltaTime);
        //transform.position = Vector2.Lerp(transform.position, targetPosition, acceleration * Time.deltaTime);
    }
}