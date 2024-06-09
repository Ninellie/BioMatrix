using System;
using TMPro;
using UnityEngine;

namespace UIScripts.BasicElements.Interactive
{
    [Serializable]
    [CreateAssetMenu(fileName = "NewButtonStyle", menuName = "ScriptableObjects/ButtonStyle", order = 1)]
    public class ButtonStyleData : ScriptableObject
    {
        public TMP_ColorGradient normalColor;
        public TMP_ColorGradient hoverColor;
        public TMP_ColorGradient pressedColor;
        public TMP_ColorGradient disabledColor;
    }
}