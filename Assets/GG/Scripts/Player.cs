using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class Player : MonoBehaviour
{
    public int lifePoints = 10;
    public float movementSpeed = 2f;

    private Rigidbody2D rb2D;
    private Vector2 moveVec;
    private SpriteRenderer sprite;

    public static Action OnGamePaused;
    public static Action OnCharacterDeath;

    //Is the fire button pressed
    public bool isFire;

    public void OnFire()
    {
        isFire = true;
    }
    public void OnFireOff()
    {
        isFire = false;
    }
    public void OnMove(InputValue input)
    {
        Vector2 inputVec = input.Get<Vector2>();
        moveVec = new Vector2(inputVec.x, inputVec.y);
        if (inputVec.x < 0)
        {
            sprite.flipX = true;
        }
        if (inputVec.x > 0)
        {
            sprite.flipX = false;
        }
    }

    public void OnPause(InputValue input)
    {
        OnGamePaused?.Invoke();
        Debug.Log("Game on pause");
    }

    public void OnUnpause(InputValue input)
    {
        OnGamePaused?.Invoke();
        Debug.Log("Game is active");
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
            OnCharacterDeath?.Invoke();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        isFire = false;
        sprite = this.GetComponent<SpriteRenderer>();
        rb2D = this.GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        Lib2DMethods.MovePhys2D(rb2D, moveVec, movementSpeed);
    }
}
