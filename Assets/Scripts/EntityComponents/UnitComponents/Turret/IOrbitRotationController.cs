using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.TurretComponents
{
    public interface IOrbitRotationController
    {
        void SetObjects(Stack<Turret> objects);
        void SetAttractor(GameObject attractor);
    }
}