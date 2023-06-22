using System;
using Assets.Scripts.Entity.Stats;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;

namespace Assets.Scripts.Entity
{
    public class Resource
    {
        public event Action ValueChangedEvent;
        public event Action IncreaseEvent;
        public event Action DecreaseEvent;
        public event Action IncrementEvent;
        public event Action DecrementEvent;
        public event Action FillEvent;
        public event Action RecoverEvent; 
        public event Action RecoveryStartEvent;
        public event Action FullRecoveryEvent;
        public event Action EmptyEvent;
        public event Action EdgeEvent;
        public event Action NotEdgeEvent;
        public event Action NotEmptyEvent;

        public bool IsFull => _isLimited && _value == (int)_maxValueStat.Value;
        public bool IsEmpty => _value == _minValue;
        public bool IsNotEmpty => _value > _minValue;
        public bool IsOnEdge => _value == _edgeValue;
        public bool IsFullyRecovered => _isRecovering && _value >= (int)_maxRecoverableValueStat.Value;
        public bool IsOverRecovered => _isRecovering && _value > (int)_maxRecoverableValueStat.Value;
        public bool IsOnRecovery => _isRecovering && _value < (int)_maxRecoverableValueStat.Value;

        private readonly int _minValue;
        private readonly int _edgeValue;
        private readonly Stat _maxValueStat;
        private readonly Stat _maxRecoverableValueStat;
        private readonly Stat _recoverySpeedPerSecondStat;
        private readonly float _restoreRate;
        private readonly bool _isRecovering;
        private readonly bool _isLimited;

        public float TimeToRecover // Add Time.deltatime in Update
        {
            get => _timeToRecover;
            set
            {
                if (!_isRecovering)
                {
                    return;
                }
                if (_value >= _maxRecoverableValueStat.Value)
                {
                    return;
                }
                if (value > _restoreRate)
                {
                    _timeToRecover = value % _restoreRate;
                    ReserveValue += _recoverySpeedPerSecondStat.Value * _restoreRate;
                }
                else
                {
                    _timeToRecover = value;
                }
            }
        }
        private float _timeToRecover;
        private float ReserveValue
        {
            get => _reserveValue;
            set
            {
                if (!_isRecovering)
                {
                    return;
                }
                if (_value >= _maxRecoverableValueStat.Value)
                {
                    return;
                }
                if (value > 1)
                {
                    var intValue = (int)value;
                    _reserveValue = value % 1;
                    _value += intValue;
                    RecoverEvent?.Invoke();
                }
                else
                {
                    _reserveValue = value;
                }
            }
        }
        private float _reserveValue;
        private int _value;

        public void Set(int value)
        {
            var oldValue = _value;
            _value = value;
            InvokeEvents(oldValue);
        }

        public void Fill()
        {
            var newValue = (int)_maxValueStat.Value;
            Set(newValue);
        }

        public void Empty()
        {
            var newValue = _minValue;
            Set(newValue);
        }

        public void Increase(int value)
        {
            var newValue = _value + value;
            Set(newValue);
        }

        public void Increase()
        {
            var newValue = _value + 1;
            Set(newValue);
        }

        public void Decrease(int value)
        {
            var newValue = _value - value;
            Set(newValue);
        }
    
        public void Decrease()
        {
            var newValue = _value - 1;
            Set(newValue);
        }
    
        public int GetMinValue()
        {
            return _minValue;
        }
    
        public int GetMaxValue()
        {
            return (int)_maxValueStat.Value;
        }
    
        public int GetLackValue()
        {
            var maxValue = (int)_maxValueStat.Value;
            return maxValue - _value;
        }
    
        public int GetValue()
        {
            return _value;
        }
    
        public float GetPercentValue()
        {
            var maxValue = _maxValueStat.Value;
            var percent = maxValue / 100;
            var currentPercent = _value / percent;
            return currentPercent;
        }

        private void InvokeEvents(int oldValue)
        {
            var newValue = _value;

            if (oldValue == newValue)
            {
                return;
            }

            var isFillEventRequired = false;
            var isEmptyEventRequired = false;
            var isFullRecoveryEventRequired = false;
            var isRecoveryStartEventRequired = false;

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

            if (_isRecovering)
            {
                var maxRecoverableValue = (int)_maxRecoverableValueStat.Value;
                var isFullyRecovered = newValue >= maxRecoverableValue;
                var isOnRecovery = newValue < maxRecoverableValue;

                if (oldValue < maxRecoverableValue && isFullyRecovered)
                {
                    isFullRecoveryEventRequired = true;
                }
                if (oldValue >= maxRecoverableValue && isOnRecovery)
                {
                    isRecoveryStartEventRequired = true;
                }
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
            if (isFullRecoveryEventRequired) FullRecoveryEvent?.Invoke();
            if (isRecoveryStartEventRequired) RecoveryStartEvent?.Invoke();
        }

        public Resource() : this(0,
        1,
        new Stat(Single.PositiveInfinity),
        new Stat(0, false),
        new Stat(0, false),
        false,
        false)
        {
        }

        public Resource(Stat maxValueStat) : this(0,
            1,
            maxValueStat,
            new Stat(0, false),
            new Stat(0, false),
            false,
            false)
        {
        }

        public Resource(int minValue) : this(minValue,
            minValue + 1,
            new Stat(Single.PositiveInfinity),
            new Stat(0, false),
            new Stat(0, false),
            false,
            false)
        {
        }

        public Resource(int minValue,
            Stat maxValueStat) : this(minValue,
            minValue + 1,
            maxValueStat,
            new Stat(0, false),
            new Stat(0, false),
            false,
            true)
        {
        }
        public Resource(int minValue,
            Stat maxValueStat,
            Stat recoverySpeedPerSecondStat) : this(minValue,
            minValue + 1,
            maxValueStat,
            recoverySpeedPerSecondStat,
            maxValueStat,
            true,
            true)
        {
        }
        public Resource(int minValue,
            Stat maxValueStat,
            Stat recoverySpeedPerSecondStat,
            Stat maxRecoverableValueStat) : this(minValue,
            minValue + 1,
            maxValueStat,
            recoverySpeedPerSecondStat,
            maxRecoverableValueStat,
            true,
            true)
        {
        }
        public Resource(int minValue,
            int edgeValue,
            Stat maxValueStat,
            Stat recoverySpeedPerSecondStat,
            Stat maxRecoverableValueStat) : this(minValue,
            edgeValue,
            maxValueStat,
            recoverySpeedPerSecondStat,
            maxRecoverableValueStat,
            true,
            true)
        {
        }
        public Resource(int minValue,
            int edgeValue,
            Stat maxValueStat,
            Stat recoverySpeedPerSecondStat,
            Stat maxRecoverableValueStat,
            bool isRecovering,
            bool isLimited)
        {
            _minValue = minValue;
            _edgeValue = edgeValue;
            _maxValueStat = maxValueStat;
            _recoverySpeedPerSecondStat = recoverySpeedPerSecondStat;
            _isRecovering = isRecovering;
            _restoreRate = 0.1f;
            _isLimited = isLimited;
            _maxRecoverableValueStat = maxRecoverableValueStat;
        }
    }
}