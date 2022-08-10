using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public int lifePoints = 10;
    public float movementSpeed = 2f;

    private Rigidbody2D rb2D;
    private Vector2 moveVec;
    public void OnMove(InputValue input)
    {
        Vector2 inputVec = input.Get<Vector2>();
        moveVec = new Vector2(inputVec.x, inputVec.y);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        int newLifePointsValue = lifePoints - 2;

        Debug.Log("Dont touch the Hero!");
        GameObject otherGO = collision.gameObject;
        switch (otherGO.tag)
        {
            case "Enemy":
                lifePoints = newLifePointsValue;
                break;

            case "Projectile":
                lifePoints--;
                break;
        }


        if (lifePoints <= 0)
        {
            //Destroy this enemy
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        rb2D = this.GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        Lib2DMethods.MovePhys2D(rb2D, moveVec, movementSpeed);
    }
}
