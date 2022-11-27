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
        Size = 5,
        MaximumLife = 5,
        Speed = 600,
    };

    public static readonly UnitStatsSettings EnemyStats = new UnitStatsSettings()
    {
        Size = 3,
        MaximumLife = 5,
        Speed = 300,
    };

    public static readonly UnitStatsSettings BoonStats = new UnitStatsSettings()
    {
        Size = 2,
        MaximumLife = 1,
        Speed = 0,
    };
    public static readonly UnitStatsSettings ProjectileStats = new UnitStatsSettings()
    {
        Size = 2,
        MaximumLife = 1,
        Speed = 700,
    };

    public static readonly FirearmStatsSettings ShotgunSettings = new FirearmStatsSettings()
    {
        Damage = 1,
        ShootForce = 700,
        ShootsPerSecond = 1,
        MaxShootDeflectionAngle = 15,
        MagazineSize = 2,
        ReloadSpeed = 1,
        SingleShootProjectile = 10,
    };
}