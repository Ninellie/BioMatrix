using Assets.Scripts.Core.Variables;
using Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents;
using Assets.Scripts.FirearmComponents;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.EntityComponents.UnitComponents.Movement
{
    [RequireComponent(typeof(Player))]
    public class PlayerMovementController : MovementController
    {
        [SerializeField] private float _aimingSpeedMultiplier = 0.5f;
        [SerializeField] private PlayerFirearm _firearm;// Convert to bool variable
        [SerializeField] private MagazineReserve _magazine;
        [SerializeField] private Player _player;

        protected override float Speed
        {
            get
            {
                if (_magazine.OnReload) // Не важно нажата ли кнопка, ведь оружие на перезарядке
                    return NoAimingSpeed;
                if (_player.IsFireButtonPressed) // Кнопка нажата, оружие не на перезарядке
                    return AimingSpeed;
                if (_firearm.CanShoot) // Кнопка не нажата, оружие не на перезарядке и не на кд
                    return NoAimingSpeed;
                return AimingSpeed; // Кнопка не нажата, оружие не на перезарядке, но совсем недавно стреляло и ещё не готово к стрельбе, потому что на кд
            }
        }
        protected float NoAimingSpeed => speedStat.Value * SpeedScale;
        protected float AimingSpeed => NoAimingSpeed * _aimingSpeedMultiplier;
        [SerializeField] private Vector2Variable _movementDirection;
        protected override Vector2 MovementDirection { get; set; }
        protected override Vector2 RawMovementDirection
        {
            get => MovementDirection;
            set => throw new System.NotImplementedException();
        }

        private new void Awake()
        {
            base.Awake();
        }

        public void OnMove(InputValue input)
        {
            MovementDirection = input.Get<Vector2>();
        }

        public void OnFire()
        {
        }

        public void OnAimingFire()
        {
        }

        private void RestoreMultiplier()
        {
            _aimingSpeedMultiplier = 1;
        }
    }
}