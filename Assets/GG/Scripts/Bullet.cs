using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int lifePoints = 1;
    //public GameObject hitEffect;
    //public Vector2 direction;

    private Rigidbody2D rb2D;

    //public Vector2 Direction
    //{
    //    get
    //    {
    //        return direction;
    //    }
    //    set
    //    {
    //        direction = value;
    //    }
    //}

    private void Start()
    {
        rb2D = this.GetComponent<Rigidbody2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("The bullet hit the target");
        GameObject otherGO = collision.gameObject;
        switch (otherGO.tag)
        {
            case "Enemy":
                lifePoints--;
                if (lifePoints <= 0)
                {
                    //Destroy this enemy
                    Destroy(gameObject);
                }
                break;
        }
    }

    //private void FixedUpdate()
    //{
    //    Lib2DMethods.MovePhys2D(rb2D, direction.normalized, 200f);
    //}
}
