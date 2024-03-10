using Assets.Scripts.Core.Variables.References;
using Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.Movement
{
    public class PlayerMovementController : MovementController
    {
        [SerializeField] private float _aimingSpeedMultiplier = 0.5f;
        //[SerializeField] private Shooter _firearm;
        [SerializeField] private CastDelayer _reload;
        [SerializeField] private CastDelayer _coolDown;

        protected override float Speed
        {
            get
            {
                if (_reload.IsCasting) // Не важно нажата ли кнопка, ведь оружие на перезарядке
                    return NoAimingSpeed;

                if (_coolDown.IsCasting) // Кнопка нажата, оружие не на перезарядке
                    return AimingSpeed;

                //if (_firearm.CanShoot) // Кнопка не нажата, оружие не на перезарядке и не на кд
                    //return NoAimingSpeed;

                return AimingSpeed; // Кнопка не нажата, оружие не на перезарядке, но совсем недавно стреляло и ещё не готово к стрельбе, потому что на кд
            }
        }
        protected float NoAimingSpeed => speed.Value * SpeedScale;
        protected float AimingSpeed => NoAimingSpeed * _aimingSpeedMultiplier;
        [SerializeField] private Vector2Reference _movementDirection;
        protected override Vector2 MovementDirection
        {
            get => _movementDirection;
            set {}
        }

        protected override Vector2 RawMovementDirection
        {
            get => MovementDirection;
            set => throw new System.NotImplementedException();
        }

        private new void Awake()
        {
            base.Awake();
        }
    }
}