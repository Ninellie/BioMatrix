using Assets.Scripts.Core.Variables.References;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.EntityComponents
{
    public class HealthReserve : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private bool _disableObjectOnEmpty;
        [SerializeField] private GameObjectReference _selfGameObject;
        [SerializeField] private bool _refillOnStart;
        [SerializeField] private IntReference _currentHealth;
        [SerializeField] private StatReference _maximumHealth;
        [SerializeField] private int _edgeValue;
        [Header("Events")]
        [SerializeField] private UnityEvent<int> _onChanged;
        [SerializeField] private UnityEvent _onEmpty;
        [SerializeField] private UnityEvent _onEdge;
        [SerializeField] private UnityEvent<int> _onDecrease;
        [SerializeField] private UnityEvent<int> _onIncrease;

        public int Value => _currentHealth;
        public bool IsEmpty => _currentHealth == 0;
        public bool IsFull => _currentHealth == _maximumHealth;

        private void Start()
        {
            if (!_refillOnStart) return;
            Fill();
        }

        public void Increase(int amount)
        {
            if (amount <= 0) return;
            var nextValue = _currentHealth + amount;
            SetValue(nextValue);
        }

        public void TakeDamage(int amount)
        {
            if (amount <= 0) return;
            var nextValue = _currentHealth - amount;
            SetValue(nextValue);
        }

        private void Fill()
        {
            SetValue((int)_maximumHealth);
        }

        public void Empty()
        {
            Debug.Log($"Health is empty. Disable: {_disableObjectOnEmpty}", this);
            SetValue(0);
        }

        private void SetValue(int value)
        {
            var oldValue = _currentHealth.Value;

            if (value > _maximumHealth)
                value = (int)_maximumHealth;

            if (value < 0)
                value = 0;

            if (_currentHealth.useConstant)
                _currentHealth.constantValue = value;
            else
                _currentHealth.variable.SetValue(value);

            var newValue = _currentHealth.Value;

            if (oldValue != newValue)
                _onChanged.Invoke(newValue - oldValue);

            if (oldValue > newValue)
                _onDecrease.Invoke(value);

            if (oldValue < newValue)
                _onIncrease.Invoke(value);

            if (newValue == _edgeValue)
                _onEdge.Invoke();

            if (newValue == 0)
            {
                Debug.Log($"Health is empty. Disable: {_disableObjectOnEmpty}", this);
                _onEmpty.Invoke();
                if (!_disableObjectOnEmpty) return;
                _selfGameObject.Value.SetActive(false);
            }
        }
    }
}