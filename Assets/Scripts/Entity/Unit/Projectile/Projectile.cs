using UnityEngine;

public class Projectile : Unit
{
    private MovementControllerBullet _movementController;
    private const float SpeedDecrease = 15f;
    private void Awake() => BaseAwake(GlobalStatsSettingsRepository.ProjectileStats);
    private void OnEnable() => BaseOnEnable();
    private void OnDisable() => BaseOnDisable();
    private void Update() => BaseUpdate();
    private void FixedUpdate()
    {
        _movementController.FixedUpdateStep();
        var mod = new StatModifier(OperationType.Addition, SpeedDecrease * -1);
        Speed.AddModifier(mod);
        if (rb2D.velocity != Vector2.zero) return;
        Death();
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

    protected override void BaseAwake(UnitStatsSettings settings)
    {
        Debug.LogWarning("1");
        base.BaseAwake(settings);
        _movementController = new MovementControllerBullet(this);
        _movementController.FixedUpdateStep();
    }
    protected override void BaseUpdate()
    {
        base.BaseUpdate();
        if (IsOnScreen == false) { TakeDamage(CurrentLifePoints); }
    }
    public void Launch(Vector2 direction, float force)
    {
        _movementController.SetDirection(direction);
        var speedMod = new StatModifier(OperationType.Addition, force);
        Speed.AddModifier(speedMod);
    }
}