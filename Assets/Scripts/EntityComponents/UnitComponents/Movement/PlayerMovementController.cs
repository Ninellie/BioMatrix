using Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.EntityComponents.UnitComponents.Movement
{
    [RequireComponent(typeof(Player))]
    public class PlayerMovementController : MovementController
    {
        [SerializeField] private float _aimingSpeedMultiplier = 0.5f;

        protected override float Speed
        {
            get
            {
                if (_player.Firearm.MagazineReserve.OnReload) // Кнопка не нажата, оружие на перезарядке
                    return NoAimingSpeed;
                if (_player.IsFireButtonPressed)
                    return AimingSpeed;
                if (_player.Firearm.CanShoot) // Кнопка не нажата, оружие готово к стрельбе
                    return NoAimingSpeed;
                return AimingSpeed; // Кнопка не нажата, оружие не на перезарядке, но совсем недавно стреляло и ещё не готово к стрельбе
            }
        }
        protected float NoAimingSpeed => speedStat.Value * SpeedScale;
        protected float AimingSpeed => NoAimingSpeed * _aimingSpeedMultiplier;

        protected override Vector2 MovementDirection { get; set; }
        protected override Vector2 RawMovementDirection
        {
            get => MovementDirection;
            set => throw new System.NotImplementedException();
        }

        private Player _player;

        private new void Awake()
        {
            _player = GetComponent<Player>();
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