using System;
using Assets.Scripts.Core.Variables.References;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.View
{
    public enum ValueFormat
    {
        D,
        D1,
        D2,
        D3
    }

    public enum PercentFormat
    {
        Flat,
        Sigh
    }

    [AddComponentMenu("UI/Indicator/Reserve counter")]
    public class ReserveCounter : MonoBehaviour
    {
        [Header("Reserve settings")]
        [Header("Label")]
        [SerializeField] private string _reserveName = "Simple Resource";
        [Header("Variables")]
        [SerializeField] protected IntReference _reserve;
        [SerializeField] protected FloatReference _maximumReserveValue;
        [Space]
        [Header("Counter text settings")]
        [SerializeField] private CounterFormat _counterFormat;
        [SerializeField] private ValueFormat _valueFormat;
        [SerializeField] private bool _inPercent;
        [SerializeField] private PercentFormat _percentFormat;
        [Space]
        [Header("Counter text")]
        [SerializeField] private TMP_Text _text;

        public virtual void UpdateCounter()
        {
            UpdateLabel();
        }

        private void UpdateLabel()
        {
            var maxValue = GetMaxValue();
            var currentValue = GetCurrentValue();

            var textToSet = _counterFormat switch
            {
                CounterFormat.LabelCurrentSlashMax => $"{_reserveName} {currentValue}/{maxValue}",
                CounterFormat.CurrentSlashMax => $"{currentValue}/{maxValue}",
                CounterFormat.LabelCurrentOnly => $"{_reserveName} {currentValue}",
                CounterFormat.CurrentOnly => $"{currentValue}",
                CounterFormat.LabelOnly => $"{_reserveName}",
                CounterFormat.DoNotShow => $"",
                _ => throw new ArgumentOutOfRangeException()
            };
            _text.SetText(textToSet);
        }

        protected int GetPercentCurrentValue()
        {
            if (_maximumReserveValue == null) return 0;
            var percent = _maximumReserveValue.Value / 100;
            var currentPercent = _reserve.Value / percent;
            return (int)currentPercent;
        }

        private string GetMaxValue()
        {
            if (_maximumReserveValue == null) return float.NaN.ToString();
            if (_inPercent)
            {
                var maxPercentValue = "100";
                if (_percentFormat == PercentFormat.Sigh)
                {
                    maxPercentValue += "%";
                }
                return maxPercentValue;
            }

            var maxValue = _valueFormat switch
            {
                ValueFormat.D => $"{_maximumReserveValue:D}",
                ValueFormat.D1 => $"{_maximumReserveValue:D1}",
                ValueFormat.D2 => $"{_maximumReserveValue:D2}",
                ValueFormat.D3 => $"{_maximumReserveValue:D3}",
                _ => throw new ArgumentOutOfRangeException()
            };

            return maxValue;
        }

        private string GetCurrentValue()
        {
            var currentIntValue = _inPercent ? GetPercentCurrentValue() : _reserve.Value;

            var currentValue = _valueFormat switch
            {
                ValueFormat.D => $"{currentIntValue:D}",
                ValueFormat.D1 => $"{currentIntValue:D1}",
                ValueFormat.D2 => $"{currentIntValue:D2}",
                ValueFormat.D3 => $"{currentIntValue:D3}",
                _ => throw new ArgumentOutOfRangeException()
            };

            if (_inPercent && _percentFormat == PercentFormat.Sigh)
            {
                currentValue += "%";
            }

            return currentValue;
        }
    }
}