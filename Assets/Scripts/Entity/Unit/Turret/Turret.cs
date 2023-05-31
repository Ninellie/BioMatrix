using UnityEngine;

public class Turret : Unit
{
    [SerializeField] private Transform _firePoint;
    [SerializeField] private TurretHub _attractor;

    public UnitStatsSettings Settings => GetComponent<UnitStatsSettings>();
    public Firearm CurrentFirearm { get; private set; }

    private MovementControllerTurret _movementController;

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

        // acceleration speed = orbit radius, ONLY FOR TURRETS, for rework
        _movementController = new MovementControllerTurret(this);
    }

    public void CreateWeapon(GameObject weapon)
    {
        var w = Instantiate(weapon);

        w.transform.SetParent(_firePoint);

        w.transform.position = _firePoint.transform.position;
        var firearm = w.GetComponent<Firearm>();
        CurrentFirearm = firearm;
    }

    public void Destroy()
    {
        TakeDamage(MaximumLifePoints.Value);
    }
    
    public void SetAttractor(GameObject attractor)
    {
        _attractor = attractor.GetComponent<TurretHub>();
    }

    public GameObject GetAttractor()
    {
        return _attractor.gameObject;
    }
}
