using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.TurretComponents
{
    public interface IOrbitRotationController
    {
        public void SetObjects(Stack<Turret> objects);
        public void SetAttractor(GameObject attractor);
    }
}