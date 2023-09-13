using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Core.Render
{
    [ExecuteInEditMode]
    public class ImageOutline : MonoBehaviour
    {
        public Color color = Color.white;
        private Image _image;

        void OnEnable()
        {
            _image = GetComponent<Image>();
            UpdateOutline(true);
        }

        void OnDisable()
        {
            UpdateOutline(false);
        }

        void Update()
        {
            UpdateOutline(true);
        }

        void UpdateOutline(bool outline)
        {
            _image.material.SetFloat("_Outline", outline ? 1f : 0);
            _image.material.SetColor("_OutlineColor", color);
        }
    }
}