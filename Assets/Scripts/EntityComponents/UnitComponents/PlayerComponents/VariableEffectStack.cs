using Assets.Scripts.Core.Variables;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents
{
    [ExecuteInEditMode]
    public class VariableEffectStack : MonoBehaviour
    {
        [SerializeField] private IntVariable _variable;
        [SerializeField] private UnityEvent _onEnable;
        [SerializeField] private UnityEvent _onDisable;

        private void OnEnable()
        {
            _variable.ApplyChange(1);
            _onEnable.Invoke();
        }

        private void OnDisable()
        {
            _variable.ApplyChange(-1);
            _onDisable.Invoke();
        }
    }
}