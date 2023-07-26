using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.Movement
{
    public interface IProjectileMovementController : IMovementController
    {
        public void SetDirection(Vector2 direction);
    }
}