using System.Collections.Generic;
using Assets.Scripts.EntityComponents.Resources;
using Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents;
using Assets.Scripts.EntityComponents.UnitComponents.ProjectileComponents;
using Assets.Scripts.FirearmComponents;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.Turret
{
    public class TurretHub : MonoBehaviour
    {
        [SerializeField] private GameObject _turretPrefab;
        [SerializeField] private GameObject _turretWeaponPrefab;

        //private Firearm _firearm;
        private readonly Stack<Turret> _currentTurrets = new();
        private IOrbitRotationController _orbitRotationController;
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
            _orbitRotationController.SetObjects(_currentTurrets);
            _orbitRotationController.SetAttractor(gameObject);
        }

        private void OnEnable() => Subscribe();

        private void OnDisable() => Unsubscribe();

        public GameObject CreateTurretWeapon()
        {
            var w = Instantiate(_turretWeaponPrefab);
            w.transform.SetParent(gameObject.transform);
            w.transform.position = gameObject.transform.position;
            //var firearm = w.GetComponent<Firearm>();
            //firearm.IsEnable = false;
            //_firearm = firearm;
            return w;
        }

        private void Subscribe()
        {
            if (_isSubscribed) return;

            var turretsResource = _resources.GetResource(ResourceName.Turrets);
            if (turretsResource is null) return;

            _resources.GetResource(ResourceName.Turrets).AddListenerToEvent(ResourceEventType.ValueChanged, UpdateTurrets);

            _isSubscribed = true;
        }

        private void Unsubscribe()
        {
            if (!_isSubscribed) return;

            var turretsResource = _resources.GetResource(ResourceName.Turrets);
            if (turretsResource is null) return;

            _resources.GetResource(ResourceName.Turrets).RemoveListenerToEvent(ResourceEventType.ValueChanged, UpdateTurrets);

            _isSubscribed = false;
        }

        private void CreateTurret()
        {
            var turretGameObject = Instantiate(_turretPrefab, transform.position, new Quaternion(), transform);
            //turretGameObject.transform.SetParent(gameObject.transform);
            var createdTurret = turretGameObject.GetComponent<Turret>();
            createdTurret.CreateWeapon(_turretWeaponPrefab);
            //createdTurret.Firearm.SetStatList(_firearm.GetStatList());
            _currentTurrets.Push(createdTurret);
        }

        private void DestroyTurret()
        {
            var turret = _currentTurrets.Pop();
            turret.Death();
        }

        private void UpdateTurrets()
        {
            var turretsCount = _resources.GetResource(ResourceName.Turrets).GetValue();
            var dif = turretsCount - _currentTurrets.Count;

            if (dif > 0)
            {
                while (dif != 0)
                {
                    CreateTurret(); //todo заменить на enable turret
                    dif--;
                }
            }
            if (dif < 0)
            {
                while (dif != 0)
                {
                    DestroyTurret(); //todo заменить на disable turret
                    dif++;
                }
            }
        }
    }
}