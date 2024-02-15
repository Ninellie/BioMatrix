using Assets.Scripts.GameSession.Spawner;

namespace Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents
{
    public class EnemyPool : Pool<EnemyData>
    {
        public EnemySpawnData spawnData;
    }
}