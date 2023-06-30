using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Entity.Unit.Turret
{
    [RequireComponent(typeof(TurretHubStatsSettings))]
    public class TurretHub : Entity
    {
        [SerializeField] private GameObject _turretPrefab;
        [SerializeField] private GameObject _turretWeaponPrefab;
        [SerializeField] private bool _isSameTurretTarget;

        public TurretHubStatsSettings Settings => GetComponent<TurretHubStatsSettings>();
        public Entity Holder { get; private set; }
        public Weapons.Firearm Firearm { get; set; }
        public Resource Turrets { get; private set; }
        public readonly Stack<Turret> currentTurrets = new();
        public bool IsSameTurretTarget
        {
            get => _isSameTurretTarget;
            set => _isSameTurretTarget = value;
        }

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
            Turrets = new Resource(0);
        }

        protected void BaseStart()
        {
            CreateTurretWeapon(_turretWeaponPrefab);
            UpdateTurrets();
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


        private void BaseFixedUpdate()
        {
            // Turret movement logic
        }

        public void CreateTurretWeapon(GameObject weapon)
        {
            var w = Instantiate(weapon);

            w.transform.SetParent(gameObject.transform);

            w.transform.position = gameObject.transform.position;
            var firearm = w.GetComponent<Weapons.Firearm>();
            firearm.IsEnable = false;
            Firearm = firearm;
        }

        public void SetHolder(Entity entity)
        {
            Holder = entity;
        }

        public void CreateTurret()
        {
            var turretGameObject = Instantiate(_turretPrefab);

            turretGameObject.transform.SetParent(gameObject.transform);

            var createdTurret = turretGameObject.GetComponent<Turret>();

            createdTurret.SetAttractor(gameObject);
            createdTurret.CreateWeapon(_turretWeaponPrefab);
            createdTurret.Firearm.SetStats(Firearm);
            currentTurrets.Push(createdTurret);
        }

        public void DestroyTurret()
        {
            var turret = currentTurrets.Pop();
            turret.Destroy();
        }

        private void UpdateTurrets()
        {
            var dif = Turrets.GetValue() - currentTurrets.Count; 
            var delay = 1;

            if (dif > 0)
            {
                while (dif != 0)
                {
                    Invoke(nameof(CreateTurret), delay);
                    delay++;
                    dif--;
                }
            }

            if (dif < 0)
            {
                while (dif != 0)
                {
                    Invoke(nameof(CreateTurret), delay);
                    delay++;
                    dif++;
                }
            }
        }
    }
}