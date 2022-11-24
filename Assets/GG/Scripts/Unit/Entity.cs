using System;
using JetBrains.Annotations;
using UnityEngine;

public class Entity : MonoBehaviour
{
    protected virtual EntityStatsSettings Settings => GlobalStatsSettingsRepository.EntityStats;
    protected Stat size;
    protected Stat maximumLifePoints;
    protected float currentLifePoints;

    protected void Awake()
    {
        SetStats(Settings);
    }
    protected void OnEnable()
    {
        size.onValueChanged += ChangeCurrentSize;
    }
    protected void OnDisable()
    {
        size.onValueChanged -= ChangeCurrentSize;
    }
    protected void ChangeCurrentSize()
    {
        gameObject.GetComponent<Transform>().position.Scale(new Vector3(size.Value, size.Value));
    }
    protected virtual void SetStats([NotNull] EntityStatsSettings settings)
    {
        if (settings == null) throw new ArgumentNullException(nameof(settings));

        size = new Stat(settings.Size);
        maximumLifePoints = new Stat(settings.MaximumLife);
    }
}