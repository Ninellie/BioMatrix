using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Diagnostics;

public class WeaponGun : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float projectileSpeed;
    public float fireRate;
    public int pierceNumber = 1;

    private Vector2 mousePosition;
    private Vector2 direction;

    public Stopwatch stopwatch = new();

    public float GetTotalSeconds()
    {
        TimeSpan ts = stopwatch.Elapsed;
        return (float)ts.TotalSeconds;
    }
    // Start is called before the first frame update
    void Start()
    {
        firePoint = this.transform;
        
        stopwatch.Start();
    }
    //If the reload has passed, then the shot is fired 
    public void Shoot()
    {
        if (GetTotalSeconds() >= (1f / fireRate))
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            direction = Lib2DMethods.DirectionToTarget(mousePosition, firePoint.position);
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            bullet.GetComponent<Bullet>().lifePoints = pierceNumber;
            Rigidbody2D rb2D = bullet.GetComponent<Rigidbody2D>();
            rb2D.AddForce(direction.normalized * projectileSpeed, ForceMode2D.Impulse);
            stopwatch.Restart();
        }
    }
    private void Update()
    {
        //Shoot if the fire button is pressed
        if (GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Player>().isFire)
        {
            Shoot();
        }
    }
}
