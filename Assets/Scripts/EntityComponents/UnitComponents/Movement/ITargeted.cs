using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.Movement
{
    public interface ITargeted
    {
        public void SetTarget(GameObject target);
    }
}