using Assets.Scripts.Core.Sets;
using Assets.Scripts.Core.Variables;
using Assets.Scripts.Core.Variables.References;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents
{
    public class TurretShootDelaySetter : MonoBehaviour
    {
        public FloatReference turretAttackSpeed;
        public TransformRuntimeSet turrets;
        public FloatVariable timeDelay;

        private void FixedUpdate()
        {
            timeDelay.SetValue(turretAttackSpeed / 1f / turrets.items.Count);
        }
    }
}