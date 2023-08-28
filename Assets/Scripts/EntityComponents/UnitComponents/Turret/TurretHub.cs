using System.Collections.Generic;
using Assets.Scripts.EntityComponents.Resources;
using Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents;
using Assets.Scripts.EntityComponents.UnitComponents.ProjectileComponents;
using Assets.Scripts.FirearmComponents;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.Turret
{
    public class TurretHub : MonoBehaviour, ISource, IDerivative, ISlayer
    {
        [SerializeField] private GameObject _turretPrefab;
        [SerializeField] private GameObject _turretWeaponPrefab;
        [SerializeField] private bool _isSameTurretTarget;

        public Entity Holder { get; private set; }
        public Firearm Firearm { get; set; }

        public readonly Stack<Turret> currentTurrets = new();

        private IOrbitRotationController _orbitRotationController;
        private ISlayer _source;
        private ResourceList _resources;

        private bool _isSubscribed;

        private void Awake()
        {
            _resources = gameObject.GetComponent<ResourceList>();
            _orbitRotationController = GetComponent<IOrbitRotationController>();
        }

        private void Start()
        {
            Subscribe();
            UpdateTurrets();
            _orbitRotationController.SetObjects(currentTurrets);
            _orbitRotationController.SetAttractor(gameObject);
        }

        private void OnEnable() => Subscribe();

        private void OnDisable() => Unsubscribe();

        public void SetSource(ISource source)
        {
            if (source is ISlayer slayer)
                _source = slayer;
        }

        public void IncreaseKills()
        {
            _resources.GetResource(ResourceName.Kills).Increase();
            _source.IncreaseKills();
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

        private void Subscribe()
        {
            if (_isSubscribed) return;

            var turretsResource = _resources.GetResource(ResourceName.Turrets);
            if (turretsResource is null)
                return;

            turretsResource.AddListenerToEvent(ResourceEventType.ValueChanged).AddListener(UpdateTurrets);

            _isSubscribed = true;
        }

        private void Unsubscribe()
        {
            if (!_isSubscribed) return;

            var turretsResource = _resources.GetResource(ResourceName.Turrets);
            if (turretsResource is null)
                return;

            turretsResource.AddListenerToEvent(ResourceEventType.ValueChanged).RemoveListener(UpdateTurrets);

            _isSubscribed = false;
        }

        private void CreateTurret()
        {
            var turretGameObject = Instantiate(_turretPrefab);
            turretGameObject.transform.SetParent(gameObject.transform);

            var createdTurret = turretGameObject.GetComponent<Turret>();
            createdTurret.SetSource(this);
            createdTurret.CreateWeapon(_turretWeaponPrefab);
            createdTurret.Firearm.SetStatList(Firearm.GetStatList());

            currentTurrets.Push(createdTurret);
        }

        private void DestroyTurret()
        {
            var turret = currentTurrets.Pop();
            turret.Death();
        }

        private void UpdateTurrets()
        {
            var turretsCount = _resources.GetResource(ResourceName.Turrets).GetValue();
            var dif = turretsCount - currentTurrets.Count;

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