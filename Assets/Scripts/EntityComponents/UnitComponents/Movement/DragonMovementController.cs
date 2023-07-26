using Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.Movement
{
    public class DragonMovementController : MovementController
    {
        [SerializeField]
        private GameObject _target;
        protected override float Speed => speedStat.Value * SpeedScale;
        protected override Vector2 MovementDirection
        {
            get => RawMovementDirection;
            set => throw new System.NotImplementedException();
        }

        protected override Vector2 RawMovementDirection
        {
            get
            {
                if (_target == null)
                {
                    return Vector2.zero;
                }
                if (_target.activeInHierarchy)
                {
                    return (_target.transform.position - transform.position).normalized;
                }
                else
                {
                    return Vector2.zero;
                }
            }
            set => throw new System.NotImplementedException();
        }

        private void Start()
        {
            _target = FindObjectOfType<Player>().gameObject;
        }
    }
}