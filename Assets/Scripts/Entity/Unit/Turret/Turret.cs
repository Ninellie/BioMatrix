using UnityEngine;

public class Turret : Unit
{
    [SerializeField] private Transform _firePoint;
    [SerializeField] private GameObject _attractor;
    private MovementControllerTurret _movementController;
    public Firearm CurrentFirearm { get; private set; }
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
        _attractor = attractor;
    }

    public GameObject GetAttractor()
    {
        return _attractor;
    }
}
