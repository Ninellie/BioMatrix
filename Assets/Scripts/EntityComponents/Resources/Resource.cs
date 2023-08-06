using System;
using Assets.Scripts.CustomAttributes;
using Assets.Scripts.EntityComponents.Stats;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.EntityComponents.Resources
{
    [Serializable]
    public class Resource
    {
        //public event Action ValueChangedEvent;
        //public event Action IncreaseEvent;
        //public event Action DecreaseEvent;
        //public event Action IncrementEvent;
        //public event Action DecrementEvent;
        //public event Action FillEvent;
        //public event Action EmptyEvent;
        //public event Action EdgeEvent;
        //public event Action NotEdgeEvent;
        //public event Action NotEmptyEvent;

        [SerializeField]
        public UnityEvent ValueChangedEvent;
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
        public ResourceName Name => _name;

        [ReadOnly]
        [SerializeField]
        private ResourceName _name;

        [ReadOnly]
        [SerializeField]
        private int _value;

        [ReadOnly]
        [SerializeField]
        private bool _isLimited;

        [ReadOnly]
        [SerializeField]
        private bool _isInfinite;

        [ReadOnly]
        [SerializeField]
        private int _minValue;

        [ReadOnly]
        [SerializeField]
        private int _edgeValue;

        [ReadOnly]
        [SerializeField]
        private Stat _maxValueStat;

        /// <summary>
        /// Creates infinite resource
        /// </summary>
        /// <param name="name"></param>
        public Resource(ResourceName name)
        {
            _name = name;
            _isLimited = false;
            _minValue = 0;
            _edgeValue = 1;
            _maxValueStat = new Stat();
            _maxValueStat.SetSettings();
            _maxValueStat.SetBaseValue(float.PositiveInfinity);
            _value = int.MaxValue;
            _isInfinite = true;
        }

        /// <summary>
        /// Creates unlimited empty resource
        /// </summary>
        /// <param name="name"></param>
        /// <param name="minValue"></param>
        /// <param name="edgeValue"></param>
        public Resource(ResourceName name, int minValue, int edgeValue)
        {
            _name = name;
            _isLimited = false;
            _minValue = minValue;
            _edgeValue = edgeValue;
            _maxValueStat = new Stat();
            _maxValueStat.SetSettings();
            _maxValueStat.SetBaseValue(float.PositiveInfinity);
            _value = _minValue;
        }

        /// <summary>
        /// Creates unlimited resource
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="minValue"></param>
        /// <param name="edgeValue"></param>
        public Resource(ResourceName name, int value, int minValue, int edgeValue)
        {
            _name = name;
            _isLimited = true;
            _minValue = minValue;
            _edgeValue = edgeValue;
            _maxValueStat = new Stat();
            _maxValueStat.SetSettings();
            _maxValueStat.SetBaseValue(float.PositiveInfinity);
            
            _value = value < minValue ? minValue : value;
        }

        /// <summary>
        /// Creates limited resource
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="minValue"></param>
        /// <param name="edgeValue"></param>
        /// <param name="maxValueStat"></param>
        public Resource(ResourceName name, int value, int minValue, int edgeValue, Stat maxValueStat)
        {
            _name = name;
            _isLimited = true;
            _minValue = minValue;
            _edgeValue = edgeValue;
            _maxValueStat = maxValueStat;

            if (value > maxValueStat.Value)
            {
                _value = (int)maxValueStat.Value;
            }
            else if (value < minValue)
            {
                _value = minValue;
            }
            else
            {
                _value = value;
            }
        }
        
        public void Set(int value)
        {
            if (_isInfinite)
            {
                value = Mathf.Min(value, _value);
            }

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