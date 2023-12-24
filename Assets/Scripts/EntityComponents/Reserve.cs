using Assets.Scripts.Core.Variables.References;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.EntityComponents
{
    public class Reserve : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private bool _disableObjectOnEmpty;
        [SerializeField] private GameObjectReference _selfGameObject;
        [SerializeField] private bool _refillOnStart;
        [SerializeField] private bool _refillOnEnable;
        [SerializeField] private IntReference _currentValue;
        [SerializeField] private StatReference _maximumValue;
        [SerializeField] private int _edgeValue;
        [Header("Events")]
        [SerializeField] private UnityEvent<int> _onChanged;
        [SerializeField] private UnityEvent _onEmpty;
        [SerializeField] private UnityEvent _onEdge;
        [SerializeField] private UnityEvent _onFill;
        [SerializeField] private UnityEvent<int> _onDecrease;
        [SerializeField] private UnityEvent<int> _onIncrease;

        public int Value => _currentValue;
        public bool IsEmpty => _currentValue == 0;
        public bool IsFull => _currentValue == _maximumValue;

        private void Start()
        {
            if (!_refillOnStart) return;
            Fill();
        }

        private void OnEnable()
        {
            if (!_refillOnEnable) return;
            Fill();
        }

        public void Increase(int amount)
        {
            if (amount <= 0) return;
            var nextValue = _currentValue + amount;
            SetValue(nextValue);
        }

        public void TakeDamage(int amount)
        {
            if (amount <= 0) return;
            var nextValue = _currentValue - amount;
            SetValue(nextValue);
        }

        private void Fill()
        {
            SetValue(_maximumValue);
        }

        public void Empty()
        {
            Debug.Log($"Health is empty. Disable: {_disableObjectOnEmpty}", this);
            SetValue(0);
        }

        private void SetValue(int value)
        {
            var oldValue = _currentValue.Value;
            value = Mathf.Clamp(value, 0, _maximumValue);
            
            if (_currentValue.useConstant)
                _currentValue.constantValue = value;
            else
                _currentValue.variable.SetValue(value);

            var newValue = _currentValue.Value;

            if (oldValue != newValue)
                _onChanged.Invoke(newValue - oldValue);

            if (oldValue > newValue)
                _onDecrease.Invoke(value);

            if (oldValue < newValue)
                _onIncrease.Invoke(value);

            if (newValue == _edgeValue)
                _onEdge.Invoke();

            if (newValue == _maximumValue)
                _onFill.Invoke();

            if (newValue == 0)
            {
                Debug.Log($"Reserve is empty. Disable: {_disableObjectOnEmpty}", this);
                _onEmpty.Invoke();
                if (!_disableObjectOnEmpty) return;
                _selfGameObject.Value.SetActive(false);
            }
        }
    }
}