using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float movementSpeed = 2f;
    private Rigidbody2D rb2D;

    //Calculates the angle between this object and the player
    private Vector2 directionToPlayer
    {
        get
        {
            var horizontal = GameObject.FindGameObjectsWithTag("Player")[0].transform.position.x - transform.position.x;
            var vertical = GameObject.FindGameObjectsWithTag("Player")[0].transform.position.y - transform.position.y;

            return new Vector2(horizontal, vertical);
        }
    }
    float Moves()
    {
        if (directionToPlayer.x != 0 || directionToPlayer.y != 0)
            return 1f;
        else
            return 0f;
    }
    //Moves this object to the player
    private void MovementLogic()
    {
        transform.Translate(Moves() * transform.up * movementSpeed * Time.fixedDeltaTime, Space.World);
    }
    //Turns the face of this object towards the player
    private void LookToPlayer()
    {
        float angle = (Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) - Mathf.PI / 2) * Mathf.Rad2Deg;
        rb2D.rotation = angle;
    }
    // Start is called before the first frame update
    void Start()
    {
        rb2D = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        LookToPlayer();
        MovementLogic();
    }
}
