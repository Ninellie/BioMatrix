using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Lib2DMethods
{
    /// Summary:
    ///     Moves Rigidbody2D of the object in a direction
    ///
    /// Parameters:
    ///   rigidBody2D:
    ///     The Rigidbody2D component of the object to be moved.
    ///
    ///   movementVector:
    ///     2D vector towards which the object will be moved
    ///
    ///   movementSpeed:
    ///     The speed at which the object will move
    public static void MovePhys2D(Rigidbody2D rigidBody2D, Vector2 movementVector, float movementSpeed)
    {
        rigidBody2D.MovePosition(rigidBody2D.position + movementVector.normalized * movementSpeed * Time.fixedDeltaTime);
    }
    public static void LookToPlayer(Rigidbody2D rigidBody2D)
    {
        float angle = (Mathf.Atan2(DirectionToPlayer(rigidBody2D.position).y, DirectionToPlayer(rigidBody2D.position).x) - Mathf.PI / 2) * Mathf.Rad2Deg;
        rigidBody2D.rotation = angle;
    }
    public static Vector2 PlayerPos
    {
        get
        {
            var horizontal = GameObject.FindGameObjectsWithTag("Player")[0].transform.position.x;
            var vertical = GameObject.FindGameObjectsWithTag("Player")[0].transform.position.y;

            return new Vector2(horizontal, vertical);
        }
     }
    public static Vector2 DirectionToPlayer(Vector2 myPos)
    {
        var horizontal = GameObject.FindGameObjectsWithTag("Player")[0].transform.position.x - myPos.x;
        var vertical = GameObject.FindGameObjectsWithTag("Player")[0].transform.position.y - myPos.y;

        return new Vector2(horizontal, vertical);
    }
    public static Vector2 inputVector
    {
        get
        {
            var horizontal = Input.GetAxisRaw("Horizontal");
            var vertical = Input.GetAxisRaw("Vertical");

            return new Vector2(horizontal, vertical);
        }
    }
    public static Vector2 RandOnCircle(float radius)
    {
        float randAng = UnityEngine.Random.Range(0, Mathf.PI * 2);
        return new Vector2(Mathf.Cos(randAng) * radius, Mathf.Sin(randAng) * radius);

    }
    public static float HypotenuseLength(float sideALength, float sideBLength)
    {
        return Mathf.Sqrt(sideALength * sideALength + sideBLength * sideBLength);
    }
}
