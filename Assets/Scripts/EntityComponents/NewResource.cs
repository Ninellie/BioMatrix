using System;
using Assets.Scripts.EntityComponents.Stats;
using UnityEngine;

namespace Assets.Scripts.EntityComponents
{
    [Serializable]
    public class NewResource
    {
        public event Action ValueChangedEvent;
        public event Action IncreaseEvent;
        public event Action DecreaseEvent;
        public event Action IncrementEvent;
        public event Action DecrementEvent;
        public event Action FillEvent;
        public event Action EmptyEvent;
        public event Action EdgeEvent;
        public event Action NotEdgeEvent;
        public event Action NotEmptyEvent;

        public bool IsFull => _isLimited && !(_value < _maxValueStat.Value);
        public bool IsEmpty => _value == _minValue;
        public bool IsNotEmpty => _value > _minValue;
        public bool IsOnEdge => _value == _edgeValue;

        [ReadOnly]
        [SerializeField]
        private string _name;
        public string Name => _name;

        [ReadOnly]
        [SerializeField]
        private int _value;

        [ReadOnly]
        [SerializeField]
        private bool _isLimited;

        [ReadOnly]
        [SerializeField]
        private int _minValue;

        [ReadOnly]
        [SerializeField]
        private int _edgeValue;

        private Stat _maxValueStat;

        public void SetName(string name) => _name = name;
        public void SetMinValue(int minValue) => _minValue = minValue;
        public void SetEdgeValue(int edgeValue) => _edgeValue = edgeValue;
        public void SetIsLimited(bool isLimited) => _isLimited = isLimited;
        
        public void Set(int value)
        {
            var oldValue = _value;

            if (_isLimited)
            {
                var maxValue = (int)_maxValueStat.Value;

                if (value >= maxValue)
                {
                    _value = maxValue;
                }

                if (value > _minValue && value < maxValue)
                {
                    _value = value;
                }

                if (value <= _minValue)
                {
                    _value = _minValue;
                }
            }
            else
            {
                _value = value > _minValue ? value : _minValue;
            }

            var newValue = _value;
            InvokeEvents(oldValue, newValue);
        }

        public void Fill() => Set((int)_maxValueStat.Value);

        public void Empty() => Set(_minValue);

        public void Increase(int value = 1) => Set(_value + value);

        public void Decrease(int value = 1) => Set(_value - value);

        public int GetValue() => _value;

        public int GetMinValue() => _minValue;

        public float GetMaxValue() => _isLimited ? (int)_maxValueStat.Value : Single.PositiveInfinity;

        public int GetLackValue() => (int)_maxValueStat.Value - _value;

        public float GetPercentValue()
        {
            if (!_isLimited)
            {
                return Single.NaN;
            }
            var maxValue = _maxValueStat.Value;
            var percent = maxValue / 100;
            var currentPercent = _value / percent;
            return currentPercent;
        }

        protected virtual void InvokeEvents(int oldValue, int newValue)
        {
            if (oldValue == newValue)
            {
                return;
            }

            var isFillEventRequired = false;
            var isEmptyEventRequired = false;

            var dif = newValue - oldValue;

            if (_isLimited)
            {
                var maxValue = (int)_maxValueStat.Value;

                if (newValue == maxValue)
                {
                    isFillEventRequired = true;
                }
            }

            if (newValue == _minValue)
            {
                isEmptyEventRequired = true;
            }

            if (dif > 0)
            {
                IncreaseEvent?.Invoke();
                while (dif != 0)
                {
                    IncrementEvent?.Invoke();
                    dif--;
                }
            }

            if (dif < 0)
            {
                DecreaseEvent?.Invoke();
                while (dif != 0)
                {
                    DecrementEvent?.Invoke();
                    dif++;
                }
            }

            if (oldValue == _minValue && newValue != _minValue)
            {
                NotEmptyEvent?.Invoke();
            }

            if (oldValue == _edgeValue && _value > _edgeValue)
            {
                NotEdgeEvent?.Invoke();
            }

            if (newValue == _edgeValue)
            {
                EdgeEvent?.Invoke();
            }

            ValueChangedEvent?.Invoke();
            if (isFillEventRequired) FillEvent?.Invoke();
            if (isEmptyEventRequired) EmptyEvent?.Invoke();
        }
    }
}