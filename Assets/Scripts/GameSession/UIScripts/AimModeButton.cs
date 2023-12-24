using System;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.GameSession.UIScripts
{
    [Serializable]
    public enum AimMode
    {
        SelfAim,
        AutoAim,
    }

    public class AimModeButton : MonoBehaviour
    {
        [SerializeField] private AimMode _aimMode;
        [SerializeField] private TMP_Text _buttonText;
        [SerializeField] private GameObject _selfAimModeController;
        [SerializeField] private GameObject _autoAimModeController;

        private void Awake()
        {
            _buttonText = GetComponentInChildren<TMP_Text>();
            _aimMode = AimMode.AutoAim;
        }

        public void ChangeAimMode()
        {
            switch (_aimMode)
            {
                case AimMode.AutoAim:
                    SetSelfAimMode();
                    break;
                case AimMode.SelfAim:
                    SetAutoAimMode();
                    break;
            }
            _buttonText.text = "Aim: " + _aimMode;
            _autoAimModeController.SetActive(_aimMode == AimMode.AutoAim);
            _selfAimModeController.SetActive(_aimMode == AimMode.SelfAim);
        }

        private void SetAutoAimMode()
        {
            _aimMode = AimMode.AutoAim;
        }

        private void SetSelfAimMode()
        {
            _aimMode = AimMode.SelfAim;
        }
    }
}