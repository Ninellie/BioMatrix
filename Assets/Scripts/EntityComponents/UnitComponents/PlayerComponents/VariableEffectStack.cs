using Assets.Scripts.Core.Variables;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents
{
    [ExecuteInEditMode]
    public class VariableEffectStack : MonoBehaviour
    {
        [SerializeField] private IntVariable _variable;

        private void OnEnable()
        {
            _variable.ApplyChange(1);
        }

        private void OnDisable()
        {
            _variable.ApplyChange(-1);
        }
    }
}