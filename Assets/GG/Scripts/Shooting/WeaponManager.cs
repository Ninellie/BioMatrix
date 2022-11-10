using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public GameObject[] playerWeapons;

    //Creates a weapon object and makes it a child of the player object
    public void CreateWeapon(GameObject[] weapons, int i)
    {
        GameObject weapon = Instantiate(weapons[i]);
        
        weapon.transform.SetParent(transform);

        weapon.transform.position = gameObject.transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        CreateWeapon(playerWeapons, 0);
    }
}
