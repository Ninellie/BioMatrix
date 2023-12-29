using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.View
{
    [AddComponentMenu("UI/Indicator/Bar")]
    public class Bar : Counter
    {
        [Space]
        [Header("Bar settings")]
        [SerializeField] private BarDisplayFormat _barDisplayFormat;
        [SerializeField] private float _limit;
        [SerializeField] [Range(0, 1)] private float _valueStepPerUnit;
        [Space]
        [Header("background")]
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private TMP_ColorGradient _backgroundColor;
        [Space]
        [Header("Value slider")]
        [SerializeField] private TMP_ColorGradient _valueColor;
        [SerializeField] private Image _valueImage;
        [SerializeField] private RectTransform _sliderRectTransform;

        private void Awake()
        { 
            SetValueImageColor(_valueColor);
            SetBackgroundImageColor(_backgroundColor);
        }

        private void Start()
        {
            UpdateCounter();
        }

        private void OnValidate()
        {
            SetValueImageColor(_valueColor);
            SetBackgroundImageColor(_backgroundColor);
            UpdateCounter();
        }

        public override void UpdateCounter()
        {
            UpdateBarValue();
            base.UpdateCounter();
        }

        private void UpdateBarValue()
        {
            var percentValue = GetPercentCurrentValue();

            var value = _barDisplayFormat switch
            {
                BarDisplayFormat.Percent => percentValue > 0 ? percentValue / 100f : 0,
                BarDisplayFormat.LimitedToNumber => Mathf.Min(_value.Value, _limit),
                BarDisplayFormat.Unlimited => _value.Value * _valueStepPerUnit,
                _ => throw new ArgumentOutOfRangeException()
            };
            _sliderRectTransform.anchorMax = new Vector2(value, 1);
        }

        private void SetValueImageColor(TMP_ColorGradient color)
        {
            _valueImage.color = color.topLeft;
        }

        private void SetBackgroundImageColor(TMP_ColorGradient color)
        {
            _backgroundImage.color = color.topLeft;
        }
    }
}