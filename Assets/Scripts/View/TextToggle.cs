using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameSession.UIScripts.View
{
    public class TextToggle : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private string _falseLabel;
        [SerializeField] private string _trueLabel;
        
        private Toggle _toggle;

        private void Awake()
        {
            _toggle = GetComponent<Toggle>();
        }

        public void ChangeText(bool state)
        {
            _text.text = state == true ? _trueLabel : _falseLabel;
        }
    }
}