using Assets.Scripts.Core.Events;
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
        [SerializeField] private GameEvent _onEmpty;
        [SerializeField] private UnityEvent _onEmptyUnityEvent;

        [SerializeField] private GameEvent _onEdge;

        [SerializeField] private GameEvent _onDecrease;
        [SerializeField] private UnityEvent<int> _onDecreaseUnityEvent;

        public int Value => _currentHealth.Value;
        public bool IsEmpty => _currentHealth.Value == 0;
        public bool IsFull => _currentHealth.Value == (int)_maximumHealth.Value;

        private void Start()
        {
            if (!_refillOnStart) return;
            Fill();
        }

        public void TakeDamage(int damage)
        {
            var nextValue = _currentHealth - damage;
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
            if (_disableObjectOnEmpty)
            {
                _selfGameObject.Value.SetActive(false);
            }
        }

        private void SetValue(int value)
        {
            var oldValue = _currentHealth.Value;

            if (value > _maximumHealth)
            {
                value = (int)_maximumHealth;
            }

            if (value < 0)
            {
                value = 0;
            }

            if (_currentHealth.useConstant)
            {
                _currentHealth.constantValue = value;
            }
            else
            {
                _currentHealth.variable.SetValue(value);
            }

            var newValue = _currentHealth.Value;

            if (oldValue > newValue)
            {
                if (_onDecrease != null)
                {
                    _onDecrease.Raise();
                    _onDecreaseUnityEvent.Invoke(value);
                }
            }

            if (newValue == _edgeValue)
            {
                if (_onEdge != null)
                {
                    _onEdge.Raise();
                }
            }

            if (newValue == 0)
            {
                Debug.Log($"Health is empty. Disable: {_disableObjectOnEmpty}", this);

                if (_onEmpty != null)
                { 
                    _onEmpty.Raise(); 
                    _onEmptyUnityEvent.Invoke();
                }

                if (_disableObjectOnEmpty)
                {
                    _selfGameObject.Value.SetActive(false);
                }
            }
        }
    }
}