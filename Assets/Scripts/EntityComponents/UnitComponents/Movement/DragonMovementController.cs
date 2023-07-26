using Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.Movement
{
    public class DragonMovementController : MovementController
    {
        [SerializeField]
        private GameObject _target;

        protected override Vector2 MovementDirection
        {
            get => RawMovementDirection;
            set => throw new System.NotImplementedException();
        }

        protected override Vector2 RawMovementDirection
        {
            get => (_target.transform.position - transform.position).normalized;
            set => throw new System.NotImplementedException();
        }

        private new void Start()
        {
            _target = FindObjectOfType<Player>().gameObject;
        }
    }
}