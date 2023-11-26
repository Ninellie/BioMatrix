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
        public Vector2Variable playerAimDirection;
        [Header("Settings")]
        public Vector2Reference playerPosition;
        public bool stickMode;
        [Header("Platform")]
        public bool platformCheck;
        public bool isMobile;

        public Camera mainCamera;

        private void Awake()
        {
            mainCamera = Camera.main;
            isMobile = Application.isMobilePlatform;
            if (!platformCheck) return;
            if (!isMobile) return;
            stickMode = true;
        }

        public void OnShoot(InputAction.CallbackContext context)
        {
            Debug.LogWarning($"On Shoot");
            var value = context.ReadValue<Vector2>();
            if (!stickMode)
            {
                value = Camera.main.ScreenToWorldPoint(value);
                value -= playerPosition;
                value.Normalize();
            }
            playerAimDirection.SetValue(value);
            onFire.Raise();
        }
    }
}