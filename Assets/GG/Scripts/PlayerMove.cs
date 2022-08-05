using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float movementSpeed = 2f;
    private Rigidbody2D rb2d;

    private void Start()
    {
        rb2d = this.GetComponent<Rigidbody2D>();
    }

    //Return Vector3 from pressed keys
    private Vector2 moveVector
    {
        get
        {
            var horizontal = Input.GetAxisRaw("Horizontal");
            var vertical = Input.GetAxisRaw("Vertical");

            return new Vector2(horizontal, vertical);
        }
    }
    //Non-physical movement method
    private void MovementLogic()
    {


        transform.Translate(moveVector.normalized * movementSpeed * Time.deltaTime);
    }

    //Called once per frame
    private void Update()
    {
        //MovementLogic();
    }

    private void FixedUpdate()
    {
        rb2d.MovePosition(rb2d.position + moveVector.normalized * movementSpeed * Time.fixedDeltaTime);
    }
}
