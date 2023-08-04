using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.Turret
{
    public interface IOrbitRotationController
    {
        void SetObjects(Stack<Turret> objects);
        void SetAttractor(GameObject attractor);
    }
}