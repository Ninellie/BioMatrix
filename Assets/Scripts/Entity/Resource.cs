using System;

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

        public bool IsFull => _isLimited && Value == (int)_maxValueStat.Value;
        public bool IsEmpty => Value == _minValue;
        public bool IsOnEdge => Value == _edgeValue;
        public bool IsFullyRecovered => _isRecovering && Value >= (int)_maxRecoverableValueStat.Value;
        public bool IsOverRecovered => _isRecovering && Value > (int)_maxRecoverableValueStat.Value;
        public bool IsOnRecovery => _isRecovering && Value < (int)_maxRecoverableValueStat.Value;

        private readonly int _minValue;
        private readonly int _edgeValue;
        private readonly Stat.Stat _maxValueStat;
        private readonly Stat.Stat _maxRecoverableValueStat;
        private readonly Stat.Stat _recoverySpeedPerSecondStat;
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
                if (Value >= _maxRecoverableValueStat.Value)
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
                if (Value >= _maxRecoverableValueStat.Value)
                {
                    return;
                }
                if (value > 1)
                {
                    var intValue = (int)value;
                    _reserveValue = value % 1;
                    Value += intValue;
                    RecoverEvent?.Invoke();
                }
                else
                {
                    _reserveValue = value;
                }
            }
        }
        private float _reserveValue;

        private int Value
        {
            get => _value;
            set
            {
                var dif = value - _value;
                if (dif == 0)
                {
                    return;
                }

                var oldValue = _value;

                if (_isLimited)
                {
                    if (value >= (int)_maxValueStat.Value)
                    {
                        _value = (int)_maxValueStat.Value;
                        FillEvent?.Invoke();
                    }
                }
                if (value <= _minValue)
                {
                    _value = _minValue;
                    EmptyEvent?.Invoke();
                }
                else
                {
                    _value = value;
                }

                ValueChangedEvent?.Invoke();

                if (_isRecovering)
                {
                    if (oldValue < (int)_maxRecoverableValueStat.Value && IsFullyRecovered)
                    {
                        FullRecoveryEvent?.Invoke();
                    }
                    if (oldValue >= (int)_maxRecoverableValueStat.Value && IsOnRecovery)
                    {
                        RecoveryStartEvent?.Invoke();
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

                if (oldValue == _minValue && !IsEmpty)
                {
                    NotEmptyEvent?.Invoke();
                }
                if (oldValue == _edgeValue && _value > _edgeValue)
                {
                    NotEdgeEvent?.Invoke();
                }
                if (IsOnEdge)
                {
                    EdgeEvent?.Invoke();
                }
            }
        }
        private int _value;
    
        public Resource() : this(0,
            1,
            new Stat.Stat(Single.PositiveInfinity),
            new Stat.Stat(0, false),
            new Stat.Stat(0, false),
            false,
            false)
        {
        }

        public Resource(Stat.Stat maxValueStat) : this(0,
            1,
            maxValueStat,
            new Stat.Stat(0, false),
            new Stat.Stat(0, false),
            false,
            false)
        {
        }

        public Resource(int minValue) : this(minValue,
            minValue + 1,
            new Stat.Stat(Single.PositiveInfinity),
            new Stat.Stat(0, false),
            new Stat.Stat(0, false),
            false,
            false)
        {
        }

        public Resource(int minValue,
            Stat.Stat maxValueStat) : this(minValue,
            minValue + 1,
            maxValueStat,
            new Stat.Stat(0, false),
            new Stat.Stat(0, false),
            false,
            true)
        {
        }
        public Resource(int minValue,
            Stat.Stat maxValueStat,
            Stat.Stat recoverySpeedPerSecondStat) : this(minValue,
            minValue + 1,
            maxValueStat,
            recoverySpeedPerSecondStat,
            maxValueStat,
            true,
            true)
        {
        }
        public Resource(int minValue,
            Stat.Stat maxValueStat,
            Stat.Stat recoverySpeedPerSecondStat,
            Stat.Stat maxRecoverableValueStat) : this(minValue,
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
            Stat.Stat maxValueStat,
            Stat.Stat recoverySpeedPerSecondStat,
            Stat.Stat maxRecoverableValueStat) : this(minValue,
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
            Stat.Stat maxValueStat,
            Stat.Stat recoverySpeedPerSecondStat,
            Stat.Stat maxRecoverableValueStat,
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

        public void Set(int value)
        {
            Value = value;
        }

        public void Fill()
        {
            Value = (int)_maxValueStat.Value;
        }

        public void Empty()
        {
            Value = _minValue;
        }

        public void Increase(int value)
        {
            Value += value;
        }

        public void Increase()
        {
            Value++;
        }

        public void Decrease(int value)
        {
            Value -= value;
        }
    
        public void Decrease()
        {
            Value--;
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
            return (int)(_maxValueStat.Value - Value);
        }
    
        public int GetValue()
        {
            return Value;
        }
    
        public float GetPercentValue()
        {
            var percent = _maxValueStat.Value / 100;
            var currentPercent = Value / percent;
            return currentPercent;
        }
    }
}