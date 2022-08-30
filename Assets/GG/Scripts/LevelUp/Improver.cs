using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Improver : MonoBehaviour
{
    public CardManager cardManager = new CardManager();

    public void Improve(CardManager.Card card)
    {
        var key = card.improvement.Keys.First();
        var value = card.improvement.Values.First();
        Debug.Log(key);
        Debug.Log(value);

        switch (key)
        {
            case "movementSpeed":
                GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Player>().movementSpeed += value;
                break;
            case "maximumHP":
                GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Player>().movementSpeed += value;
                break;
            case "shieldCount":

                break;
            case "regenerationRate":

                break;
            case "fireRate":
                GameObject.FindGameObjectsWithTag("Player")[0].GetComponentInChildren<WeaponGun>().fireRate += value;
                break;
            case "projectileSpeed":
                GameObject.FindGameObjectsWithTag("Player")[0].GetComponentInChildren<WeaponGun>().projectileSpeed += value;
                break;
            case "pierceNumber":
                GameObject.FindGameObjectsWithTag("Player")[0].GetComponentInChildren<WeaponGun>().pierceNumber += (int)value;
                break;
            case "reloadSpeed":

                break;
            case "projectileNumber":

                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Improve(cardManager.GetRandomCard());
    }

    // Update is called once per frame
    void Update()
    {

    }
}
