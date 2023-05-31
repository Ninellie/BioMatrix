using UnityEngine;

public class Projectile : Unit
{
    public UnitStatsSettings Settings => GetComponent<UnitStatsSettings>();

    [SerializeField] public float timeToStop = 15f;

    private MovementControllerBullet _movementController;

    private void Awake() => BaseAwake(Settings);
    
    private void OnEnable() => BaseOnEnable();
    
    private void OnDisable() => BaseOnDisable();
    
    private void Update() => BaseUpdate();
    
    private void FixedUpdate()
    {
        _movementController.FixedUpdateStep();
        if (!_movementController.IsStopped()) return;
        Death();
    }

    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        Collider2D otherCollider2D = collision2D.collider;
        if (!otherCollider2D.gameObject.CompareTag("Enemy")) return;
        if (!otherCollider2D.gameObject.GetComponent<Enemy>().Alive) return;
        TakeDamage(MinimalDamageTaken);
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
        if (IsOnScreen == false)
        {
            TakeDamage(LifePoints.GetValue());
        }
    }
    
    public void Launch(Vector2 direction, float force)
    {
        _movementController.SetDirection(direction);
        var speedMod = new StatModifier(OperationType.Addition, force);
        Speed.AddModifier(speedMod);
    }
}