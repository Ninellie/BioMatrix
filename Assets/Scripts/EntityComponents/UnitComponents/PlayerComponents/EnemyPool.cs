using Assets.Scripts.GameSession.Spawner;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents
{
    public class EnemyPool : Pool<EnemyData>
    {
        //public EnemySpawnData spawnData;
        public EnemySpawnDataListPreset preset;
        
        public float GetWeigth()
        {
            foreach (var data in preset.enemiesSpawnData)
            {
                if(data.enemyPrefab == itemPrefab)
                {
                    return data.spawnWeightCurve.Evaluate(Time.timeSinceLevelLoad);
                }
            }

            return default;
        }
    }
}