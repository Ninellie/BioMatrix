using UnityEngine;

public class ProjectileCreator : MonoBehaviour
{
    public GameObject[] CreateProjectiles(int singleShotProjectiles, GameObject ammo, Transform firingPoint)
    {
        var projectiles = new GameObject[singleShotProjectiles];
        for (var i = 0; i < projectiles.Length; i++)
        {
            projectiles[i] = Instantiate(ammo, firingPoint.position, firingPoint.rotation);
        }
        return projectiles;
    }
}