using UnityEngine;

public class Projectile : Unit
{
    private void Awake() => BaseAwake(GlobalStatsSettingsRepository.ProjectileStats);
    private void OnEnable() => BaseOnEnable();
    private void OnDisable() => BaseOnDisable();
    private void Update() => BaseUpdate();
    private void FixedUpdate() => BaseFixedUpdate();
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var collisionGameObject = collision.gameObject;
        switch (collisionGameObject.tag)
        {
            case "Enemy":
                if (collisionGameObject.GetComponent<Entity>().Alive)
                {
                    TakeDamage(MinimalDamageTaken);
                }
                break;
        }
    }
    protected override void BaseUpdate()
    {
        base.BaseUpdate();
        if (IsOnScreen == false)
        {
            TakeDamage(CurrentLifePoints);
        }
    }
    public void Launch(Vector2 direction, float force)
    {
        Movement.SetMovementDirection(direction);
        var speedMod = new StatModifier(OperationType.Addition, force);
        Speed.AddModifier(speedMod);
        Movement.ChangeState(MovementState.Rectilinear);
    }
}