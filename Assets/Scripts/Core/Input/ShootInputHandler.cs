using Assets.Scripts.Core.Events;
using Assets.Scripts.Core.Variables;
using Assets.Scripts.Core.Variables.References;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Core.Input
{
    public class ShootInputHandler : MonoBehaviour
    {
        public GameEvent onFire;
        public GameEvent onAimModeChange;
        public Vector2Variable playerAimDirection;
        public Vector2 rawAimDirection;
        [Header("Settings")]
        public Vector2Reference firePoint;
        public bool stickMode;
        public Camera mainCamera;
        public bool platformCheck;
        [Header("Dynamic")]
        public bool isMobile;
        public bool fireButtonPressed;
        public bool autoAim;

        private void Awake()
        {
            mainCamera = Camera.main;
            isMobile = Application.isMobilePlatform;
            if (!platformCheck) return;
            if (!isMobile) return;
            stickMode = true;
        }

        private void FixedUpdate()
        {
            if (!stickMode)
            {
                Vector2 value = Camera.main.ScreenToWorldPoint(rawAimDirection);
                value -= firePoint;
                value.Normalize();
                playerAimDirection.SetValue(value);
            }
            else
            {
                playerAimDirection.SetValue(rawAimDirection);
            }

            if (fireButtonPressed)
            {
                onFire.Raise();
            }
        }

        public void SetFireButtonPressed(bool isPressed)
        {
            fireButtonPressed = isPressed;
        }

        public void OnShoot(InputAction.CallbackContext context)
        {
            Debug.LogWarning($"On Shoot");

            if (!autoAim)
            {
                rawAimDirection = context.ReadValue<Vector2>();
            }

            fireButtonPressed = context.action.IsPressed();
        }

        public void OnAimModeChange(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            Debug.LogWarning($"On Aim Mode Changed");
            autoAim = !autoAim;
            onAimModeChange.Raise();
        }
    }
}