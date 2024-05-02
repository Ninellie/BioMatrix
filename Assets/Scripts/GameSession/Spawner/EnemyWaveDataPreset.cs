using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.GameSession.Spawner
{
    [CreateAssetMenu(fileName = "New wave data", menuName = "Spawner/Wave data")]
    public class EnemyWaveDataPreset : ScriptableObject
    {
        [SerializeField] private AnimationCurve _maxWaveSizeCurve;
        [SerializeField] private AnimationCurve _minWaveSizeCurve; // Todo удалить и начать использовать _waveSizeRange


        [SerializeField] private AnimationCurve _maxEnemiesOnScreenCurve; // todo прогрессирующую шкалу? Чем больше врагов тем меньше шанс на появление нового. Подсчитать разницу. Допустим врагов 9, а максимум - 10. Разницца очень малая - единица.

        [SerializeField] private int _fulfillSeconds;
        [SerializeField] private int _intervalMultiplier;
        [SerializeField] private int _firstWaveSizeMultiplier;

            // todo шанс на появление можно умножать на это значение умноженное на коэффициент зависимости. Тогда формула будет иметь вид "(max - current) * k". В итоге, чем больше разница тем больше шанс. А шанс суммируется с кривой волны. 
            // todo А до этого ещё и добавляется Range для большей случайности. Таким образом,  (WaveSize + Range) * (max - current) * k. Пример: (1+(от 0 до 1, пусть 0) + ((10-9) * 0.1). Коэффициент влияния разницы очень малый. В итоге будет лишь 1.1 Но с k около 1, будет уже 2.

        /// <summary>
        /// 
        /// </summary>
        /// <param name="time">Current game session time</param>
        /// <param name="interval">In seconds</param>
        /// <returns>Number of enemies in a spawned wave</returns>
        public int GetAmountForTime(float time)
        {
            var maxAmount = (int)_maxEnemiesOnScreenCurve.Evaluate(time);
            var interval = _fulfillSeconds / maxAmount;
            interval *= _intervalMultiplier;
            return maxAmount;
        }

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