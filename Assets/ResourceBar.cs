using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.EntityComponents;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ResourceBar : MonoBehaviour
{
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private TMP_ColorGradient _backgroundColor;

    [SerializeField] private TMP_ColorGradient _valueColor;
    [SerializeField] private Image _valueImage;

    [SerializeField]
    [Range(0,1)]
    private float _value;

    [SerializeField] private RectTransform _rectTransform;

    private void Awake()
    {
        _backgroundImage = GetComponent<Image>();
        SetValueImageColor(_valueColor);
        SetBackgroundImageColor(_backgroundColor);

    }

    private void Update()
    {
        SetValue(_value);
        SetValueImageColor(_valueColor);
        SetBackgroundImageColor(_backgroundColor);
    }

    private void SetValue(float value)
    {
        _rectTransform.anchorMax = new Vector2(value, 1);
    }

    private void SetValueImageColor(TMP_ColorGradient color)
    {
        _backgroundImage.color = color.topLeft;
    }
    private void SetBackgroundImageColor(TMP_ColorGradient color)
    {
        _backgroundImage.color = color.topLeft;
    }

    //private Resource _targetResource;
}
