using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("EnemyCharacteristics")]
    public int lifePoints = 1;
    public float movementSpeed = 2f;
    public GameObject onDeathDrop;

    private Rigidbody2D rb2D;
    public bool isOnScreen;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Dont touch me!");
        GameObject otherGO = collision.gameObject;
        switch (otherGO.tag)
        {
            case "Player":
                    //Destroy this enemy
                    Destroy(gameObject);
                break;

            case "Projectile":
                lifePoints--;
                if (lifePoints <= 0)
                {
                    //Drop experiance shard
                    Instantiate(onDeathDrop, rb2D.position, rb2D.transform.rotation);
                    //Destroy this enemy
                    Destroy(gameObject);
                }
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb2D = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Turns the face of this object towards the player
        Lib2DMethods.LookToPlayer(rb2D);

        //Moves this object to the player
        Lib2DMethods.MovePhys2D(rb2D, Lib2DMethods.DirectionToPlayer(rb2D.position), movementSpeed);
    }
}
