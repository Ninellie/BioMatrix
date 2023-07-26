using Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.Movement
{
    public class MiteMovementController : MovementController
    {
        [SerializeField]
        private FrontSide _viewDirection;

        [SerializeField]
        private GameObject _target;

        protected override Vector2 MovementDirection
        {
            get
            {
                return _viewDirection switch
                {
                    FrontSide.Up => gameObject.transform.TransformVector(0, 1, 0),
                    FrontSide.Down => gameObject.transform.TransformVector(0, -1, 0),
                    FrontSide.Right => gameObject.transform.TransformVector(1, 0, 0),
                    FrontSide.Left => gameObject.transform.TransformVector(-1, 0, 0),
                    _ => Vector2.zero
                };
            }
            set => throw new System.NotImplementedException();
        }

        protected override Vector2 RawMovementDirection
        {
            get => (_target.transform.position - transform.position).normalized;
            set => throw new System.NotImplementedException();
        }

        private new void Start()
        {
            base.Start();
            _target = FindObjectOfType<Player>().gameObject;
        }
    }
}