using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Diagnostics;

public class Revolver : MonoBehaviour
{
    public GameObject reloadLable;
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float projectileSpeed;
    public float fireRate;
    public int pierceNumber;
    public int magazineMax;
    public int magazineCurrent;
    public float reloadSpeed;

    private Vector2 mousePosition;
    private Vector2 direction;

    public Stopwatch fireDelayStopwatch = new();
    public Stopwatch reloadStopwatch = new();

    public float GetTotalSeconds(Stopwatch stopwatch)
    {
        TimeSpan timeSpan = stopwatch.Elapsed;
        return (float)timeSpan.TotalSeconds;
    }

    private void Reload()
    {
        reloadLable.SetActive(true);
        Invoke("ReloadOff", 1 / reloadSpeed);
    }

    private void ReloadOff()
    {
        reloadLable.SetActive(false);
        magazineCurrent = magazineMax;
    }

    // Start is called before the first frame update
    void Start()
    {
        reloadSpeed = 0.8f;
        pierceNumber = 1;
        magazineMax = 6;
        magazineCurrent = magazineMax;
        reloadLable = GameObject.FindWithTag("Canvas").GetComponent<ReloadLabel>().reloadLabel;
        reloadLable.SetActive(false);
        firePoint = GetComponentInParent<WeaponManager>().transform;
        
        fireDelayStopwatch.Start();
        reloadStopwatch.Stop();
    }
    //If the reload has passed, then the shot is fired 
    public void Shoot()
    {
        if (GetTotalSeconds(fireDelayStopwatch) >= (1f / fireRate) && magazineCurrent > 0)
        {
            magazineCurrent--;
            mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            direction = Lib2DMethods.DirectionToTarget(mousePosition, firePoint.position);
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            bullet.GetComponent<Bullet>().lifePoints = pierceNumber;
            Rigidbody2D rb2D = bullet.GetComponent<Rigidbody2D>();
            rb2D.AddForce(direction.normalized * projectileSpeed, ForceMode2D.Impulse);
            fireDelayStopwatch.Restart();
            if(magazineCurrent <= 0)
            {
                Reload();
            }
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
