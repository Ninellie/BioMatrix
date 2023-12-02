using Assets.Scripts.EntityComponents.Resources;
using Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents;
using Assets.Scripts.EntityComponents.UnitComponents.ProjectileComponents;
using Assets.Scripts.FirearmComponents;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.Turret
{
    public class Turret : MonoBehaviour
    {
        [SerializeField] private Transform _firePoint;
        //public Firearm Firearm { get; private set; }

        private ResourceList _resources;

        private void Awake()
        {
            Debug.Log($"{gameObject.name} Turret Awake");
            _resources = GetComponent<ResourceList>();

        }

        private void FixedUpdate()
        {
            //Firearm.DoAction();
        }

        public void CreateWeapon(GameObject weapon)
        {
            var w = Instantiate(weapon);
            w.transform.SetParent(_firePoint);
            w.transform.position = _firePoint.transform.position;
            //var firearm = w.GetComponent<Firearm>();
            //firearm.SetSource(this);
            //Firearm = firearm;
        }

        public void Death()
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}