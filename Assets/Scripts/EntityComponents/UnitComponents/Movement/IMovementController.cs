using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.Movement
{
    public interface IMovementController
    {
        //Vector2 GetMovementDirection();
        Vector2 GetRawMovementDirection();
        float GetSpeedScale();
        void SetSpeedScale(float value);
        bool IsStopped();
        void AddVelocity(Vector2 velocity);
    }
}