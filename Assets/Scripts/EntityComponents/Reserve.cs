using System;
using System.Collections.Generic;
using System.Linq;
using Core.Variables.References;
using UnityEngine;
using UnityEngine.Events;

namespace EntityComponents
{
    public enum ReserveAction
    {
        None,
        Empty,
        Fill,
        UseInitialValue
    } 
    
    [Serializable]
    public class ReserveMark
    {
        [SerializeField] private IntReference value;
        [SerializeField] private UnityEvent onReaching;
        
        public int Value => value;
        public UnityEvent OnReaching => onReaching;
    }
    
    public class Reserve : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private bool _disableObjectOnEmpty;
        [SerializeField] private GameObjectReference _selfGameObject;
        [Space]
        [SerializeField] private ReserveAction _startAction;
        [SerializeField] private ReserveAction _enableAction;
        [Space]
        [SerializeField] private IntReference _currentValue;
        [SerializeField] private StatReference _maximumValue;
        [SerializeField] private IntReference _initialValue;
        [SerializeField] private int _edgeValue;
        [Space]
        [SerializeField] private FloatReference _gainMultiplier;
        [SerializeField] private bool _useMultiplier;
        [Header("Events")]
        [SerializeField] private UnityEvent<int> _onChanged;
        [SerializeField] private UnityEvent<int> _onEmpty;
        [SerializeField] private UnityEvent _onEdge;
        [SerializeField] private UnityEvent _onFill;
        [SerializeField] private UnityEvent<int> _onDecrease;
        [SerializeField] private UnityEvent<int> _onIncrease;
        [SerializeField] private List<ReserveMark> marks;

        public int Value => _currentValue;
        public bool IsEmpty => _currentValue == 0;
        public bool IsFull => _currentValue == _maximumValue;

        private void Start()
        {
            DoAction(_startAction);
        }

        private void OnEnable()
        {
            DoAction(_enableAction);
        }

        private void DoAction(ReserveAction action)
        {
            switch (action)
            {
                case ReserveAction.None:
                    break;
                case ReserveAction.Empty:
                    Empty();
                    break;
                case ReserveAction.Fill:
                    Fill();
                    break;
                case ReserveAction.UseInitialValue:
                    SetValue(_initialValue);
                    break;
            }
        }

        public void Increase(int amount)
        {
            if (IsFull)
            {
                Debug.Log($"Cannot increase, reserve is already full");
                return;
            }
            if (amount <= 0) return;
            if (_useMultiplier)
            {
                var multiplier = _gainMultiplier.Value;
                float baseValue = amount;
                amount = (int)(baseValue * multiplier);
            }
            var nextValue = _currentValue + amount;
            SetValue(nextValue);
        }

        public void TakeDamage(int amount)
        {
            if (IsEmpty)
            {
                Debug.Log($"Cannot take more damage, reserve is already empty");
                return;
            }
            if (amount <= 0) return;
            var nextValue = _currentValue - amount;
            SetValue(nextValue);
        }

        public void Fill()
        {
            if (IsFull)
            {
                Debug.Log($"Reserve is already full");
                return;
            }
            SetValue(_maximumValue);
        }

        public void Empty()
        {
            if (IsEmpty) 
            {
                Debug.Log($"Reserve is already empty");
                return;
            }
            Debug.Log($"Reserve is empty. Disable: {_disableObjectOnEmpty}");
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
                _onEmpty.Invoke(oldValue);
                if (!_disableObjectOnEmpty) return;
                _selfGameObject.Value.SetActive(false);
            }

            foreach (var mark in marks.Where(mark => newValue == mark.Value))
            {
                mark.OnReaching.Invoke();
            }
        }
    }
}