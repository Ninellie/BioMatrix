using UnityEngine;
using UnityEngine.UI;

namespace Core.Render
{
    [ExecuteInEditMode]
    public class ImageOutline : MonoBehaviour
    {
        public Color color = Color.white;
        private Image _image;

        private void OnEnable()
        {
            _image = GetComponent<Image>();
            UpdateOutline(true);
        }

        private void OnDisable()
        {
            UpdateOutline(false);
        }

        private void Update()
        {
            UpdateOutline(true);
        }

        public void UpdateOutline(bool outline)
        {
            var mat = new Material(_image.material);
            mat.SetColor("_OutlineColor", color);
            mat.SetFloat("_Outline", outline ? 1f : 0);
            _image.material = mat;
        }
    }
}