using UnityEngine;

public class Enemy : Unit
{
    protected override EntityStatsSettings Settings => GlobalStatsSettingsRepository.EnemyStats;
    protected new void OnEnable()
    {
        base.OnEnable();
    }
    protected new void OnDisable()
    {
        base.OnDisable();
    }
    protected new void Awake()
    {
        SetStats(Settings);
        SetMovement();
        RestoreLifePoints();
    }
    protected new void Update()
    {
        base.Update();
    }
    protected new void FixedUpdate()
    {
        base.FixedUpdate();
    }
    protected override void SetMovement()
    {
        movement = new Movement(gameObject, MovementMode.Pursue, speed.Value);
        movement.SetPursuingTarget(GameObject.FindGameObjectsWithTag("Player")[0]);
    }
}