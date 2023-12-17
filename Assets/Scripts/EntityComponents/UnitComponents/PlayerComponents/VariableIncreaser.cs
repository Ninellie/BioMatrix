using Assets.Scripts.Core.Events;
using Assets.Scripts.Core.Variables;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents
{
    public class VariableIncreaser : MonoBehaviour
    {
        [SerializeField] private IntVariable _variable;
        [SerializeField] private FloatVariable _maxVariable;
        [SerializeField] private GameEvent _onChanged;
        [SerializeField] private GameEvent _onIncrease;

        public void Increase(int amount)
        {
            // Не подходит. Хп должно меняться ТОЛЬКО из магазина, потому что у него есть максимальное значение. Может быть переделать ресурс компонент в scriptable?
            if (_variable == null) return;
            _variable.ApplyChange(amount);
            
            _onChanged.Raise();
            _onIncrease.Raise();
        }
    }
}