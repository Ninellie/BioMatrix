using UnityEngine;

public class WeaponCreater : MonoBehaviour
{
    public GameObject playerWeapon;
    
    private void Start()
    {
        CreateWeapon(playerWeapon);
    }
    private void CreateWeapon(GameObject weapons)
    {
        var weapon = Instantiate(weapons);

        weapon.transform.SetParent(transform);

        weapon.transform.position = gameObject.transform.position;
    }
}
