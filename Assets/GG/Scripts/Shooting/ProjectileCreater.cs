using UnityEngine;

public class ProjectileCreater : MonoBehaviour
{
    public GameObject[] CreateProjectiles(int singleShotProjectiles, GameObject ammo, Transform firingPoint)
    {
        //Transform firingPoint = settings.transform;
        GameObject[] projectiles = new GameObject[singleShotProjectiles];
        for (int i = 0; i < projectiles.Length; i++)
        {
            projectiles[i] = Instantiate(ammo, firingPoint.position, firingPoint.rotation);
        }
        return projectiles;
    }
}