using UnityEngine;

namespace Assets.Scripts.GameSession.Spawner
{
    [CreateAssetMenu(fileName = "New spawn data", menuName = "Spawner/Spawn data")]
    public class SpawnDataPreset : ScriptableObject
    {
        [SerializeField, Multiline] private string _description;
        [SerializeField, Min(1), Tooltip("Время полного заполнение врагов с нуля до максимума на данное время")] private int _fulfillSeconds;
        [SerializeField, Min(1), Tooltip("Управляет величиной интервала и количеством врагов в волне")] private int _intervalMultiplier;
        [SerializeField, Tooltip("Коэффициент на который умножится первый спавн")] private int _firstWaveSizeMultiplier;
        [SerializeField] private AnimationCurve _maxEnemiesOnScreen;
        public int FulfillSeconds => _fulfillSeconds;
        public int IntervalMultiplier => _intervalMultiplier;
        public int FirstWaveSizeMultiplier => _firstWaveSizeMultiplier;
        public AnimationCurve MaxEnemiesOnScreen => _maxEnemiesOnScreen;
    }
}