using UnityEngine;

public class Boon : Unit
{
    private void Awake() => BaseAwake(GlobalStatsSettingsRepository.BoonStats);
    private void OnEnable() => BaseOnEnable();
    private void OnDisable() => BaseOnDisable();
    private void Update() => BaseUpdate();
    private void FixedUpdate() => Movement.FixedUpdateMove();
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
    protected void BaseAwake(UnitStatsSettings settings)
    {
        Debug.Log($"{gameObject.name} Boon Awake");
        var movement = new Movement(gameObject, MovementMode.Idle, settings.Speed);
        base.BaseAwake(settings, movement);
    }
}
