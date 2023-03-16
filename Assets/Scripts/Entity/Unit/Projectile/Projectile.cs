using UnityEngine;

public class Projectile : Unit
{
    [SerializeField] public float timeToStop = 15f;
    public UnitStatsSettings Settings
    {
        get => GetComponent<UnitStatsSettings>();
        set => throw new System.NotImplementedException();
    }

    private MovementControllerBullet _movementController;
    private void Awake() => BaseAwake(Settings);
    private void OnEnable() => BaseOnEnable();
    private void OnDisable() => BaseOnDisable();
    private void Update() => BaseUpdate();
    private void FixedUpdate()
    {
        _movementController.FixedUpdateStep();
        //var mod = new StatModifier(OperationType.Addition, _speedDecrease * -1);
        //Speed.AddModifier(mod);
        if (Rb2D.velocity != Vector2.zero) return;
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