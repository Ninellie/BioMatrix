using System;

namespace Assets.Scripts.EntityComponents
{
    [Serializable]
    public enum StatName
    {
        None,
        // Anything that has a collider
        Size,
        // Anything that can take damage
        MaximumHealth,
        Regeneration,
        // Anything that can knockback
        KnockbackPower,
        // Anything that can move
        MovementSpeed,
        // Anything that has a collider
        RotationSpeed,
        // Anything that can inflict damage
        Damage,
        // Anything that can level up
        ExperienceMultiplier,
        // Anything that can attract boons
        MagnetismRadius,

        // Special for Shields
        MaximumLayers,
        MaximumRechargeableLayers,
        RechargeRatePerMinute,
        RepulseForce,
        RepulseRadius,

        // Special for Firearms
        ShootForce,
        ShootsPerSecond,
        MaxShootDeflectionAngle,
        MagazineCapacity,
        ReloadTime,
        Projectiles,
        ProjectileSizeMultiplier,
        Pierce,
        AddedProjectileKnockback,

        // Special for Enemies
        SpawnWeight,
    }
}