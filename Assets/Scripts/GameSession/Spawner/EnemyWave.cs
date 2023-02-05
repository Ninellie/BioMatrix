using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyWave
{
    private readonly int _maxSize;
    private readonly int _minSize;
    private readonly int _firstWaveMultiplier;
    private readonly int _complicationValue;
    private readonly int _minComlicationDevider;
    private readonly GameTimer _gameTimer;
    public EnemyWave(GameTimer gameTimer) : this(2, 1, 10, 60, 2, gameTimer)
    {
    }
    public EnemyWave(int maxSize, int minSize, int firstWaveMultiplier, int complicationValue, int minComlicationDevider, GameTimer gameTimer)
    {
        _maxSize = maxSize;
        _minSize = minSize;
        _firstWaveMultiplier = firstWaveMultiplier;
        _complicationValue = complicationValue;
        _minComlicationDevider = minComlicationDevider;
        _gameTimer = gameTimer;
    }
    public int GetSize(WaveType waveType)
    {
        return Random.Range(GetMinSize(waveType), GetMaxSize(waveType));
    }
    public int GetMaxSize(WaveType waveType)
    {
        switch (waveType)
        {
            case WaveType.Normal:
                return GetMaxNormalSize();
            case WaveType.First:
                return _maxSize * _firstWaveMultiplier;
            default:
                throw new ArgumentOutOfRangeException(nameof(waveType), waveType, null);
        }
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
        var complicationValue = seconds / (_complicationValue * _minComlicationDevider);
        if (complicationValue >= 1f)
        {
            return (int)(_minSize + complicationValue);
        }
        return _minSize;
    }
}