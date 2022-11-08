using System;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    private BulletSettings settings;
    private Camera m_camera;
    private bool isOnScreen;

    public void Awake()
    {
        m_camera = FindObjectOfType<Camera>();
        settings = GetComponent<BulletSettings>();
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
            //Destroy this bullet
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        isOnScreen = CheckVisibility();
        if (!isOnScreen)
        {
            Destroy(gameObject);
        }
    }
    private bool CheckVisibility()
    {
        var screenPos = m_camera.WorldToScreenPoint(transform.position);
        var onScreen = screenPos.x > 0f &&
                       screenPos.x < Screen.width &&
                       screenPos.y > 0f &&
                       screenPos.y < Screen.height;
        return onScreen;
    }
    private void SetProjectileSize(float size, GameObject projectile)
    {
        var projectileTransform = new Vector2(size, size);
        projectile.transform.localScale = projectileTransform;
    }
}
