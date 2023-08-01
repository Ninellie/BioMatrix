using System;
using Assets.Scripts.EntityComponents.Stats;

namespace Assets.Scripts.EntityComponents
{
    public interface IRecoverable
    {
        event Action RecoverEvent;
        event Action RecoveryStartEvent;
        event Action FullRecoveryEvent;
        void AddRecoveryTime(float time);
    }

    public class RecoverableResource : Resource, IRecoverable
    {
        public event Action RecoverEvent;
        public event Action RecoveryStartEvent;
        public event Action FullRecoveryEvent;

        public bool IsFullyRecovered => _isRecovering && GetValue() >= (int)_maxRecoverableValueOldStat.Value;
        public bool IsOverRecovered => _isRecovering && GetValue() > (int)_maxRecoverableValueOldStat.Value;
        public bool IsOnRecovery => _isRecovering && GetValue() < (int)_maxRecoverableValueOldStat.Value;

        private readonly OldStat _maxRecoverableValueOldStat;
        private readonly OldStat _recoverySpeedPerSecondOldStat;
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
                if (GetValue() >= _maxRecoverableValueOldStat.Value)
                {
                    return;
                }
                if (value > _restoreRate)
                {
                    _timeToRecover = value % _restoreRate;
                    ReserveValue += _recoverySpeedPerSecondOldStat.Value * _restoreRate;
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
                if (GetValue() >= _maxRecoverableValueOldStat.Value)
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

        public void AddRecoveryTime(float time)
        {

        }

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
                var maxRecoverableValue = (int)_maxRecoverableValueOldStat.Value;
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
            new OldStat(Single.PositiveInfinity),
            new OldStat(0, false),
            new OldStat(0, false),
            false)
        {
        }

        public RecoverableResource(OldStat maxValueOldStat) : this(0,
            1,
            maxValueOldStat,
            new OldStat(0, false),
            new OldStat(0, false),
            false)
        {
        }

        public RecoverableResource(int minValue) : this(minValue,
            minValue + 1,
            new OldStat(Single.PositiveInfinity),
            new OldStat(0, false),
            new OldStat(0, false),
            false)
        {
        }

        public RecoverableResource(int minValue,
            OldStat maxValueOldStat) : this(minValue,
            minValue + 1,
            maxValueOldStat,
            new OldStat(0, false),
            new OldStat(0, false),
            false)
        {
        }

        public RecoverableResource(int minValue,
            OldStat maxValueOldStat,
            OldStat recoverySpeedPerSecondOldStat) : this(minValue,
            minValue + 1,
            maxValueOldStat,
            recoverySpeedPerSecondOldStat,
            maxValueOldStat,
            true)
        {
        }

        public RecoverableResource(int minValue,
            OldStat maxValueOldStat,
            OldStat recoverySpeedPerSecondOldStat,
            OldStat maxRecoverableValueOldStat) : this(minValue,
            minValue + 1,
            maxValueOldStat,
            recoverySpeedPerSecondOldStat,
            maxRecoverableValueOldStat,
            true)
        {
        }

        public RecoverableResource(int minValue,
            int edgeValue,
            OldStat maxValueOldStat,
            OldStat recoverySpeedPerSecondOldStat,
            OldStat maxRecoverableValueOldStat) : this(minValue,
            edgeValue,
            maxValueOldStat,
            recoverySpeedPerSecondOldStat,
            maxRecoverableValueOldStat,
            true)
        {
        }

        public RecoverableResource(int minValue,
            int edgeValue,
            OldStat maxValueOldStat,
            OldStat recoverySpeedPerSecondOldStat,
            OldStat maxRecoverableValueOldStat,
            bool isRecovering) : base(minValue, edgeValue, maxValueOldStat)
        {
            _maxRecoverableValueOldStat = maxRecoverableValueOldStat;
            _recoverySpeedPerSecondOldStat = recoverySpeedPerSecondOldStat;
            _isRecovering = isRecovering;
            _restoreRate = 0.1f;
        }
    }
}