using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float movementSpeed = 2f;
    private Vector3 moveVector;
    private Rigidbody _rb;
    private BoxCollider2D _collider;
    void HandleInput()
    {
        moveVector.x = Input.GetAxis("Horizontal");
        moveVector.y = Input.GetAxis("Vertical");
    }
    private Vector3 _movementVector
    {
        get
        {
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");

            return new Vector3(horizontal, vertical, 0.0f);
        }
    }
    private void MovementLogic()
    {
        //HandleInput();
        //moveVector = new Vector3(moveVector.x, moveVector.y, 0.0f);
        //transform.Translate(moveVector.normalized * movementSpeed * Time.fixedDeltaTime);
    }

    private void MoveLogic()
    {
        _rb.AddForce(_movementVector * movementSpeed, ForceMode.Impulse);
    }

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        //MovementLogic();

    }

    // Update is called once per frame
    void Update()
    {

    }
}
