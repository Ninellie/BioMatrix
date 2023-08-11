using Assets.Scripts.FirearmComponents;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.Turret
{
    public class Turret : Unit
    {
        [SerializeField] private Transform _firePoint;
        public UnitStatsSettings Settings => GetComponent<UnitStatsSettings>();
        public Firearm Firearm { get; private set; }

        private TurretHub _hub;

        private void Awake() => BaseAwake(Settings);

        private void OnEnable() => BaseOnEnable();

        protected override void BaseOnEnable()
        {
            base.BaseOnEnable();
            KillsCount.IncrementEvent += () => _hub.KillsCount.Increase();
        }

        private void OnDisable() => BaseOnDisable();

        protected override void BaseOnDisable()
        {
            base.BaseOnDisable();
            KillsCount.IncrementEvent -= () => _hub.KillsCount.Increase();
        }
    
        protected override void BaseAwake(UnitStatsSettings settings)
        {
            Debug.Log($"{gameObject.name} Turret Awake");
            base.BaseAwake(settings);
        }

        public void CreateWeapon(GameObject weapon)
        {
            var w = Instantiate(weapon);

            w.transform.SetParent(_firePoint);

            w.transform.position = _firePoint.transform.position;
            var firearm = w.GetComponent<Firearm>();
            firearm.SetHolder(this);
            Firearm = firearm;
        }

        public void Destroy()
        {
            TakeDamage(MaximumLifePoints.Value);
        }
    
        public void SetAttractor(GameObject attractor)
        {
            _hub = attractor.GetComponent<TurretHub>();
        }
    }
}
