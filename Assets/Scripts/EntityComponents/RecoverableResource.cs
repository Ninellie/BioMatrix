using System;
using Assets.Scripts.EntityComponents.Stats;

namespace Assets.Scripts.EntityComponents
{
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
            false)
        {
        }

        public RecoverableResource(Stat maxValueStat) : this(0,
            1,
            maxValueStat,
            new Stat(0, false),
            new Stat(0, false),
            false)
        {
        }

        public RecoverableResource(int minValue) : this(minValue,
            minValue + 1,
            new Stat(Single.PositiveInfinity),
            new Stat(0, false),
            new Stat(0, false),
            false)
        {
        }

        public RecoverableResource(int minValue,
            Stat maxValueStat) : this(minValue,
            minValue + 1,
            maxValueStat,
            new Stat(0, false),
            new Stat(0, false),
            false)
        {
        }

        public RecoverableResource(int minValue,
            Stat maxValueStat,
            Stat recoverySpeedPerSecondStat) : this(minValue,
            minValue + 1,
            maxValueStat,
            recoverySpeedPerSecondStat,
            maxValueStat,
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
            true)
        {
        }

        public RecoverableResource(int minValue,
            int edgeValue,
            Stat maxValueStat,
            Stat recoverySpeedPerSecondStat,
            Stat maxRecoverableValueStat,
            bool isRecovering) : base(minValue, edgeValue, maxValueStat)
        {
            _maxRecoverableValueStat = maxRecoverableValueStat;
            _recoverySpeedPerSecondStat = recoverySpeedPerSecondStat;
            _isRecovering = isRecovering;
            _restoreRate = 0.1f;
        }
    }
}