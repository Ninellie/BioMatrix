using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TurretHubStatsSettings))]
public class TurretHub : Entity
{
    [SerializeField] private GameObject _turretPrefab;
    [SerializeField] private GameObject _turretWeaponPrefab;
    [SerializeField] private bool _isSameTurretTarget;

    public TurretHubStatsSettings Settings => GetComponent<TurretHubStatsSettings>();
    public Stat TurretCount { get; private set; }
    public readonly Stack<Turret> currentTurrets = new();
    public bool IsSameTurretTarget
    {
        get => _isSameTurretTarget;
        set => _isSameTurretTarget = value;
    }

    public Firearm Firearm { get; set; }

    private void Awake() => BaseAwake(Settings);

    private void Start() => BaseStart();

    private void OnEnable() => BaseOnEnable();

    private void OnDisable() => BaseOnDisable();

    private void Update() => BaseUpdate();

    private void FixedUpdate() => BaseFixedUpdate();

    protected void BaseAwake(TurretHubStatsSettings settings)
    {
        Debug.Log($"{gameObject.name} {nameof(TurretHub)} Awake");
        base.BaseAwake(settings);
        TurretCount = StatFactory.GetStat(settings.turretCount);
    }

    protected void BaseStart()
    {
        CreateTurretWeapon(_turretWeaponPrefab);
        UpdateTurrets();
    }

    protected override void BaseOnEnable()
    {
        if (TurretCount != null) TurretCount.ValueChangedEvent += UpdateTurrets;
    }
    
    protected override void BaseOnDisable()
    {
        if (TurretCount != null) TurretCount.ValueChangedEvent -= UpdateTurrets;
    }

    public void CreateTurretWeapon(GameObject weapon)
    {
        var w = Instantiate(weapon);

        w.transform.SetParent(gameObject.transform);

        w.transform.position = gameObject.transform.position;
        var firearm = w.GetComponent<Firearm>();
        firearm.IsEnable = false;
        Firearm = firearm;
    }

    private void BaseFixedUpdate()
    {
        // Turret movement logic
    }

    private void UpdateTurrets()
    {
        var dif = (int)TurretCount.Value - currentTurrets.Count;
        var isAboveZero = dif > 0;
        float delay = 1;

        while (dif != 0)
        {
            if (isAboveZero)
            {
                Invoke(nameof(CreateTurret), delay);
                dif--;
            }
            else
            {
                Invoke(nameof(DestroyTurret), delay);
                dif++;
            }

            delay++;
        }
    }

    public void CreateTurret()
    {
        var turretGameObject = Instantiate(_turretPrefab);

        turretGameObject.transform.SetParent(gameObject.transform);

        var createdTurret = turretGameObject.GetComponent<Turret>();

        createdTurret.SetAttractor(gameObject);
        createdTurret.CreateWeapon(_turretWeaponPrefab);
        createdTurret.CurrentFirearm.SetStats(Firearm);
        currentTurrets.Push(createdTurret);
    }

    public void DestroyTurret()
    {
        var turret = currentTurrets.Pop();
        turret.Destroy();
    }
}