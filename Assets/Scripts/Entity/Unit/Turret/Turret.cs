using UnityEngine;

public class Turret : Unit
{
    [SerializeField] private Transform _firePoint;
    [SerializeField] private GameObject _attractor;
    private MovementControllerTurret _movementController;
    public UnitStatsSettings Settings => GetComponent<UnitStatsSettings>();

    private void Awake() => BaseAwake(Settings);
    private void FixedUpdate() => BaseFixedUpdate();
    protected void BaseFixedUpdate()
    {
        if (_attractor == null) return;
        _movementController?.OrbitalFixedUpdateStep();
    }
    protected override void BaseAwake(UnitStatsSettings settings)
    {
        Debug.Log($"{gameObject.name} Turret Awake");

        base.BaseAwake(settings);
        // acceleration speed = orbit radius
        _movementController = new MovementControllerTurret(this, _attractor);
    }

    public void Destroy()
    {
        TakeDamage(MaximumLifePoints.Value);
    }
    public void SetAttractor(GameObject attractor)
    {
        _attractor = attractor;
    }
}
