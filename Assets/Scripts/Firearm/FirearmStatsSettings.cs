using UnityEngine;

namespace Assets.Scripts.Firearm
{
    public class FirearmStatsSettings : MonoBehaviour
    {
        [SerializeField, Min(0), Tooltip("Damage additional modifier added to projectile damage")] public float damage;
        [SerializeField, Min(0), Tooltip("Speed additional modifier added to projectile speed")] public float shootForce;
        [SerializeField, Min(0), Tooltip("Number of projectiles fired per second")] public float shootsPerSecond;
        [SerializeField, Min(0), Tooltip("Maximum spread angle when firing")] public float maxShootDeflectionAngle;
        [SerializeField, Min(1), Tooltip("Maximum deviation of the projectile trajectory from the aiming point")] public float magazineCapacity;
        [SerializeField, Min(0.01f), Tooltip("Time in seconds needed for reload")] public float reloadTime;
        [SerializeField, Min(1), Tooltip("How many projectiles the Firearm fires per shot")] public float singleShootProjectile;
        [SerializeField, Tooltip("Damage multiplier modifier added to projectile size")] public float projectileSizeMultiplier;
        [SerializeField, Min(0), Tooltip("The number of enemy units that the projectile will pierce before being destroyed.")] public float projectilePierceCount;
        [SerializeField, Tooltip("Knockback additional modifier added to projectile knockback")] public float addedProjectileKnockback;
    }
}