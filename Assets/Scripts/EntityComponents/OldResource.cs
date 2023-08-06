using System;
using Assets.Scripts.EntityComponents.Stats;
using static System.Single;

namespace Assets.Scripts.EntityComponents
{
    public class OldResource
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

        public bool IsFull => _isLimited && !(_value < _maxValueOldStat.Value);
        public bool IsEmpty => _value == _minValue;
        public bool IsNotEmpty => _value > _minValue;
        public bool IsOnEdge => _value == _edgeValue;

        private int _value;

        private readonly int _minValue;
        private readonly int _edgeValue;
        private readonly OldStat _maxValueOldStat;
        private readonly bool _isLimited;

        public void Set(int value)
        {
            var oldValue = _value;

            if (_isLimited)
            {
                var maxValue = (int)_maxValueOldStat.Value;

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

        public void Fill() => Set((int)_maxValueOldStat.Value);

        public void Empty() => Set(_minValue);

        public void Increase(int value = 1) => Set(_value + value);

        public void Decrease(int value = 1) => Set(_value - value);

        public int GetValue() => _value;

        public int GetMinValue() => _minValue;

        public float GetMaxValue() => _isLimited ? (int)_maxValueOldStat.Value : PositiveInfinity;

        public int GetLackValue() => (int)_maxValueOldStat.Value - _value;

        public float GetPercentValue()
        {
            if (!_isLimited)
            {
                return NaN;
            }
            var maxValue = _maxValueOldStat.Value;
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
                var maxValue = (int)_maxValueOldStat.Value;

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

        public OldResource() : this(0,
        1,
        new OldStat(PositiveInfinity))
        {
        }

        public OldResource(OldStat maxValueOldStat) : this(0,
            1,
            maxValueOldStat)
        {
        }

        public OldResource(int minValue) : this(minValue,
            minValue + 1,
            new OldStat(PositiveInfinity))
        {
        }

        public OldResource(int minValue,
            OldStat maxValueOldStat) : this(minValue,
            minValue + 1,
            maxValueOldStat)
        {
        }

        public OldResource(int minValue,
            int edgeValue,
            OldStat maxValueOldStat)
        {
            _minValue = minValue;
            _edgeValue = edgeValue;
            _maxValueOldStat = maxValueOldStat;
            _isLimited = !IsPositiveInfinity(maxValueOldStat.Value);
        }
    }
}