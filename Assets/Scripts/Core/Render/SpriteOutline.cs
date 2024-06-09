using UnityEngine;

namespace Core.Render
{
    public class SpriteOutline : MonoBehaviour
    {
        [SerializeField] private Color _color = Color.white;
        [SerializeField] private bool _showOutline;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private void OnEnable()
        {
            if (_spriteRenderer == null) _spriteRenderer = GetComponent<SpriteRenderer>();
            UpdateOutline();
        }

        private void OnDisable()
        {
            UpdateOutline();
        }

        public void SetEnable(bool value)
        {
            _showOutline = value;
            UpdateOutline();
        }

        public void SetColor(Color value)
        {
            _color = value;
            UpdateOutline();
        }

        private void UpdateOutline()
        {
            var mpb = new MaterialPropertyBlock();
            _spriteRenderer.GetPropertyBlock(mpb);
            mpb.SetFloat("_Outline", _showOutline ? 1f : 0);
            mpb.SetColor("_OutlineColor", _color);
            _spriteRenderer.SetPropertyBlock(mpb);
        }
    }
}