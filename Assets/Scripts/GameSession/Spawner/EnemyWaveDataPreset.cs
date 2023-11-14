using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.GameSession.Spawner
{
    [CreateAssetMenu(fileName = "New wave data", menuName = "Spawner/Wave data")]
    public class EnemyWaveDataPreset : ScriptableObject
    {
        [SerializeField] private AnimationCurve _maxWaveSizeCurve;
        [SerializeField] private AnimationCurve _minWaveSizeCurve;
        [SerializeField] private AnimationCurve _maxEnemiesOnScreenCurve;
        [SerializeField] private int _firstWaveSizeMultiplier;

        public int GetMaxEnemiesInScene()
        {
            var maxEnemies = (int)_maxEnemiesOnScreenCurve.Evaluate(Time.timeSinceLevelLoad);
            return maxEnemies;
        }

        public int GetSize(WaveType waveType)
        {
            return Random.Range(GetMinSize(waveType), GetMaxSize(waveType));
        }

        private int GetMaxSize(WaveType waveType)
        {
            var maxSize = GetMaxNormalSize();
            return waveType switch
            {
                WaveType.Normal => maxSize,
                WaveType.First => maxSize * _firstWaveSizeMultiplier,
                _ => throw new ArgumentOutOfRangeException(nameof(waveType), waveType, null)
            };
        }

        private int GetMinSize(WaveType waveType)
        {
            var minSize = GetMinNormalSize();
            return waveType switch
            {
                WaveType.Normal => minSize,
                WaveType.First => minSize * _firstWaveSizeMultiplier,
                _ => throw new ArgumentOutOfRangeException(nameof(waveType), waveType, null)
            };
        }

        private int GetMaxNormalSize()
        {
            return (int)_maxWaveSizeCurve.Evaluate(Time.timeSinceLevelLoad);
        }

        private int GetMinNormalSize()
        {
            return (int)_minWaveSizeCurve.Evaluate(Time.timeSinceLevelLoad);
        }
    }
}