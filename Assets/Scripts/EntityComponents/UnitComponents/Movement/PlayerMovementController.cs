using Core.Variables.References;
using EntityComponents.UnitComponents.PlayerComponents;
using UnityEngine;

namespace EntityComponents.UnitComponents.Movement
{
    public class PlayerMovementController : MovementController
    {
        [SerializeField] private float _aimingSpeedMultiplier = 0.5f;
        [SerializeField] private CastDelayer _reload;
        [SerializeField] private CastDelayer _coolDown;

        protected override float Speed
        {
            get
            {
                var result = 0f;
                
                if (_reload.IsCasting) // Оружие на перезарядке
                    result = NoAimingSpeed;

                if (_coolDown.IsCasting) // Оружие на кулдауне, но не на перезарядке
                    result = AimingSpeed;
                
                if (clampToMinSpeed) // Если скорость меньше минимальной, применяется минимальная
                    result = Mathf.Max(result, minSpeed);
                
                
                return result; // Кнопка не нажата, оружие не на перезарядке, но совсем недавно стреляло и ещё не готово к стрельбе, потому что на кд
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