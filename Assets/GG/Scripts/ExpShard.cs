using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpShard : MonoBehaviour
{
    public int lifePoints = 1;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("The ExpShard was taken");
        GameObject otherGO = collision.gameObject;
        switch (otherGO.tag)
        {
            case "Enemy":
                break;
            case "Player":
                lifePoints--;
                break;
            case "Projectile":
                break;
            case "Boon":

                break;
        }

        if (lifePoints <= 0)
        {
            //Destroy this expShard
            Destroy(gameObject);
        }
    }
}
