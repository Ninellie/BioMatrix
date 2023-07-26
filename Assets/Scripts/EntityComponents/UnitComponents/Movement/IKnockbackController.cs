using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.Movement
{
    public interface IKnockbackController
    {
        public void Knockback(Vector2 force);
        public void Knockback(GameObject target);
    }
}