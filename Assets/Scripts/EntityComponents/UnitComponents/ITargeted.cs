using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents
{
    public interface ITargeted
    {
        public void SetTarget(GameObject target);
    }
}