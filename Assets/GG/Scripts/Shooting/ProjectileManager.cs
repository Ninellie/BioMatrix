using System;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    private ProjectileSettings settings;
    private Camera m_camera;
    private bool isOnScreen;

    public void Awake()
    {
        m_camera = FindObjectOfType<Camera>();
        settings = GetComponent<ProjectileSettings>();
        SetProjectileSize(settings.size, gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject otherGO = collision.gameObject;
        switch (otherGO.tag)
        {
            case "Enemy":
                if (otherGO.GetComponent<Enemy>().lifePoints > 0)
                {
                    settings.pierceCount--;
                }
                break;
        }

        if (settings.pierceCount <= 0)
        {
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        isOnScreen = Lib2DMethods.CheckVisibilityOnCamera(m_camera, gameObject);
        if (!isOnScreen)
        {
            Destroy(gameObject);
        }
    }
    private void SetProjectileSize(float size, GameObject projectile)
    {
        var projectileTransform = new Vector2(size, size);
        projectile.transform.localScale = projectileTransform;
    }
}
