using System;
using Random = UnityEngine.Random;

public class EnemyWaveProperties
{
    private readonly int _maxSize;
    private readonly int _minSize;
    private readonly int _firstWaveMultiplier;
    private readonly int _complicationValue;
    private readonly int _minComplicationDivider;
    private readonly int _maxEnemiesInScene;
    private readonly int _maxEnemiesInSceneMultiplier;
    private readonly GameTimer _gameTimer;
    public EnemyWaveProperties(GameTimer gameTimer) : this(6, 3, 3, 60, 2, gameTimer)
    {
    }
    public EnemyWaveProperties(int maxSize, int minSize, int firstWaveMultiplier, int complicationValue, int minComplicationDivider, GameTimer gameTimer)
    {
        _maxSize = maxSize;
        _minSize = minSize;
        _firstWaveMultiplier = firstWaveMultiplier;
        _complicationValue = complicationValue;
        _minComplicationDivider = minComplicationDivider;
        _gameTimer = gameTimer;
        _maxEnemiesInSceneMultiplier = 1;
        _maxEnemiesInScene = 150;
    }

    public int GetMaxEnemiesInScene()
    {
        var seconds = _gameTimer.GetTotalSeconds();
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
        var seconds = _gameTimer.GetTotalSeconds();
        var complicationValue = seconds / _complicationValue;
        if (complicationValue >= 1f)
        {
            return (int)(_maxSize + complicationValue);
        }
        return _maxSize;
    }
    private int GetMinNormalSize()
    {
        var seconds = _gameTimer.GetTotalSeconds();
        var complicationValue = seconds / (_complicationValue * _minComplicationDivider);
        if (complicationValue >= 1f)
        {
            return (int)(_minSize + complicationValue);
        }
        return _minSize;
    }
}