using System.Collections.Generic;
using Assets.Scripts.FirearmComponents;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.Turret
{
    public class TurretHub : Entity
    {
        [SerializeField] private GameObject _turretPrefab;
        [SerializeField] private GameObject _turretWeaponPrefab;
        [SerializeField] private bool _isSameTurretTarget;

        public TurretHubStatsSettings Settings => GetComponent<TurretHubStatsSettings>();
        public Entity Holder { get; private set; }
        public Firearm Firearm { get; set; }
        public OldResource Turrets { get; private set; }
        public readonly Stack<Turret> currentTurrets = new();
        public bool IsSameTurretTarget
        {
            get => _isSameTurretTarget;
            set => _isSameTurretTarget = value;
        }

        private IOrbitRotationController _orbitRotationController;

        private void Awake() => BaseAwake(Settings);

        private void Start() => BaseStart();

        private void OnEnable() => BaseOnEnable();

        private void OnDisable() => BaseOnDisable();

        private void Update() => BaseUpdate();

        protected void BaseAwake(TurretHubStatsSettings settings)
        {
            base.BaseAwake(settings);
            var turretCount = (int)settings.turretCount;
            _orbitRotationController = GetComponent<IOrbitRotationController>();
            Turrets = new OldResource(0);
            Turrets.Increase(turretCount);
        }

        protected void BaseStart()
        {
            UpdateTurrets();
            _orbitRotationController.SetObjects(currentTurrets);
            _orbitRotationController.SetAttractor(gameObject);
        }

        protected override void BaseOnEnable()
        {
            Turrets.ValueChangedEvent += UpdateTurrets;
            KillsCount.IncrementEvent += () => Holder.KillsCount.Increase();
        }

        protected override void BaseOnDisable()
        {
            Turrets.ValueChangedEvent -= UpdateTurrets;
            KillsCount.IncrementEvent -= () => Holder.KillsCount.Increase();
        }

        public void SetHolder(Entity entity)
        {
            Holder = entity;
        }

        public GameObject CreateTurretWeapon()
        {
            var w = Instantiate(_turretWeaponPrefab);
            w.transform.SetParent(gameObject.transform);
            w.transform.position = gameObject.transform.position;
            var firearm = w.GetComponent<Firearm>();
            firearm.IsEnable = false;
            Firearm = firearm;
            return w;
        }

        private void CreateTurret()
        {
            var turretGameObject = Instantiate(_turretPrefab);

            turretGameObject.transform.SetParent(gameObject.transform);

            var createdTurret = turretGameObject.GetComponent<Turret>();

            createdTurret.SetAttractor(gameObject);
            createdTurret.CreateWeapon(_turretWeaponPrefab);

            createdTurret.Firearm.SetStatList(Firearm.GetStatList());
            currentTurrets.Push(createdTurret);
        }

        private void DestroyTurret()
        {
            var turret = currentTurrets.Pop();
            turret.Destroy();
        }

        private void UpdateTurrets()
        {
            var dif = Turrets.GetValue() - currentTurrets.Count;

            if (dif > 0)
            {
                while (dif != 0)
                {
                    CreateTurret();
                    dif--;
                }
            }
            if (dif < 0)
            {
                while (dif != 0)
                {
                    DestroyTurret();
                    dif++;
                }
            }
        }
    }
}