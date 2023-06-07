using UnityEngine;

namespace Assets.Scripts.Entity.Unit.Projectile
{
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
        //public GameObject[] CreateProjectiles(int singleShotProjectiles, GameObject ammo, Transform firingPoint, StatModifier sizeMod, StatModifier pierceMod)
        //{
        //    var projectiles = new GameObject[singleShotProjectiles];
        //    int i = 0;
        //    foreach (var projectile in projectiles)
        //    {
        //        var proj = projectile.GetComponent<Projectile>();
        //        proj.Size.AddModifier(sizeMod);
        //        proj.MaximumLifePoints.AddModifier(pierceMod);
        //        projectiles[i] = Instantiate(ammo, firingPoint.position, firingPoint.rotation);
        //        i++;
        //    }
        //    return projectiles;
        //}
    }
}