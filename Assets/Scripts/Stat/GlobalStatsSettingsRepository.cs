public class GlobalStatsSettingsRepository
{
    public static readonly EntityStatsSettings EntityStats = new EntityStatsSettings
    {
        Size = 1,
        MaximumLife = 1,
        LifeRegenerationInSecond = 0,
        KnockbackPower = 0,
    };

    public static readonly EnclosureStatsSettings EnclosureStats = new EnclosureStatsSettings()
    {
        Size = 1,
        MaximumLife = 1000,
        LifeRegenerationInSecond = 0,
        KnockbackPower = 1100,
        ConstrictionRate = 0.00035f,
    };

    public static readonly UnitStatsSettings UnitStats = new UnitStatsSettings()
    {
        Size = 1,
        MaximumLife = 1,
        LifeRegenerationInSecond = 0,
        Speed = 0,
        KnockbackPower = 0,
    };

    public static readonly PlayerStatsSettings PlayerStats = new PlayerStatsSettings()
    {
        Size = 1,
        MaximumLife = 30000,
        LifeRegenerationInSecond = 0,
        Speed = 170,
        MagnetismRadius = 150,
        MagnetismPower = 1000,
        KnockbackPower = 200,
    };

    public static readonly EnemyStatsSettings EnemyStats = new EnemyStatsSettings()
    {
        Size = 0.8f,
        MaximumLife = 2,
        LifeRegenerationInSecond = 0,
        Speed = 100,
        SpawnWeight = 1000,
        KnockbackPower = 700,
        AccelerationSpeed = 200,
        RotationSpeed = 5,
    };

    public static readonly UnitStatsSettings BoonStats = new UnitStatsSettings()
    {
        Size = 1,
        MaximumLife = 1,
        LifeRegenerationInSecond = 0,
        Speed = 0,
        KnockbackPower = 0,
    };
    public static readonly UnitStatsSettings ProjectileStats = new UnitStatsSettings()
    {
        Size = 1,
        MaximumLife = 1,
        LifeRegenerationInSecond = 0,
        Speed = 700,
        KnockbackPower = 100,
    };

    public static readonly FirearmStatsSettings ShotgunStats = new FirearmStatsSettings()
    {
        Damage = 1,
        ShootForce = 500,
        ShootsPerSecond = 3,
        MaxShootDeflectionAngle = 1,
        MagazineSize = 6,
        ReloadSpeed = 1,
        SingleShootProjectile = 1,
    };
}