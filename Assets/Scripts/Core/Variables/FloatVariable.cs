using Assets.Scripts.SourceStatSystem;
using System.Collections.Generic;
using UnityEngine;


public class ObjectPool : ScriptableObject
{
    private Dictionary<GameObject, StatSourseHolder> _enemies;

    private Dictionary<EnemyId, StatSourcePack> _baseEnemyStats;

    public void AddObject(GameObject gameObject, EnemyId enemyId)
    {

        var enemy = Instantiate(gameObject);
        // Добавляем объект в пул

        var stats = _baseEnemyStats.GetValueOrDefault(EnemyId);
        if (stats == null)
            return;

        CreateInstance(typeof(StatSourcePack));

        s. stats.

        _enemies.Add(enemy, );
        // Добавляем ему статов
        gameObject.AddComponent<Stat>();

    }

    public void RemoveObject()
    {

    }
}

public class StatSourseHolder : ScriptableObject
{
    public StatSourcePack baseStatSources;


    [SerializeField] private List<StatSourceData> _statSources = new();
    [SerializeField] private StatSourcePack _baseStatSources;

    public List<StatSourceData> GetStatSources()
    {
        var statSourceDataList = new List<StatSourceData>();
        statSourceDataList.AddRange(_statSources);
        if (_baseStatSources == null)
        {
            Debug.LogWarning($"Base Stat Sources is null");
            return statSourceDataList;
        }
        statSourceDataList.AddRange(_baseStatSources.StatSources);
        return statSourceDataList;
    }

    public void AddStatSource(StatSourceData statSourceData)
    {
        _statSources.Add(statSourceData);
    }

    public void RemoveStatSource(StatSourceData statSourceData)
    {
        _statSources.Remove(statSourceData);
    }

    public StatSourceData[] statSources;

    // Когда оно изменяется, оно ищет подходящий стат и изменяет его

    public void AddSource(StatSourceData source)
    {

    }
}

[CreateAssetMenu]
public class StatVariable : FloatVariable
{
    public StatId statId;
    public GameEvent onChanged;


}

public class Stat : ScriptableObject
{
#if UNITY_EDITOR
    [Multiline]
    public string developerDescription = "";
#endif
    public float value;
    public GameEvent onChangedGameEvent;

    public void SetValue(float value)
    {
        var oldValue = this.value;
        this.value = value;
        TryRaiseEvent(oldValue);
    }

    public void SetValue(FloatVariable value)
    {
        var oldValue = this.value;
        this.value = value.value;
        TryRaiseEvent(oldValue);
    }

    public void ApplyChange(float amount)
    {
        var oldValue = value;
        value += amount;
        TryRaiseEvent(oldValue);
    }

    public void ApplyChange(FloatVariable amount)
    {
        var oldValue = value;
        value += amount.value;
        TryRaiseEvent(oldValue);
    }

    private void TryRaiseEvent(float oldValue)
    {
        if (!oldValue.Equals(value))
        {
            onChangedGameEvent.Raise();
        }
    }
}


[CreateAssetMenu]
public class FloatVariable : ScriptableObject
{
#if UNITY_EDITOR
    [Multiline]
    public string developerDescription = "";
#endif
    public float value;

    public void SetValue(float value)
    {
        this.value = value;
    }

    public void SetValue(FloatVariable value)
    {
        this.value = value.value;
    }

    public void ApplyChange(float amount)
    {
        value += amount;
    }

    public void ApplyChange(FloatVariable amount)
    {
        value += amount.value;
    }
}