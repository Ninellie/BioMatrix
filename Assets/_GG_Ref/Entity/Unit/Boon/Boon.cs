using UnityEngine;

public class Boon : Unit
{
    protected override EntityStatsSettings Settings => GlobalStatsSettingsRepository.UnitStats;
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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("The Boon was taken");
        var collisionGameObject = collision.gameObject;
        switch (collisionGameObject.tag)
        {
            case "Player":
                TakeDamage(CurrentLifePoints);
                break;
        }
    }
    protected override void SetMovement()
    {
        movement = new Movement(gameObject, MovementMode.Idle, speed.Value);
    }
}
