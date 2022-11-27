public class GlobalStatsSettingsRepository
{
    public static readonly EntityStatsSettings EntityStats = new EntityStatsSettings
    {
        Size = 1,
        MaximumLife = 1,
    };

    public static readonly UnitStatsSettings UnitStats = new UnitStatsSettings()
    {
        Size = 1,
        MaximumLife = 1,
        Speed = 100,
    };

    public static readonly UnitStatsSettings PlayerStats = new UnitStatsSettings()
    {
        Size = 1,
        MaximumLife = 5,
        Speed = 600,
    };

    public static readonly UnitStatsSettings EnemyStats = new UnitStatsSettings()
    {
        Size = 1,
        MaximumLife = 5,
        Speed = 100,
    };

    public static readonly UnitStatsSettings BoonStats = new UnitStatsSettings()
    {
        Size = 1,
        MaximumLife = 1,
        Speed = 0,
    };
    public static readonly UnitStatsSettings ProjectileStats = new UnitStatsSettings()
    {
        Size = 1,
        MaximumLife = 1,
        Speed = 700,
    };

    public static readonly FirearmStatsSettings ShotgunSettings = new FirearmStatsSettings()
    {
        Damage = 1,
        ShootForce = 500,
        ShootsPerSecond = 2,
        MaxShootDeflectionAngle = 1,
        MagazineSize = 25,
        ReloadSpeed = 1,
        SingleShootProjectile = 1,
    };
}