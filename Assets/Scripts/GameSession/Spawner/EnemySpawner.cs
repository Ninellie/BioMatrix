using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Set in Inspector")]
    public GameObject[] enemyTypes;
    public float enemyDefaultPadding = 1.5f;
    public GameObject timerGameObject;

    private readonly int _secondsBetweenWaves = 1;
    private GameTimer _gameTimer;
    private readonly System.Random _random = new();
    private readonly EnemyPlacer _enemyPlacer = new();
    private readonly Grouping _grouping = new();
    private readonly Rarity _rarity = new();
    private Player _player;
    private EnemyWave _enemyWave;

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
        _gameTimer = timerGameObject.GetComponent<GameTimer>();
        _enemyWave = new EnemyWave(_gameTimer);
        if (FindObjectOfType<Player>() == null)
        {
            Debug.Log("Player script is null in scene");
        }
        _player = FindObjectOfType<Player>();
    }
    private void Start()
    {
        SpawnFirstWave();
        InvokeRepeating("SpawnWave", _secondsBetweenWaves, _secondsBetweenWaves);
    }
    public void SpawnWave()
    {
        if(_player == null) return;
        var playerPosition = (Vector2)_player.transform.position;
        var spawn = CreateSpawn(_enemyWave.GetSize(WaveType.Normal), playerPosition);
        var mode = _grouping.GetRandomMode();
        _enemyPlacer.PlaceEnemies(spawn, mode, playerPosition);
        PrepareEnemies(spawn, playerPosition);
    }
    public void SpawnFirstWave()
    {
        if (_player == null) return;
        var playerPosition = (Vector2)_player.transform.position;
        var spawn = CreateSpawn(_enemyWave.GetSize(WaveType.First), playerPosition);
        var mode = _grouping.GetRandomMode();
        _enemyPlacer.PlaceEnemies(spawn, mode, playerPosition);
        PrepareEnemies(spawn, playerPosition);
    }
    private GameObject[] CreateSpawn(int enemiesInSpawn, Vector2 playerPosition)
    {
        var spawn = GetEnemyList(enemiesInSpawn, enemyTypes);
        var spawnedEnemies = new GameObject[enemiesInSpawn];

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
            PrepareEnemyToSpawn(e, playerPosition);
        }
    }
    private void PrepareEnemyToSpawn(Enemy enemy, Vector2 playerPosition)
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
    private GameObject[] GetEnemyList(int numberOfEnemies, IEnumerable<GameObject> _enemyTypes)
    {
        var enemyList = _enemyTypes.ToList();

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
        var sum = enemyList.Sum(enemy => (int)enemy.GetComponent<Enemy>().spawnWeight.Value);
        var next = _random.Next(sum);
        var limit = 0;
        foreach (var enemy in enemyList)
        {
            limit += (int)enemy.GetComponent<Enemy>().spawnWeight.Value;
            if (next < limit)
            {
                return enemy;
            }
        }
        throw new InvalidOperationException("");
    }
}