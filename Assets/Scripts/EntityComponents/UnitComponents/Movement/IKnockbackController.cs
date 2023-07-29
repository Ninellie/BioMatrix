using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.Movement
{
    public interface IKnockbackController
    {
        void Knockback(Vector2 force);
        void Knockback(GameObject target);
    }
}