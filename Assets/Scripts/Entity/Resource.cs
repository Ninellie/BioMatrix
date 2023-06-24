using System;
using Assets.Scripts.Entity;
using Assets.Scripts.Entity.Stats;

public class RecoverableResource : Resource
{
    public event Action RecoverEvent;
    public event Action RecoveryStartEvent;
    public event Action FullRecoveryEvent;

    public bool IsFullyRecovered => _isRecovering && GetValue() >= (int)_maxRecoverableValueStat.Value;
    public bool IsOverRecovered => _isRecovering && GetValue() > (int)_maxRecoverableValueStat.Value;
    public bool IsOnRecovery => _isRecovering && GetValue() < (int)_maxRecoverableValueStat.Value;

    private readonly Stat _maxRecoverableValueStat;
    private readonly Stat _recoverySpeedPerSecondStat;
    private readonly float _restoreRate;
    private readonly bool _isRecovering;

    public float TimeToRecover // Add Time.deltatime in Update
    {
        get => _timeToRecover;
        set
        {
            if (!_isRecovering)
            {
                return;
            }
            if (GetValue() >= _maxRecoverableValueStat.Value)
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
            if (GetValue() >= _maxRecoverableValueStat.Value)
            {
                return;
            }
            if (value > 1)
            {
                var intValue = (int)value;
                _reserveValue = value % 1;
                Increase(intValue);
                RecoverEvent?.Invoke();
            }
            else
            {
                _reserveValue = value;
            }
        }
    }
    private float _reserveValue;

    protected override void InvokeEvents(int oldValue, int newValue)
    {
        base.InvokeEvents(oldValue, newValue);
        InvokeRecoveryEvents(oldValue, newValue);
    }

    private void InvokeRecoveryEvents(int oldValue, int newValue)
    {
        var isFullRecoveryEventRequired = false;
        var isRecoveryStartEventRequired = false;

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

        if (isFullRecoveryEventRequired) FullRecoveryEvent?.Invoke();
        if (isRecoveryStartEventRequired) RecoveryStartEvent?.Invoke();
    }

    public RecoverableResource() : this(0,
    1,
    new Stat(Single.PositiveInfinity),
    new Stat(0, false),
    new Stat(0, false),
    false,
    false)
    {
    }

    public RecoverableResource(Stat maxValueStat) : this(0,
        1,
        maxValueStat,
        new Stat(0, false),
        new Stat(0, false),
        false,
        false)
    {
    }

    public RecoverableResource(int minValue) : this(minValue,
        minValue + 1,
        new Stat(Single.PositiveInfinity),
        new Stat(0, false),
        new Stat(0, false),
        false,
        false)
    {
    }

    public RecoverableResource(int minValue,
        Stat maxValueStat) : this(minValue,
        minValue + 1,
        maxValueStat,
        new Stat(0, false),
        new Stat(0, false),
        false,
        true)
    {
    }

    public RecoverableResource(int minValue,
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

    public RecoverableResource(int minValue,
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

    public RecoverableResource(int minValue,
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

    public RecoverableResource(int minValue,
        int edgeValue,
        Stat maxValueStat,
        Stat recoverySpeedPerSecondStat,
        Stat maxRecoverableValueStat,
        bool isRecovering,
        bool isLimited) : base(minValue, edgeValue, maxValueStat, isLimited)
    {
        _maxRecoverableValueStat = maxRecoverableValueStat;
        _recoverySpeedPerSecondStat = recoverySpeedPerSecondStat;
        _isRecovering = isRecovering;
        _restoreRate = 0.1f;
    }
}

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

        //public event Action RecoverEvent;
        //public event Action RecoveryStartEvent;
        //public event Action FullRecoveryEvent;

        public event Action EmptyEvent;
        public event Action EdgeEvent;
        public event Action NotEdgeEvent;
        public event Action NotEmptyEvent;

        public bool IsFull => _isLimited && _value == (int)_maxValueStat.Value;
        public bool IsEmpty => _value == _minValue;
        public bool IsNotEmpty => _value > _minValue;
        public bool IsOnEdge => _value == _edgeValue;

        private int _value;

        private readonly int _minValue;
        private readonly int _edgeValue;
        private readonly Stat _maxValueStat;
        private readonly bool _isLimited;

        public void Set(int value)
        {
            var oldValue = _value;
            _value = value;
            var newValue = _value;
            InvokeEvents(oldValue, newValue);
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

        public Resource() : this(0,
        1,
        new Stat(Single.PositiveInfinity),
        false)
        {
        }

        public Resource(Stat maxValueStat) : this(0,
            1,
            maxValueStat,
            false)
        {
        }

        public Resource(int minValue) : this(minValue,
            minValue + 1,
            new Stat(Single.PositiveInfinity),
            false)
        {
        }

        public Resource(int minValue,
            Stat maxValueStat) : this(minValue,
            minValue + 1,
            maxValueStat,
            true)
        {
        }

        public Resource(int minValue,
            int edgeValue,
            Stat maxValueStat) : this(minValue,
            edgeValue,
            maxValueStat,
            true)
        {
        }

        public Resource(int minValue,
            int edgeValue,
            Stat maxValueStat,
            bool isLimited)
        {
            _minValue = minValue;
            _edgeValue = edgeValue;
            _maxValueStat = maxValueStat;
            _isLimited = isLimited;
        }
    }
}