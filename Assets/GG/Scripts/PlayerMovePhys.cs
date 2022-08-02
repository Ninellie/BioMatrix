using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovePhys : MonoBehaviour
{

    public float movementSpeed = 2f;
    private Vector3 moveVector;
    private Rigidbody2D _rb;

    private Vector3 _movementVector
    {
        get
        {
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");

            return new Vector3(horizontal, vertical, 0.0f);
        }
    }

    private void MoveLogic()
    {
        _rb.AddForce(_movementVector * movementSpeed, ForceMode2D.Impulse);
    }

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveLogic();
    }
}
