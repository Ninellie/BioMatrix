using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float movementSpeed = 2f;

    //Return Vector3 from pressed keys
    private Vector3 moveVector
    {
        get
        {
            var horizontal = Input.GetAxisRaw("Horizontal");
            var vertical = Input.GetAxisRaw("Vertical");

            //The z-axis does not change
            return new Vector3(horizontal, vertical, 0.0f);
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
        MovementLogic();
    }
}
