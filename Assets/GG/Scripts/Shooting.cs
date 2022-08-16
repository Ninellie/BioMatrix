using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shooting : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float bulletForce = 200f;
    public float fireRate = 1f;
    public Vector2 mousePosition;
    public Vector2 direction;

    //private bool isPressed;
    public void OnFire(InputValue input)
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        direction = Lib2DMethods.DirectionToMe(mousePosition);

        Shoot();
    }
    // Start is called before the first frame update
    void Start()
    {
        firePoint = this.transform;
    }
    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb2D = bullet.GetComponent<Rigidbody2D>();
        rb2D.AddForce(direction.normalized * bulletForce, ForceMode2D.Impulse);
    }
}
