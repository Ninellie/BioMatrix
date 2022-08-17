using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public bool isOnScreen;
    public int lifePoints = 1;

    public Camera m_camera;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("The bullet hit the target");
        GameObject otherGO = collision.gameObject;
        switch (otherGO.tag)

        {
            case "Enemy":
                lifePoints--;
                break;
        }

        if (lifePoints <= 0)
        {
            //Destroy this bullet
            Destroy(gameObject);
        }
    }
    public void Start()
    {
        m_camera = FindObjectOfType<Camera>();
    }
    public bool CheckVisibility()
    {
        var screenPos = m_camera.WorldToScreenPoint(transform.position);
        var onScreen = screenPos.x > 0f &&
                       screenPos.x < Screen.width &&
                       screenPos.y > 0f &&
                       screenPos.y < Screen.height;
        return onScreen;
    }

    private void Update()
    {
        if (!CheckVisibility())
        {
            Destroy(gameObject);
        }
    }
}
