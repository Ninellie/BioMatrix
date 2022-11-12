using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("EnemyCharacteristics")]
    public int lifePoints;
    public bool isAlive;
    public float movementSpeed;
    public GameObject onDeathDrop;
    private Rigidbody2D rb2D;
    private int killDamage;
    

    [SerializeField] private GameObject damagePopup;
    //private GameObject droppedDamagePopup;

    private Vector2 collisionPoint;

    void Start()
    {
        isAlive = true;
        rb2D = this.GetComponent<Rigidbody2D>();
        killDamage = lifePoints;
        
    }
    private void Awake()
    {
        gameObject.AddComponent<Movement>();
        gameObject.GetComponent<Movement>().ChangeMode(MovementMode.Seek);
        gameObject.GetComponent<Movement>().SetPursuingTarget(GameObject.FindGameObjectsWithTag("Player")[0]);
        gameObject.GetComponent<Movement>().Accelerate(movementSpeed);
    }

    //void FixedUpdate()
    //{
    //    //Turns the face of this object towards the player
    //    Lib2DMethods.LookToPlayer(rb2D);
    //    //Moves this object to the player
    //    Lib2DMethods.MovePhys2D(rb2D, Lib2DMethods.DirectionToPlayer(rb2D.position), movementSpeed);
    //}
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
                    TakeDamage(1);
                    if (lifePoints <= 0)
                    {
                        Die();
                        DropBonus();
                    }
                }

                break;
        }
    }

    private void TakeDamage(int DamageTaken)
    {
        lifePoints -= DamageTaken;
        DropDamagePopup(DamageTaken, collisionPoint);
    }
    private void Die()
    {
        isAlive = false;
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
    private void DropBonus()
    {
        Instantiate(onDeathDrop, rb2D.position, rb2D.transform.rotation);
    }
    private void DropDamagePopup(int damage, Vector2 transform)
    {
        GameObject droppedDamagePopup = Instantiate(damagePopup);
        droppedDamagePopup.transform.position = transform;
        droppedDamagePopup.GetComponent<DamagePopup>().Setup(damage);
    }
}
