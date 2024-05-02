using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents
{
    public class EnemyPool : Pool<EnemyData>
    {
        [SerializeField] private AnimationCurve _spawnWeightCurve;

        public float GetWeight()
        {
            return _spawnWeightCurve.Evaluate(Time.timeSinceLevelLoad);
        }
    }
}