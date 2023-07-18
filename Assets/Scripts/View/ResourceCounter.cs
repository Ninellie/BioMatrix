using System;
using Assets.Scripts.EntityComponents;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.View
{
    public class ResourceCounter : MonoBehaviour
    {
        [SerializeField]
        private string _resourceName = "Simple Resource";

        [Header("Text settings")]
        [SerializeField] private CounterFormat _counterFormat;
    
        [Header("Counter text")]
        [SerializeField] private TMP_Text _text;

        protected Resource targetResource;

        public void SetResource(Resource resource)
        {
            targetResource = resource;
            targetResource.ValueChangedEvent += UpdateCounter;
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
            var textToSet = _counterFormat switch
            {
                CounterFormat.LabelCurrentSlashMax => $"{_resourceName} {targetResource.GetValue()}/{targetResource.GetMaxValue()}",
                CounterFormat.CurrentSlashMax => $"{targetResource.GetValue()}/{targetResource.GetMaxValue()}",
                CounterFormat.LabelCurrentOnly => $"{_resourceName} {targetResource.GetValue()}",
                CounterFormat.CurrentOnly => $"{targetResource.GetValue()}",
                CounterFormat.LabelOnly => $"{_resourceName}",
                CounterFormat.DoNotShow => $"",
                _ => throw new ArgumentOutOfRangeException()
            };
            _text.SetText(textToSet);
        }
    }
}