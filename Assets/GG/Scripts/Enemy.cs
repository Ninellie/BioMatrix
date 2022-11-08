using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("EnemyCharacteristics")]
    public int lifePoints = 1;
    public bool isAlive;
    public float movementSpeed = 2f;
    public GameObject onDeathDrop;
    private Rigidbody2D rb2D;
    private int killDamage;
    

    [SerializeField] private GameObject damagePopup;
    private GameObject droppedDamagePopup;

    private Vector2 collisionPoint;

    void Start()
    {
        isAlive = true;
        rb2D = this.GetComponent<Rigidbody2D>();
        killDamage = lifePoints;
    }
    void FixedUpdate()
    {
        //Turns the face of this object towards the player
        Lib2DMethods.LookToPlayer(rb2D);
        //Moves this object to the player
        Lib2DMethods.MovePhys2D(rb2D, Lib2DMethods.DirectionToPlayer(rb2D.position), movementSpeed);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject otherGO = collision.gameObject;
        collisionPoint = otherGO.transform.position;
        switch (otherGO.tag)
        {
            case "Player":
                Die();
                break;

            case "Projectile":
                if (lifePoints > 0)
                {
                    TakeDamage(killDamage);
                    if (lifePoints <= 0)
                    {
                        DropBonus();
                        Die();
                    }
                }

                break;
        }
    }

    private void TakeDamage(int DamageTaken)
    {
        isAlive = false;
        lifePoints -= DamageTaken;
        Debug.Log(lifePoints);
        DropDamagePopup(DamageTaken, collisionPoint);
    }
    private void Die()
    {
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
    private void DropBonus()
    {
        Instantiate(onDeathDrop, rb2D.position, rb2D.transform.rotation);
    }
    private void DropDamagePopup(int damage, Vector2 transform)
    {
        droppedDamagePopup = Instantiate(damagePopup);
        droppedDamagePopup.transform.position = transform;
        droppedDamagePopup.GetComponent<DamagePopup>().Setup(damage);
    }
}
