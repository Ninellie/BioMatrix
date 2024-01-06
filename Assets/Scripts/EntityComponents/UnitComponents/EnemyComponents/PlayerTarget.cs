using Assets.Scripts.Core.Render;
using Assets.Scripts.Core.Sets;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.EnemyComponents
{
    public class PlayerTarget : MonoBehaviour
    {
        [SerializeField] private TMP_ColorGradient _outlineColor;
        [SerializeField] private PlayerTargetRuntimeSet _runtimeSet;
        [field: SerializeField] public bool IsCurrent { get; private set; }
        public Transform Transform { get; private set; }

        private SpriteRenderer SpriteRenderer
        {
            get
            {
                if (_spriteRenderer != null) return _spriteRenderer;
                _spriteRenderer = GetComponent<SpriteRenderer>();
                return _spriteRenderer;
            }
        }
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            if (Transform == null) Transform = transform;
        }

        private void OnBecameVisible()
        {
            _runtimeSet.Add(this);
        }

        private void OnBecameInvisible()
        {
            _runtimeSet.Remove(this);
            if (!IsCurrent) return;
            RemoveFromTarget();
        }

        public void TakeAsTarget()
        {
            var mpb = new MaterialPropertyBlock();
            SpriteRenderer.GetPropertyBlock(mpb);
            mpb.SetFloat("_Outline", 1f);
            mpb.SetColor("_OutlineColor", _outlineColor.topLeft);
            SpriteRenderer.SetPropertyBlock(mpb);
            IsCurrent = true;
        }

        public void RemoveFromTarget()
        {
            var mpb = new MaterialPropertyBlock();
            SpriteRenderer.GetPropertyBlock(mpb);
            mpb.SetFloat("_Outline", 0f);
            mpb.SetColor("_OutlineColor", _outlineColor.topLeft);
            SpriteRenderer.SetPropertyBlock(mpb);
            IsCurrent = false;
        }
    }
}