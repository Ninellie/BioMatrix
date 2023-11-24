using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.View
{
    [AddComponentMenu("UI/Indicator/Reserve bar")]
    public class ReserveBar : ReserveCounter
    {
        [Space]
        [Header("Bar settings")]
        [SerializeField] private BarDisplayFormat _barDisplayFormat;
        [SerializeField] private float _limit;
        [SerializeField] [Range(0, 1)] private float _valueStepPerResourceUnit;
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

        public override void UpdateCounter()
        {
            UpdateBarValue();
            base.UpdateCounter();
        }

        private void UpdateBarValue()
        {
            var value = _barDisplayFormat switch
            {
                BarDisplayFormat.Percent => GetPercentCurrentValue() / 100f,
                BarDisplayFormat.LimitedToNumber => Mathf.Min(_reserve.Value, _limit),
                BarDisplayFormat.Unlimited => _reserve.Value * _valueStepPerResourceUnit,
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