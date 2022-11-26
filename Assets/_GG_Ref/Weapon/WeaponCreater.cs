using UnityEngine;

public class WeaponCreater : MonoBehaviour
{
    public GameObject[] playerWeapons;
    public void CreateWeapon(GameObject[] weapons, int i)
    {
        GameObject weapon = Instantiate(weapons[i]);
        
        weapon.transform.SetParent(transform);

        weapon.transform.position = gameObject.transform.position;
    }
    private void Start()
    {
        CreateWeapon(playerWeapons, 0);
    }
}
