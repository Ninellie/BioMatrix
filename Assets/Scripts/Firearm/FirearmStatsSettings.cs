using UnityEngine;

namespace Assets.Scripts.Firearm
{
    public class FirearmStatsSettings : MonoBehaviour
    {
        [SerializeField, Min(0)] public float damage;
        [SerializeField, Min(0)] public float shootForce;
        [SerializeField, Min(0)] public float shootsPerSecond;
        [SerializeField, Min(0)] public float maxShootDeflectionAngle;
        [SerializeField, Min(1)] public float magazineCapacity;
        [SerializeField, Min(0.01f)] public float reloadSpeed;
        [SerializeField, Min(1)] public float singleShootProjectile;
        [SerializeField] public float projectileSizeMultiplier;
        [SerializeField, Min(0)] public float projectilePierceCount;
        [SerializeField] public float addedProjectileKnockback;
    }
}