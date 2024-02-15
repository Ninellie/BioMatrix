using Assets.Scripts.GameSession.Spawner;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents
{
    public class EnemyPool : Pool<EnemyData>
    {
        public EnemySpawnData spawnData;

        public float GetWeigth()
        {
            return spawnData.spawnWeightCurve.Evaluate(Time.timeSinceLevelLoad);
        }
    }
}