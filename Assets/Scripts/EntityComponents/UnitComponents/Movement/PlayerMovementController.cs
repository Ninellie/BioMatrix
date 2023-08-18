using Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.EntityComponents.UnitComponents.Movement
{
    [RequireComponent(typeof(Player))]
    public class PlayerMovementController : MovementController
    {
        [SerializeField]
        private float _shootingSpeedMultiplier;
            //= 0.3f;
        protected override float Speed =>
                _isFireButtonPressed && !_player.GetWeapon().GetAmmoResource().IsEmpty
                ? speedOldStat.Value * SpeedScale * _shootingSpeedMultiplier
                : speedOldStat.Value * SpeedScale;


        protected override Vector2 MovementDirection { get; set; }
        protected override Vector2 RawMovementDirection
        {
            get => MovementDirection;
            set => throw new System.NotImplementedException();
        }

        private Player _player;
        private bool _isFireButtonPressed;

        private new void Awake()
        {
            _player = GetComponent<Player>();
            base.Awake();
        }

        public void OnMove(InputValue input)
        {
            var inputVector2 = input.Get<Vector2>();
            MovementDirection = inputVector2;
        }

        public void OnFire()
        {
            _isFireButtonPressed = true;
        }

        public void OnFireOff()
        {
            _isFireButtonPressed = false;
        }
    }
}