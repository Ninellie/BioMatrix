using System;
using Assets.Scripts.EntityComponents.Stats;
using Assets.Scripts.SourceStatSystem;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.EntityComponents.Resources
{
    [Serializable]
    public enum ResourceEventType
    {
        ValueChanged,
        Increase,
        Decrease,
        Increment,
        Decrement,
        Fill,
        Empty,
        Edge,
        NotEdge,
        NotEmpty,
    }

    [Serializable]
    public class Resource
    {
        [HideInInspector] public string stringName;
        public ResourceName Name => _name;
        public bool IsFull => _isLimited && !(_value < MaxValue);
        public bool IsEmpty => _value == _minValue;
        public bool IsNotEmpty => _value > _minValue;
        public bool IsOnEdge => _value == _edgeValue;

        [SerializeField] private ResourceName _name;
        [SerializeField] private int _value;
        [SerializeField] private bool _isLimited;
        [SerializeField] private bool _isInfinite;
        [SerializeField] private int _minValue;
        [SerializeField] private int _edgeValue;

        private int MaxValue
        {
            get
            {
                if (_isLimited)
                {
                    return (int)_statManager.GetStatValue(_maxValueStatId);
                }
                else
                {
                    return int.MaxValue;
                }
            }
        }

        [SerializeField] private StatId _maxValueStatId;
        [SerializeField] private StatManagerComponent _statManager;

        [HideInInspector, NonSerialized] public UnityEvent onValueChanged = new();
        [HideInInspector, NonSerialized] public UnityEvent onIncrease = new();
        [HideInInspector, NonSerialized] public UnityEvent onDecrease = new();
        [HideInInspector, NonSerialized] public UnityEvent onIncrement = new();
        [HideInInspector, NonSerialized] public UnityEvent onDecrement = new();
        [HideInInspector, NonSerialized] public UnityEvent onFill = new();
        [HideInInspector, NonSerialized] public UnityEvent onEmpty = new();
        [HideInInspector, NonSerialized] public UnityEvent onEdge = new();
        [HideInInspector, NonSerialized] public UnityEvent onNotEdge = new();
        [HideInInspector, NonSerialized] public UnityEvent onNotEmpty = new();

        private void InitializeEvents()
        {
            onValueChanged = new UnityEvent();
            onIncrease = new UnityEvent();
            onDecrease = new UnityEvent();
            onIncrement = new UnityEvent();
            onDecrement = new UnityEvent();
            onFill = new UnityEvent();
            onEmpty = new UnityEvent();
            onEdge = new UnityEvent();
            onNotEdge = new UnityEvent();
            onNotEmpty = new UnityEvent();
        }

        /// <summary>
        /// Creates infinite resource
        /// </summary>
        /// <param name="name"></param>
        //public Resource(ResourceName name)
        //{
        //    _name = name;
        //    _isLimited = false;
        //    _minValue = 0;
        //    _edgeValue = 1;
        //    _maxValueStat = new Stat();
        //    _maxValueStat.SetSettings();
        //    _maxValueStat.SetBaseValue(float.PositiveInfinity);
        //    _value = int.MaxValue;
        //    _isInfinite = true;
        //    InitializeEvents();
        //}

        /// <summary>
        /// Creates unlimited empty resource
        /// </summary>
        /// <param name="name"></param>
        /// <param name="minValue"></param>
        /// <param name="edgeValue"></param>
        //public Resource(ResourceName name, int minValue, int edgeValue)
        //{
        //    _name = name;
        //    _isLimited = false;
        //    _minValue = minValue;
        //    _edgeValue = edgeValue;
        //    _maxValueStat = new Stat();
        //    _maxValueStat.SetSettings();
        //    _maxValueStat.SetBaseValue(float.PositiveInfinity);
        //    _value = _minValue;
        //    InitializeEvents();
        //}

        /// <summary>
        /// Creates unlimited resource
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="minValue"></param>
        /// <param name="edgeValue"></param>
        //public Resource(ResourceName name, int value, int minValue, int edgeValue)
        //{
        //    _name = name;
        //    _isLimited = false;
        //    _minValue = minValue;
        //    _edgeValue = edgeValue;
        //    _maxValueStat = new Stat();
        //    _maxValueStat.SetSettings();
        //    _maxValueStat.SetBaseValue(float.PositiveInfinity);
        //    _value = value < minValue ? minValue : value;
        //    InitializeEvents();
        //}
        
        public Resource(ResourceName name, int value, int minValue, int edgeValue)
        {
            _name = name;
            _isLimited = false;
            _minValue = minValue;
            _edgeValue = edgeValue;
            _value = value < minValue ? minValue : value;
            InitializeEvents();
        }

        /// <summary>
        /// Creates limited resource
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="minValue"></param>
        /// <param name="edgeValue"></param>
        /// <param name="maxValueStatId"></param>
        /// <param name="statManager"></param>
        public Resource(ResourceName name, int value, int minValue, int edgeValue, StatId maxValueStatId, StatManagerComponent statManager)
        {
            _name = name;
            _isLimited = true;
            _minValue = minValue;
            _edgeValue = edgeValue;
            _statManager = statManager;
            _maxValueStatId = maxValueStatId;

            var maxValue = (int)statManager.GetStatValue(maxValueStatId);

            if (value > maxValue) _value = maxValue;
            else if (value < minValue) _value = minValue;
            else _value = value;

            InitializeEvents();
        }

        public void Set(int value)
        {
            if (_isInfinite) value = Mathf.Min(value, _value);
            var oldValue = _value;
            if (_isLimited)
            {
                var maxValue = MaxValue;
                if (value >= maxValue) _value = maxValue;
                if (value > _minValue && value < maxValue) _value = value;
                if (value <= _minValue) _value = _minValue;
            }
            else
            {
                _value = value > _minValue ? value : _minValue;
            }
            var newValue = _value;
            InvokeEvents(oldValue, newValue);
        }

        public void Fill() => Set(MaxValue);
        public void Empty() => Set(_minValue);
        public void Increase(int value = 1) => Set(_value + value);
        public void Decrease(int value = 1) => Set(_value - value);
        public int GetValue() => _value;
        public int GetMinValue() => _minValue;
        public float GetMaxValue() => _isLimited ? MaxValue : float.PositiveInfinity;
        public int GetLackValue() => MaxValue - _value;
        public float GetPercentValue()
        {
            if (!_isLimited) return float.NaN;
            var percent = MaxValue / 100;
            var currentPercent = _value / percent;
            return currentPercent;
        }

        public void AddListenerToEvent(ResourceEventType eventType, UnityAction action)
        {
            switch (eventType)
            {
                case ResourceEventType.ValueChanged:
                    onValueChanged.AddListener(action);
                    break;
                case ResourceEventType.Increase:
                    onIncrease.AddListener(action);
                    break;
                case ResourceEventType.Decrease:
                    onDecrease.AddListener(action);
                    break;
                case ResourceEventType.Increment:
                    onIncrement.AddListener(action);
                    break;
                case ResourceEventType.Decrement:
                    onDecrement.AddListener(action);
                    break;
                case ResourceEventType.Fill:
                    onFill.AddListener(action);
                    break;
                case ResourceEventType.Empty:
                    onEmpty.AddListener(action);
                    break;
                case ResourceEventType.Edge:
                    onEdge.AddListener(action);
                    break;
                case ResourceEventType.NotEdge:
                    onNotEdge.AddListener(action);
                    break;
                case ResourceEventType.NotEmpty:
                    onNotEmpty.AddListener(action);
                    break;
            }
        }

        public void RemoveListenerToEvent(ResourceEventType eventType, UnityAction action)
        {
            switch (eventType)
            {
                case ResourceEventType.ValueChanged:
                    onValueChanged.RemoveListener(action);
                    break;
                case ResourceEventType.Increase:
                    onIncrease.RemoveListener(action);
                    break;
                case ResourceEventType.Decrease:
                    onDecrease.RemoveListener(action);
                    break;
                case ResourceEventType.Increment:
                    onIncrement.RemoveListener(action);
                    break;
                case ResourceEventType.Decrement:
                    onDecrement.RemoveListener(action);
                    break;
                case ResourceEventType.Fill:
                    onFill.RemoveListener(action);
                    break;
                case ResourceEventType.Empty:
                    onEmpty.RemoveListener(action);
                    break;
                case ResourceEventType.Edge:
                    onEdge.RemoveListener(action);
                    break;
                case ResourceEventType.NotEdge:
                    onNotEdge.RemoveListener(action);
                    break;
                case ResourceEventType.NotEmpty:
                    onNotEmpty.RemoveListener(action);
                    break;
            }
        }

        private void InvokeEvents(int oldValue, int newValue)
        {
            if (oldValue == newValue) return;

            var isFillEventRequired = false;
            var isEmptyEventRequired = false;

            var dif = newValue - oldValue;

            if (_isLimited)
            {
                var maxValue = MaxValue;

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
                onIncrease?.Invoke();
                while (dif != 0)
                {
                    onIncrement?.Invoke();
                    dif--;
                }
            }

            if (dif < 0)
            {
                onDecrease?.Invoke();
                while (dif != 0)
                {
                    onDecrement?.Invoke();
                    dif++;
                }
            }

            if (oldValue == _minValue && newValue != _minValue) onNotEmpty?.Invoke();
            if (oldValue == _edgeValue && _value > _edgeValue) onNotEdge?.Invoke();
            if (newValue == _edgeValue) onEdge?.Invoke();
            onValueChanged?.Invoke();
            if (isFillEventRequired) onFill?.Invoke();
            if (isEmptyEventRequired) onEmpty?.Invoke();
        }
    }
}