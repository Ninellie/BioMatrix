using Core.Variables;
using Core.Variables.References;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents
{
    public class VariableChanger : MonoBehaviour
    {
        [SerializeField] private IntVariable _variable;
        [SerializeField] private FloatReference _maxVariable;
        [SerializeField] private UnityEvent _onChanged;
        [SerializeField] private UnityEvent _onIncrease;

        public void Increase(int amount)
        {
            if (_variable == null) return;
            var finalValue = _variable.value + amount;
            finalValue = (int)Mathf.Clamp(finalValue, 0, _maxVariable.Value);
            if (finalValue == _variable.value) return;
            _variable.SetValue(finalValue);
            _onChanged.Invoke();
            _onIncrease.Invoke();
        }
    }
}