using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public GameObject[] playerWeapons;

    //Creates a weapon object and makes it a child of the player object
    public void CreateWeapon(GameObject[] weapons, int i)
    {
        GameObject weapon = Instantiate(weapons[i]);
        weapon.transform.SetParent(this.transform);
    }

    // Start is called before the first frame update
    void Start()
    {
        CreateWeapon(playerWeapons, 0);
    }
}
