using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.Movement
{
    public interface IMovementController
    {
        public Vector2 GetMovementDirection();
        public Vector2 GetRawMovementDirection();
        public float GetSpeedScale();
        public bool IsStopped();
        public void AddVelocity(Vector2 velocity);
    }
}