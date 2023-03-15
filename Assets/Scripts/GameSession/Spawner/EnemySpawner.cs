using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Set in Inspector")]
    public GameObject[] enemyPrefabs;
    public float enemyDefaultPadding = 1.5f;
    
    private readonly int _secondsBetweenWaves = 2;
    private GameTimer _gameTimer;
    private readonly System.Random _random = new();
    private readonly Grouping _grouping = new();
    private readonly Rarity _rarity = new();
    private readonly Circle _circle = new Circle();
    private Player _player;
    private EnemyWaveProperties _enemyWaveProperties;

    private const int DefaultComplicationValue = 60;
    private int TimerBonus
    {
        get
        {
            var seconds = _gameTimer.GetTotalSeconds();
            if (seconds < DefaultComplicationValue)
            {
                return 0;
            }
            var remainder = seconds % DefaultComplicationValue;
            return (int)(seconds - remainder) / DefaultComplicationValue;
        }
    }
    private void Awake()
    {
        _gameTimer = FindObjectOfType<GameTimer>();
        _enemyWaveProperties = new EnemyWaveProperties(_gameTimer);
        if (FindObjectOfType<Player>() is null)
        {
            Debug.Log("Player script is null in scene");
        }
        _player = FindObjectOfType<Player>();
    }
    private void Start()
    {
        SpawnFirstWave();
        InvokeRepeating(nameof(SpawnNormalWave), _secondsBetweenWaves, _secondsBetweenWaves);
    }
    private void SpawnWave(WaveType waveType)
    {
        if(_player is null) return;
        if (IsSpawnBlocked()) return;

        var playerPosition = (Vector2)_player.transform.position;
        var waveSize = _enemyWaveProperties.GetSize(waveType);
        var spawn = CreateWave(waveSize);
        var mode = _grouping.GetRandomMode();

        PlaceEnemies(spawn, mode, playerPosition);
        PrepareEnemies(spawn, playerPosition);
    }
    private void SpawnFirstWave()
    {
        SpawnWave(WaveType.First);
    }
    private void SpawnNormalWave()
    {
        SpawnWave(WaveType.Normal);
    }

    private bool IsSpawnBlocked()
    {
        var enemyCount = FindObjectsOfType<Enemy>().Length;
        var maxEnemies = _enemyWaveProperties.GetMaxEnemiesInScene();
        return enemyCount >= maxEnemies;
    }

    public void PlaceEnemies(GameObject[] enemies, GroupingMode mode, Vector2 playerPosition)
    {
        float padding = 0;
        if (mode == GroupingMode.Group)
        {
            padding = enemies.Sum(enemy => enemy.GetComponent<CircleCollider2D>().radius) / 2;
        }

        var positions = _circle.GetPositions(enemies.Length, mode, playerPosition, padding);

        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].transform.position = positions[i];
        }
    }
    private GameObject[] CreateWave(int waveSize)
    {
        var spawn = GetEnemyList(waveSize, enemyPrefabs);
        var spawnedEnemies = new GameObject[waveSize];

        for (var i = 0; i < spawn.Length; i++)
        {
            spawnedEnemies[i] = Instantiate<GameObject>(spawn[i]);
        }
        return spawnedEnemies;
    }
    private void PrepareEnemies(GameObject[] enemies, Vector2 playerPosition)
    {
        foreach (var enemy in enemies)
        {
            var e = enemy.GetComponent<Enemy>();
            PrepareEnemy(e, playerPosition);
        }
    }
    private void PrepareEnemy(Enemy enemy, Vector2 playerPosition)
    {
        var rarity = _rarity.GetRandomRarity();
        var levelUpBonus = TimerBonus;
        enemy.SetRarity(rarity);
        enemy.LevelUp(levelUpBonus);
        enemy.RestoreLifePoints();
        if (enemy.GetEnemyType() == EnemyType.AboveView)
        {
            enemy.LookAt2D(playerPosition);
        }
    }
    private GameObject[] GetEnemyList(int numberOfEnemies, IEnumerable<GameObject> enemyTypes)
    {
        var enemyList = enemyTypes.ToList();

        List<GameObject> selectedEnemies = new();

        for (var i = 0; i < numberOfEnemies; i++)
        {
            var randomEnemyFromList = GetRandomEnemyFromList(enemyList);
            selectedEnemies.Add(randomEnemyFromList);
        }
        return selectedEnemies.ToArray();
    }
    private GameObject GetRandomEnemyFromList(List<GameObject> enemyList)
    {
        var sum = enemyList.Sum(enemy => (int)enemy.GetComponent<Enemy>().Settings.spawnWeight);
        var next = _random.Next(sum);
        var limit = 0;
        foreach (var enemy in enemyList)
        {
            limit += (int)enemy.GetComponent<Enemy>().Settings.spawnWeight;
            if (next < limit)
            {
                return enemy;
            }
        }
        throw new InvalidOperationException("");
    }
}