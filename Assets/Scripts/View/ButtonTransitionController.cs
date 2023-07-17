using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.View
{
    [Serializable]
    public enum ButtonState
    {
        Normal,
        Hovered,
        Pressed,
    }

    [ExecuteInEditMode]
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(EventTrigger))]
    public class ButtonTransitionController : MonoBehaviour
    {
        [SerializeField]
        private ButtonStyleData _style;

        private Button _button;
        private TMP_Text _label;
        private EventTrigger _eventTrigger;
        [SerializeField]
        private ButtonState _currentState;
        [SerializeField]
        private ButtonState _posState;

        private void Awake()
        {
            _eventTrigger = GetComponent<EventTrigger>();
            _label = GetComponentInChildren<TMP_Text>();
            _button = GetComponent<Button>();
            _button.transition = Selectable.Transition.None;
        }
        
        private void OnEnable()
        {
            if (_button.IsInteractable())
            {
                SetNormalColor();
            }
            else
            {
                SetDisabledColor();
            }
            _posState = ButtonState.Normal;
            _currentState = ButtonState.Normal;
        }

        private void Start()
        {
            _eventTrigger.triggers.Clear();
            CreateEvent(EventTriggerType.PointerEnter, OnEnter);
            CreateEvent(EventTriggerType.PointerExit, OnExit);
            CreateEvent(EventTriggerType.PointerDown, OnDawn);
            CreateEvent(EventTriggerType.PointerUp, OnUp);
        }

        private void CreateEvent(EventTriggerType eventType, Action action)
        {
            var eventTrigger = new EventTrigger.Entry();
            eventTrigger.eventID = eventType;
            eventTrigger.callback.AddListener((eventData) => action());
            _eventTrigger.triggers.Add(eventTrigger);
        }

        private void OnEnter()
        {
            if (!_button.IsInteractable()) return;
            _posState = ButtonState.Hovered;
            if (_currentState != ButtonState.Normal) return;
            SetHoverColor();
            _currentState = ButtonState.Hovered;
        }

        private void OnExit()
        {
            if (!_button.IsInteractable()) return;
            _posState = ButtonState.Normal;
            if (_currentState != ButtonState.Hovered) return;
            SetNormalColor();
            _currentState = ButtonState.Normal;
        }

        private void OnDawn()
        {
            if (!_button.IsInteractable()) return;
            if (_currentState != ButtonState.Hovered) return;
            SetPressedColor();
            _currentState = ButtonState.Pressed;
        }
        
        private void OnUp()
        {
            if (!_button.IsInteractable()) return;
            if (_currentState == ButtonState.Pressed && _posState == ButtonState.Hovered)
            {
                SetHoverColor();
                _currentState = ButtonState.Hovered;
            }
            if (_currentState == ButtonState.Pressed && _posState == ButtonState.Normal)
            {
                SetNormalColor();
                _currentState = ButtonState.Normal;
            }
        }

        private void SetNormalColor()
        {
            _label.colorGradientPreset = _style.normalColor;
        }

        private void SetHoverColor()
        {
            _label.colorGradientPreset = _style.hoverColor;
        }

        private void SetPressedColor()
        {
            _label.colorGradientPreset = _style.pressedColor;
        }

        private void SetDisabledColor()
        {
            _label.colorGradientPreset = _style.disabledColor;
        }
    }
}