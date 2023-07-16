using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.EntityComponents;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ResourceBar : MonoBehaviour
{
    [SerializeField]
    [Range(0, 1)]
    private float _value;

    [SerializeField]
    [Range(0, 100)]
    private float _currentValue;

    [SerializeField]
    [Range(0, 100)]
    private float _maxValue;

    [SerializeField]
    private string _resourceName;

    [Header("Background settings")]
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private TMP_ColorGradient _backgroundColor;

    [Header("Value slider settings")]
    [SerializeField] private TMP_ColorGradient _valueColor;
    [SerializeField] private Image _valueImage;
    [SerializeField] private RectTransform _sliderRectTransform;

    [Header("Value text settings")]
    
    [SerializeField] private bool _showText;
    [SerializeField] private TMP_Text _text;



    private void Awake()
    {
        SetValueImageColor(_valueColor);
        SetBackgroundImageColor(_backgroundColor);
    }

    private void Update()
    {
        var onePercent = _maxValue / 100;
        var currentPercent = _currentValue / onePercent;
        var integerCurrentPercent = Mathf.RoundToInt(currentPercent);
        _value = integerCurrentPercent / 100f;
        SetValue(_value);
        SetValueImageColor(_valueColor);
        SetBackgroundImageColor(_backgroundColor);
        SetText();
    }

    private void SetText()
    {
        if (!_showText) return;
        var textToSet = _resourceName + " " + _currentValue;
        _text.SetText(textToSet);
    }

    private void SetValue(float value)
    {
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

    //private Resource _targetResource;
}
