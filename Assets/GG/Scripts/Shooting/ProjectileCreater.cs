using UnityEngine;

public class ProjectileCreater : MonoBehaviour
{
    public GameObject[] CreateProjectiles(FirearmSettings settings)
    {
        Transform firingPoint = settings.transform;
        GameObject[] projectiles = new GameObject[settings.SingleShotProjectiles];
        for (int i = 0; i < projectiles.Length; i++)
        {
            projectiles[i] = Instantiate(settings.Ammo, firingPoint.position, firingPoint.rotation);
        }
        return projectiles;
    }
}