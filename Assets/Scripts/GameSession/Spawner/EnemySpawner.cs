using System;
using System.Collections;
using Assets.Scripts.Core.Variables.References;
using Assets.Scripts.EntityComponents.UnitComponents.EnemyComponents;
using Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents;
using UnityEngine;

namespace Assets.Scripts.GameSession.Spawner
{
    /// <summary>
    /// Эта система спавна управляется слабо, но креативно.
    /// Вы можете задать кривую максимума врагов на сцене а также сколько требуется секунд для заполнения от нуля до этого максимума.
    /// Система сама подстроится и будет спавнить врагов с нужной скоростью для каждой определённой секунды.
    /// </summary>
    public class EnemySpawner : MonoBehaviour
    {
        //private float EnemiesPerSecond => _maxEnemiesOnScreen.Evaluate(Time.timeSinceLevelLoad) / _fulfillSeconds;
        //private float WaveInterval => _fulfillSeconds / _maxEnemiesOnScreen.Evaluate(Time.timeSinceLevelLoad) * _intervalMultiplier;
        [SerializeField] private Vector2Reference _playerPosition;
        [SerializeField] private EnemyPool _enemyPool;
        [Header("Spawn pattern settings")]
        [SerializeField] private SpawnDataPreset _spawnData;
        [SerializeField] private float _spawnInterval;
        [Header("Readonly Indicators")]
        [SerializeField] private int _maxEnemies;
        [SerializeField] private float _waveIntervalPerEnemyInSeconds;
        [SerializeField] private float _spawnQueueSize;

        private readonly Circle _circle = new();

        #region UnityMessages

        private void Awake()
        {
            _enemyPool = GetComponentInChildren<EnemyPool>();
        }

        private void Start()
        {
            if (!IsSpawnBlocked())
            {
                _spawnQueueSize += _maxEnemies
                                   / _spawnData.FulfillSeconds
                                   * _spawnData.IntervalMultiplier
                                   * _spawnData.FirstWaveSizeMultiplier;
            }
        }

        private void OnEnable()
        {
            Invoke(nameof(StartWork), 0.2f);
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }

        #endregion

        private void StartWork()
        {
            StartCoroutine(EnqueueToSpawn());
            StartCoroutine(Co_Spawn());
        }

        private bool IsSpawnBlocked()
        {
            _maxEnemies = (int)_spawnData.MaxEnemiesOnScreen.Evaluate(Time.timeSinceLevelLoad);
            return !(_enemyPool.pool.CountActive < _maxEnemies);
        }

        private IEnumerator EnqueueToSpawn() // add enemies to queue
        {
            if (_playerPosition is null)
            {
                throw new NullReferenceException($"playerPosition is null in {gameObject}");
            }
            while (true)
            {
                yield return new WaitWhile(IsSpawnBlocked);
                yield return new WaitForSeconds(_spawnData.IntervalMultiplier);
                _spawnQueueSize += _maxEnemies / (float)_spawnData.FulfillSeconds * _spawnData.IntervalMultiplier;
            }
        }

        // Эта корутина является лишь оптимизацией спавна. Она медленно спавнит врага минимально возможными группами.
        private IEnumerator Co_Spawn()
        {
            while (true)
            {
                yield return new WaitUntil(() => _spawnQueueSize >= 1);

                var spawnsCount = (int)(_waveIntervalPerEnemyInSeconds * _spawnQueueSize);
                spawnsCount = Mathf.Clamp(spawnsCount, 1, _maxEnemies);
                
                for (int i = 0; i < spawnsCount; i++)
                { 
                    Spawn();
                }

                if (_maxEnemies >= 1)
                {
                    _waveIntervalPerEnemyInSeconds = (float)_spawnData.FulfillSeconds / _maxEnemies;
                    yield return new WaitForSeconds(_waveIntervalPerEnemyInSeconds);
                }
                else
                {
                    yield return new WaitForSeconds(_spawnInterval);
                }
            }
        }

        private void Spawn()
        {
            var enemy = _enemyPool.Get();
            var radius = _circle.GetRadiusInscribedAroundTheCamera();
            radius += enemy.GetComponent<CircleCollider2D>().radius;
            var angle = _circle.GetRandomAngle();
            var enemyPos = _circle.GetPointOn(radius, _playerPosition, angle);
            enemy.transform.position = enemyPos;
            _spawnQueueSize--;
            if (enemy.spriteType.EnemyType != EnemyType.AboveView) return;
            RotateRigidbody2DToTarget(_playerPosition, enemy.rigidbody2D);
        }

        private void RotateRigidbody2DToTarget(Vector2 targetPosition, Rigidbody2D rb2D)
        {
            var direction = rb2D.position - targetPosition;
            var angle = (Mathf.Atan2(direction.y, direction.x) + Mathf.PI / 2) * Mathf.Rad2Deg;
            rb2D.rotation = angle;
            rb2D.SetRotation(angle);
        }
    }
}