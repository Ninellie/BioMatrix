using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [Header("Set in Inspector")]
    public GameObject[] enemyTypes;
    public float enemyDefaultPadding = 1.5f;
    public GameObject timerGameObject;
    [SerializeField] private int _secondsBetweenWaves = 1;

    private const int Complicator = 30;
    private const int MaxEnemiesInWave = 2;
    private const int MinEnemiesInWave = 1;
    
    private int MaxEnemiesInNormalWave
    {
        get
        {
            var complicator = _gameTimer.GetTotalSeconds() / 30;
            if (complicator >= 1f)
            {
                return (int)(MaxEnemiesInWave + complicator);
            }
            
            return MaxEnemiesInWave;
        }
    }
    private int MinEnemiesInNormalWave
    {
        get
        {
            var complicator = _gameTimer.GetTotalSeconds() / 60;
            if (complicator >= 1f)
            {
                return (int)(MinEnemiesInWave + complicator);
            }

            return MinEnemiesInWave;
        }
    }
    private GameTimer _gameTimer;
    private readonly System.Random _random = new();

    private void Awake()
    {
        _gameTimer = timerGameObject.GetComponent<GameTimer>();
    }
    private void Start()
    {
        SpawnWave();
    }
    public void SpawnWave()
    {
        if(GameObject.FindGameObjectWithTag("Player") == null) return;

        var spawn = CreateSpawn();

        var r = Random.Range(0f, 1f);

        if (r > 0.8)
        {
            PlaceEnemies(spawn, GroupingMode.Default);
        }
        else
        {
            PlaceEnemies(spawn, r > 0.2 ? GroupingMode.Group : GroupingMode.Surround);
        }
        Invoke("SpawnWave", _secondsBetweenWaves);
    }
    private GameObject[] CreateSpawn()
    {
        var enemiesInWave = Random.Range(MinEnemiesInNormalWave, MaxEnemiesInNormalWave);
        var spawn = GetEnemyList(enemiesInWave, enemyTypes);

        var spawnedEnemies = new GameObject[enemiesInWave];

        for (var i = 0; i < spawn.Length; i++)
        {
            spawnedEnemies[i] = Instantiate<GameObject>(spawn[i]);
        }

        foreach (var enemy in spawnedEnemies)
        {
            var e = enemy.GetComponent<Enemy>();
            var r = Random.Range(0f, 1f);

            var rarity = r > 0.99 ? RarityEnum.Rare
                : r > 0.85 ? RarityEnum.Magic
                : RarityEnum.Normal;

            e.SetRarity(rarity);

            ImproveAccordingToTimer(e);
        }
        return spawnedEnemies;
    }
    private void ImproveAccordingToTimer(Enemy enemy)
    {
        enemy.LevelUp(GetTimerBonus(_gameTimer));
        enemy.RestoreLifePoints();
    }
    private int GetTimerBonus(GameTimer gameTimer)
    {
        return (int)gameTimer.GetTotalSeconds() / Complicator;
    }
    private float GetCircleRadiusInscribedAroundTheCamera()
    {
        var camHeight = Camera.main.orthographicSize * 2;
        var camWidth = camHeight * Camera.main.aspect;
        return GetHypotenuseLength(camHeight, camWidth) / 2;
        
    }
    public float GetHypotenuseLength(float sideALength, float sideBLength)
    {
        return Mathf.Sqrt(sideALength * sideALength + sideBLength * sideBLength);
    }
    private float CalculateWavesPerSecond()
    {
        var seconds = _gameTimer.GetTotalSeconds();
        return _secondsBetweenWaves + (seconds / Complicator);
    }
    private GameObject GetRandomEnemyFromList(List<GameObject> enemyList)
    {
        var sum = 0;
        foreach (var enemy in enemyList)
        {
            sum += (int)enemy.GetComponent<Enemy>().spawnWeight.Value;
        }

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
    private void PlaceEnemies(GameObject[] enemies, GroupingMode mode)
    {
        var radius = GetCircleRadiusInscribedAroundTheCamera();
        var playerPoint = (Vector2)FindObjectOfType<Player>().transform.position;
        switch (mode)
        {
            case GroupingMode.Default:
                PlaceDefaultEnemies(enemies, radius, playerPoint);
                break;
            case GroupingMode.Surround:
                PlaceRingEnemies(enemies, radius, playerPoint);
                break;
            case GroupingMode.Group:
                PlaceGroupEnemies(enemies, radius, playerPoint);
                break;
        }
    }
    private void PlaceDefaultEnemies(IEnumerable<GameObject> enemies, float radius, Vector2 playerPoint)
    {
        foreach (var enemy in enemies)
        {
            var randomAng = GetRandomAngle();
            enemy.transform.position = GetPointOnCircle(radius, playerPoint, randomAng);
        }
    }
    private void PlaceRingEnemies(IEnumerable<GameObject> enemies, float radius, Vector2 playerPoint)
    {
        var enemiesArray = enemies as GameObject[] ?? enemies.ToArray();
        var angleStep = (float)Math.PI * 2f / enemiesArray.Length;
        var nextAngle = GetRandomAngle();

        foreach (var enemy in enemiesArray)
        {
            enemy.transform.position = GetPointOnCircle(radius, playerPoint, nextAngle);
            nextAngle += angleStep;
        }
    }
    private void PlaceGroupEnemies(IEnumerable<GameObject> enemies, float radius, Vector2 playerPoint)
    {
        var enemiesArray = enemies as GameObject[] ?? enemies.ToArray();
        var padding = enemiesArray.Sum(enemy => enemy.GetComponent<CircleCollider2D>().radius) / 2;
        var packCentre = GetPointOnCircle(radius + padding, playerPoint, GetRandomAngle());
        foreach (var enemy in enemiesArray)
        {
            var v2 = new Vector2(Random.value, Random.value);
            if (Random.value > 0.5f)
            {
                enemy.transform.position = packCentre - v2;
            }
            else
            {
                enemy.transform.position = packCentre + v2;
            }
        }
    }
    private float GetRandomAngle()
    {
        return Random.Range(0, Mathf.PI * 2);
    }
    private Vector2 GetPointOnCircle(float radius, Vector2 circleCenter, float fi)
    {
        var randomPointOnBaseCircle = new Vector2(Mathf.Cos(fi) * radius, Mathf.Sin(fi) * radius);
        var randomPointOnActualCircle = circleCenter + randomPointOnBaseCircle;
        return randomPointOnActualCircle;
    }
}