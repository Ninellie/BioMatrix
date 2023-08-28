using System;
using Assets.Scripts.EntityComponents;
using Assets.Scripts.EntityComponents.Resources;
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

    [AddComponentMenu("UI/Resource/Counter")]
    public class ResourceCounter : MonoBehaviour
    {
        [SerializeField]
        private string _resourceName = "Simple Resource";

        [Header("Text settings")]
        [SerializeField] private CounterFormat _counterFormat;
        [SerializeField] private ValueFormat _valueFormat;
    
        [Header("Counter text")]
        [SerializeField] private TMP_Text _text;

        protected Resource targetResource;

        public void SetResource(Resource resource)
        {
            targetResource = resource;
            targetResource.AddListenerToEvent(ResourceEventType.ValueChanged).AddListener(UpdateCounter);
            UpdateCounter();
        }

        public void SetLabel(string label)
        {
            _resourceName = label;
            UpdateLabel();
        }

        protected virtual void UpdateCounter()
        {
            UpdateLabel();
        }

        private void UpdateLabel()
        {
            var currentValue = _valueFormat switch
            {
                ValueFormat.D => $"{targetResource.GetValue():D}",
                ValueFormat.D1 => $"{targetResource.GetValue():D1}",
                ValueFormat.D2 => $"{targetResource.GetValue():D2}",
                ValueFormat.D3 => $"{targetResource.GetValue():D3}",
                _ => throw new ArgumentOutOfRangeException()
            };
            var maxValue = _valueFormat switch
            {
                ValueFormat.D => $"{(int)targetResource.GetMaxValue():D}",
                ValueFormat.D1 => $"{(int)targetResource.GetMaxValue():D1}",
                ValueFormat.D2 => $"{(int)targetResource.GetMaxValue():D2}",
                ValueFormat.D3 => $"{(int)targetResource.GetMaxValue():D3}",
                _ => throw new ArgumentOutOfRangeException()
            };

            var textToSet = _counterFormat switch
            {
                CounterFormat.LabelCurrentSlashMax => $"{_resourceName} {currentValue}/{maxValue}",
                CounterFormat.CurrentSlashMax => $"{currentValue}/{maxValue}",
                CounterFormat.LabelCurrentOnly => $"{_resourceName} {currentValue}",
                CounterFormat.CurrentOnly => $"{currentValue}",
                CounterFormat.LabelOnly => $"{_resourceName}",
                CounterFormat.DoNotShow => $"",
                _ => throw new ArgumentOutOfRangeException()
            };
            _text.SetText(textToSet);
        }
    }
}