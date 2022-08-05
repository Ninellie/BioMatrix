using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : Player
{
    private Rigidbody2D rb2D;

    private void Start()
    {
        rb2D = this.GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        Lib2DMethods.MovePhys2D(rb2D, Lib2DMethods.inputVector, movementSpeed);
    }
}
