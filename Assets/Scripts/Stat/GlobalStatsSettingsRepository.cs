public class GlobalStatsSettingsRepository
{
    public static readonly EntityStatsSettings EntityStats = new EntityStatsSettings
    {
        size = 1,
        maximumLife = 1,
        lifeRegenerationInSecond = 0,
        knockbackPower = 0,
    };

    public static readonly EnclosureStatsSettings EnclosureStats = new EnclosureStatsSettings()
    {
        size = 1,
        maximumLife = 1000,
        lifeRegenerationInSecond = 0,
        knockbackPower = 1100,
        constrictionRate = 0.00035f,
    };

    public static readonly UnitStatsSettings UnitStats = new UnitStatsSettings()
    {
        size = 1,
        maximumLife = 1,
        lifeRegenerationInSecond = 0,
        speed = 0,
        knockbackPower = 0,
    };

    public static readonly PlayerStatsSettings PlayerStats = new PlayerStatsSettings()
    {
        size = 1,
        maximumLife = 30000,
        lifeRegenerationInSecond = 0,
        speed = 220,
        magnetismRadius = 150,
        magnetismPower = 1000,
        knockbackPower = 200,
    };

    public static readonly EnemyStatsSettings DragonStats = new EnemyStatsSettings()
    {
        size = 0.8f,
        maximumLife = 1,
        lifeRegenerationInSecond = 0,
        speed = 100,
        spawnWeight = 1000,
        knockbackPower = 700,
        accelerationSpeed = 200,
        rotationSpeed = 0,
    };

    public static readonly EnemyStatsSettings MiteStats = new EnemyStatsSettings()
    {
        size = 0.3f,
        maximumLife = 1,
        lifeRegenerationInSecond = 0,
        speed = 300,
        spawnWeight = 100,
        knockbackPower = 300,
        accelerationSpeed = 2000,
        rotationSpeed = 2,
    };

    public static readonly UnitStatsSettings BoonStats = new UnitStatsSettings()
    {
        size = 1,
        maximumLife = 1,
        lifeRegenerationInSecond = 0,
        speed = 0,
        knockbackPower = 0,
    };
    public static readonly UnitStatsSettings ProjectileStats = new UnitStatsSettings()
    {
        size = 1,
        maximumLife = 1,
        lifeRegenerationInSecond = 0,
        speed = 700,
        knockbackPower = 100,
    };

    public static readonly FirearmStatsSettings ShotgunStats = new FirearmStatsSettings()
    {
        damage = 1,
        shootForce = 500,
        shootsPerSecond = 3,
        maxShootDeflectionAngle = 1,
        magazineSize = 6,
        reloadSpeed = 1,
        singleShootProjectile = 1,
    };
}