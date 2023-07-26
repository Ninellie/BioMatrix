using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.Movement
{
    public interface IEnemyController
    {
        public void SetTarget(GameObject target);
    }
}