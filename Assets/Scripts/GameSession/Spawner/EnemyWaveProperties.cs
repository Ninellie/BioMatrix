using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.GameSession.Spawner
{
    public class EnemyWaveProperties
    {
        private readonly int _maxSize;
        private readonly int _minSize;
        private readonly int _firstWaveMultiplier;
        private readonly int _complicationValue;
        private readonly int _minComplicationDivider;
        private readonly int _maxEnemiesInScene;
        private readonly int _maxEnemiesInSceneMultiplier;
        public EnemyWaveProperties() : this(6, 3, 3, 60, 2)
        {
        }
        public EnemyWaveProperties(int maxSize, int minSize, int firstWaveMultiplier, int complicationValue, int minComplicationDivider)
        {
            _maxSize = maxSize;
            _minSize = minSize;
            _firstWaveMultiplier = firstWaveMultiplier;
            _complicationValue = complicationValue;
            _minComplicationDivider = minComplicationDivider;
            _maxEnemiesInSceneMultiplier = 1;
            _maxEnemiesInScene = 15;
        }

        public int GetMaxEnemiesInScene()
        {
            var seconds = Time.time;
            var minute = seconds / 60;
            var multiplier = _maxEnemiesInSceneMultiplier * minute;
            if (multiplier < 1) { multiplier = 1; }
            var maxEnemies = _maxEnemiesInScene * multiplier;
        
            return _maxEnemiesInScene;
        }
        public int GetSize(WaveType waveType)
        {
            return Random.Range(GetMinSize(waveType), GetMaxSize(waveType));
        }
        public int GetMaxSize(WaveType waveType)
        {
            return waveType switch
            {
                WaveType.Normal => GetMaxNormalSize(),
                WaveType.First => _maxSize * _firstWaveMultiplier,
                _ => throw new ArgumentOutOfRangeException(nameof(waveType), waveType, null)
            };
        }
        public int GetMinSize(WaveType waveType)
        {
            switch (waveType)
            {
                case WaveType.Normal:
                    return GetMinNormalSize();
                case WaveType.First:
                    return _minSize * _firstWaveMultiplier;
                default:
                    throw new ArgumentOutOfRangeException(nameof(waveType), waveType, null);
            }
        }
        private int GetMaxNormalSize()
        {
            var seconds = Time.time;
            var complicationValue = seconds / _complicationValue;
            if (complicationValue >= 1f)
            {
                return (int)(_maxSize + complicationValue);
            }
            return _maxSize;
        }
        private int GetMinNormalSize()
        {
            var seconds = Time.time;
            var complicationValue = seconds / (_complicationValue * _minComplicationDivider);
            if (complicationValue >= 1f)
            {
                return (int)(_minSize + complicationValue);
            }
            return _minSize;
        }
    }
}