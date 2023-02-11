using UnityEngine;

public class Projectile : Unit
{
    private const float SpeedDecrease = 15f;
    private void Awake() => BaseAwake(GlobalStatsSettingsRepository.ProjectileStats);
    private void OnEnable() => BaseOnEnable();
    private void OnDisable() => BaseOnDisable();
    private void Update() => BaseUpdate();
    private void FixedUpdate()
    {
        if (_rigidbody2D.velocity == Vector2.zero)
        {
            Death();
            return;
        }

        BaseFixedUpdate();

        var mod = new StatModifier(OperationType.Addition, SpeedDecrease * -1);
        Speed.AddModifier(mod);
    }

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
        if (IsOnScreen == false) { TakeDamage(CurrentLifePoints); }
    }
    public void Launch(Vector2 direction, float force)
    {
        //Movement.SetMovementDirection(direction);
        VelocityController.SetDirection(direction);
        var speedMod = new StatModifier(OperationType.Addition, force);
        Speed.AddModifier(speedMod);
        //Movement.ChangeState(MovementState.Rectilinear);
    }
}